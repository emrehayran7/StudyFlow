using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Security.Claims;

namespace StudyFlow.Api.src.Middleware
{
    public sealed class StudyFlowRequestMiddleware
    {
        private const string CorrelationIdHeaderName = "X-Correlation-Id";
        private const string SessionIdHeaderName = "X-Session-Id";
        private const string BearerPrefix = "Bearer ";

        private readonly RequestDelegate _next;
        private readonly ILogger<StudyFlowRequestMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public StudyFlowRequestMiddleware(
            RequestDelegate next,
            ILogger<StudyFlowRequestMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string correlationId = ResolveHeaderValue(context, CorrelationIdHeaderName)
                ?? Guid.NewGuid().ToString("N");
            string sessionId = ResolveHeaderValue(context, SessionIdHeaderName)
                ?? Guid.NewGuid().ToString("N");

            EnrichRequest(context, correlationId, sessionId);

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                RequestValidationResult validationResult = ValidateRequest(context);
                if (!validationResult.IsValid)
                {
                    await WriteErrorAsync(
                        context,
                        validationResult.StatusCode,
                        validationResult.Code,
                        validationResult.Message,
                        correlationId);
                    return;
                }

                bool bearerTokenValid = await ValidateBearerTokenAsync(context, correlationId);
                if (!bearerTokenValid)
                {
                    return;
                }

                EnrichUserSessionAndClaims(context, correlationId, sessionId);

                await _next(context);
            }
            catch (BadHttpRequestException exception)
            {
                await HandleExceptionAsync(
                    context,
                    exception,
                    StatusCodes.Status400BadRequest,
                    "Request.Invalid",
                    "The request is invalid.",
                    correlationId);
            }
            catch (UnauthorizedAccessException exception)
            {
                await HandleExceptionAsync(
                    context,
                    exception,
                    StatusCodes.Status401Unauthorized,
                    "Auth.Unauthorized",
                    "The request is not authorized.",
                    correlationId);
            }
            catch (KeyNotFoundException exception)
            {
                await HandleExceptionAsync(
                    context,
                    exception,
                    StatusCodes.Status404NotFound,
                    "Resource.NotFound",
                    "The requested resource was not found.",
                    correlationId);
            }
            catch (ArgumentException exception)
            {
                await HandleExceptionAsync(
                    context,
                    exception,
                    StatusCodes.Status400BadRequest,
                    "Request.ArgumentInvalid",
                    "One or more request arguments are invalid.",
                    correlationId);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(
                    context,
                    exception,
                    StatusCodes.Status500InternalServerError,
                    "Server.UnhandledError",
                    "An unexpected server error occurred.",
                    correlationId);
            }
            finally
            {
                stopwatch.Stop();

                StudyFlowUserSession? userSession = context.Items[StudyFlowRequestItems.UserSession] as StudyFlowUserSession;

                _logger.LogInformation(
                    "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds} ms. CorrelationId: {CorrelationId}; SessionId: {SessionId}; UserId: {UserId}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds,
                    correlationId,
                    sessionId,
                    userSession?.UserId);
            }
        }

        private static void EnrichRequest(HttpContext context, string correlationId, string sessionId)
        {
            context.TraceIdentifier = correlationId;
            context.Items[StudyFlowRequestItems.CorrelationId] = correlationId;
            context.Items[StudyFlowRequestItems.SessionId] = sessionId;
            context.Response.Headers[CorrelationIdHeaderName] = correlationId;
            context.Response.Headers[SessionIdHeaderName] = sessionId;
        }

        private static string? ResolveHeaderValue(HttpContext context, string headerName)
        {
            string? value = context.Request.Headers[headerName].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            value = value.Trim();

            return value.Length <= 128 ? value : null;
        }

        private static RequestValidationResult ValidateRequest(HttpContext context)
        {
            if (RequestCanHaveBody(context.Request) &&
                context.Request.ContentLength.GetValueOrDefault() > 0 &&
                !IsJsonRequest(context.Request))
            {
                return RequestValidationResult.Invalid(
                    StatusCodes.Status415UnsupportedMediaType,
                    "Request.UnsupportedMediaType",
                    "Request body must use a JSON content type.");
            }

            RequestValidationResult routeValidationResult = ValidateIdentifierValues(context.Request.RouteValues);
            if (!routeValidationResult.IsValid)
            {
                return routeValidationResult;
            }

            Dictionary<string, object?> queryValues = context.Request.Query
                .ToDictionary(x => x.Key, x => (object?)x.Value.ToString(), StringComparer.OrdinalIgnoreCase);

            return ValidateIdentifierValues(queryValues);
        }

        private static bool RequestCanHaveBody(HttpRequest request)
        {
            return HttpMethods.IsPost(request.Method) ||
                   HttpMethods.IsPut(request.Method) ||
                   HttpMethods.IsPatch(request.Method);
        }

        private static bool IsJsonRequest(HttpRequest request)
        {
            return !string.IsNullOrWhiteSpace(request.ContentType) &&
                   request.ContentType.Contains("json", StringComparison.OrdinalIgnoreCase);
        }

        private static RequestValidationResult ValidateIdentifierValues(IEnumerable<KeyValuePair<string, object?>> values)
        {
            foreach (KeyValuePair<string, object?> value in values)
            {
                if (!IsIdentifierKey(value.Key) || value.Value == null)
                {
                    continue;
                }

                string stringValue = value.Value.ToString() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    continue;
                }

                if (!int.TryParse(stringValue, out int id) || id <= 0)
                {
                    return RequestValidationResult.Invalid(
                        StatusCodes.Status400BadRequest,
                        "Request.InvalidIdentifier",
                        $"{value.Key} must be a positive integer.");
                }
            }

            return RequestValidationResult.Valid();
        }

        private static bool IsIdentifierKey(string key)
        {
            return key.Equals("id", StringComparison.OrdinalIgnoreCase) ||
                   key.EndsWith("Id", StringComparison.OrdinalIgnoreCase);
        }

        private async Task<bool> ValidateBearerTokenAsync(HttpContext context, string correlationId)
        {
            if (!EndpointRequiresAuthorization(context))
            {
                return true;
            }

            string authorizationHeader = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authorizationHeader))
            {
                await WriteErrorAsync(
                    context,
                    StatusCodes.Status401Unauthorized,
                    "Auth.BearerTokenMissing",
                    "Bearer token is required.",
                    correlationId);
                return false;
            }

            if (!authorizationHeader.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
            {
                await WriteErrorAsync(
                    context,
                    StatusCodes.Status401Unauthorized,
                    "Auth.BearerTokenInvalidFormat",
                    "Authorization header must use the Bearer scheme.",
                    correlationId);
                return false;
            }

            string token = authorizationHeader[BearerPrefix.Length..].Trim();
            if (string.IsNullOrWhiteSpace(token))
            {
                await WriteErrorAsync(
                    context,
                    StatusCodes.Status401Unauthorized,
                    "Auth.BearerTokenEmpty",
                    "Bearer token cannot be empty.",
                    correlationId);
                return false;
            }

            if (context.User.Identity?.IsAuthenticated != true)
            {
                await WriteErrorAsync(
                    context,
                    StatusCodes.Status401Unauthorized,
                    "Auth.BearerTokenInvalid",
                    "Bearer token is invalid or expired.",
                    correlationId);
                return false;
            }

            int? userId = TryGetUserId(context.User);
            if (userId == null)
            {
                await WriteErrorAsync(
                    context,
                    StatusCodes.Status401Unauthorized,
                    "Auth.UserIdClaimMissing",
                    "Bearer token does not contain a valid user id claim.",
                    correlationId);
                return false;
            }

            return true;
        }

        private static bool EndpointRequiresAuthorization(HttpContext context)
        {
            Endpoint? endpoint = context.GetEndpoint();

            if (endpoint == null)
            {
                return false;
            }

            bool allowsAnonymous = endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null;
            if (allowsAnonymous)
            {
                return false;
            }

            return endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>().Count > 0;
        }

        private static void EnrichUserSessionAndClaims(HttpContext context, string correlationId, string sessionId)
        {
            int? userId = TryGetUserId(context.User);
            string? email = context.User.FindFirst(ClaimTypes.Email)?.Value;
            List<string> roles = context.User
                .FindAll(ClaimTypes.Role)
                .Select(x => x.Value)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            StudyFlowUserSession userSession = new StudyFlowUserSession(
                context.User.Identity?.IsAuthenticated == true,
                userId,
                email,
                roles,
                correlationId,
                sessionId);

            context.Items[StudyFlowRequestItems.UserSession] = userSession;

            if (context.User.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
            {
                return;
            }

            AddClaimIfMissing(identity, StudyFlowClaimTypes.RequestId, correlationId);
            AddClaimIfMissing(identity, StudyFlowClaimTypes.SessionId, sessionId);

            if (userId != null)
            {
                AddClaimIfMissing(identity, StudyFlowClaimTypes.UserId, userId.Value.ToString());
            }
        }

        private static int? TryGetUserId(ClaimsPrincipal user)
        {
            string? userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(userIdClaim, out int userId)
                ? userId
                : null;
        }

        private static void AddClaimIfMissing(ClaimsIdentity identity, string type, string value)
        {
            if (!identity.HasClaim(type, value))
            {
                identity.AddClaim(new Claim(type, value));
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            int statusCode,
            string code,
            string message,
            string correlationId)
        {
            if (context.Response.HasStarted)
            {
                throw exception;
            }

            if (statusCode >= StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception, "Unhandled exception. CorrelationId: {CorrelationId}", correlationId);
            }
            else
            {
                _logger.LogWarning(exception, "Request failed. CorrelationId: {CorrelationId}", correlationId);
            }

            string? details = _environment.IsDevelopment()
                ? exception.Message
                : null;

            await WriteErrorAsync(context, statusCode, code, message, correlationId, details);
        }

        private static async Task WriteErrorAsync(
            HttpContext context,
            int statusCode,
            string code,
            string message,
            string correlationId,
            string? details = null)
        {
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            context.Response.Headers[CorrelationIdHeaderName] = correlationId;

            if (context.Items[StudyFlowRequestItems.SessionId] is string sessionId)
            {
                context.Response.Headers[SessionIdHeaderName] = sessionId;
            }

            ApiErrorResponse response = new ApiErrorResponse(
                false,
                statusCode,
                code,
                message,
                correlationId,
                context.Request.Path,
                details);

            await context.Response.WriteAsJsonAsync(response);
        }

        private sealed record ApiErrorResponse(
            bool Success,
            int StatusCode,
            string Code,
            string Message,
            string CorrelationId,
            string Path,
            string? Details);

        private sealed record RequestValidationResult(
            bool IsValid,
            int StatusCode,
            string Code,
            string Message)
        {
            public static RequestValidationResult Valid()
            {
                return new RequestValidationResult(true, StatusCodes.Status200OK, string.Empty, string.Empty);
            }

            public static RequestValidationResult Invalid(int statusCode, string code, string message)
            {
                return new RequestValidationResult(false, statusCode, code, message);
            }
        }
    }
}
