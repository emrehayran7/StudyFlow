using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StudyFlow.Core.Auth;
using StudyFlow.Core.Commands.Auth.Register.Request;
using StudyFlow.Core.Commands.Auth.Response;
using StudyFlow.Core.Helper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Core.Commands.Auth.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
    {
        private const string DefaultRoleName = "User";

        private readonly StudyFlowDbContext _dbContext;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly JwtSettings _jwtSettings;

        public RegisterCommandHandler(
            StudyFlowDbContext dbContext,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService,
            IPasswordHasherService passwordHasher,
            IOptions<JwtSettings> jwtSettings)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Result<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            RegisterDto dto = request.RegisterDto;

            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                return Result<AuthResponseDto>.Failure(AuthErrors.FirstNameRequired);
            }

            if (string.IsNullOrWhiteSpace(dto.LastName))
            {
                return Result<AuthResponseDto>.Failure(AuthErrors.LastNameRequired);
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return Result<AuthResponseDto>.Failure(AuthErrors.EmailRequired);
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                return Result<AuthResponseDto>.Failure(AuthErrors.PasswordRequired);
            }

            if (dto.Password.Length < 6)
            {
                return Result<AuthResponseDto>.Failure(AuthErrors.PasswordTooShort);
            }

            string normalizedEmail = dto.Email.Trim().ToLowerInvariant();

            bool emailExists = await _dbContext.Users
                .AnyAsync(x => x.Email == normalizedEmail, cancellationToken);

            if (emailExists)
            {
                return Result<AuthResponseDto>.Failure(AuthErrors.EmailAlreadyExists);
            }

            DateTime now = DateTime.UtcNow;

            


            var user = new User
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = normalizedEmail,
                Password = _passwordHasher.HashPassword(dto.Password),
                EducationLevel = string.IsNullOrWhiteSpace(dto.EducationLevel) ? null : dto.EducationLevel.Trim(),
                CreatedAt = now,
                CreatedBy = "Register"
            };

            Role? defaultRole = await _dbContext.Roles
                .FirstOrDefaultAsync(x => x.Name == DefaultRoleName, cancellationToken);

            if (defaultRole == null)
            {
                defaultRole = new Role
                {
                    Name = DefaultRoleName,
                    Description = "Default role for registered users."
                };

                _dbContext.Roles.Add(defaultRole);
            }

            var userRole = new UserRole
            {
                User = user,
                Role = defaultRole
            };

            user.UserRoles.Add(userRole);

            _dbContext.Users.Add(user);
            _dbContext.UserRoles.Add(userRole);
            await _dbContext.SaveChangesAsync(cancellationToken);

            string accessToken = _jwtService.GenerateAccessToken(user, out DateTime accessTokenExpiresAt);
            string refreshToken = _refreshTokenService.GenerateRefreshToken();
            string refreshTokenHash = _refreshTokenService.HashRefreshToken(refreshToken);

            var refreshTokenEntity = new StudyFlow.Domain.Entities.RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshTokenHash,
                CreatedAt = now,
                ExpiresAt = now.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };

            _dbContext.RefreshTokens.Add(refreshTokenEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<AuthResponseDto>.Success(new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = accessTokenExpiresAt
            });
        }
    }
}
