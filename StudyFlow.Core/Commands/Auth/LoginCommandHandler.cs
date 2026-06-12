using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StudyFlow.Core.Auth;
using StudyFlow.Core.Commands.Auth.Request;
using StudyFlow.Core.Commands.Auth.Response;
using StudyFlow.Core.Helper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Auth
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtSettings _jwtSettings;
        public LoginCommandHandler(
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
        public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            LoginDto logindto = request.LoginDto;

            User user = await _dbContext.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == logindto.Email, cancellationToken);
            if (user == null)
            {
                return Result<AuthResponseDto>.Failure(AuthErrors.InvalidCredentials);
            }
            bool passwordValid = _passwordHasher.VerifyPassword(logindto.Password, user.Password);
            if (!passwordValid)
            {
                return Result<AuthResponseDto>.Failure(AuthErrors.InvalidCredentials);
            }
            string accessToken = _jwtService.GenerateAccessToken(user, out DateTime accessTokenExpiresAt);
            string refreshToken = _refreshTokenService.GenerateRefreshToken();
            string refreshTokenHash = _refreshTokenService.HashRefreshToken(refreshToken);


            StudyFlow.Domain.Entities.RefreshToken refreshTokenEntity = new
            StudyFlow.Domain.Entities.RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshTokenHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
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
