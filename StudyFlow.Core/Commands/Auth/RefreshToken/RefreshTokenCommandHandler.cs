using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StudyFlow.Core.Auth;
using StudyFlow.Core.Commands.Auth.RefreshToken.Request;
using StudyFlow.Core.Commands.Auth.Response;
using StudyFlow.Core.Helper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Auth.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>

    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenCommandHandler(
            StudyFlowDbContext dbContext,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<Result<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            string oldRefreshToken = request.RefreshTokenDto.RefreshToken;
            string oldRefreshTokenHash = _refreshTokenService.HashRefreshToken(oldRefreshToken);

            StudyFlow.Domain.Entities.RefreshToken? storedToken = await _dbContext.RefreshTokens
                .Include(x => x.User)
                .ThenInclude(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.TokenHash == oldRefreshTokenHash, cancellationToken);
            if (storedToken == null || !storedToken.IsActive)
            {
                return Result<AuthResponseDto>.Failure(AuthErrors.InvalidRefreshToken);
            }
            string newAccessToken = _jwtService.GenerateAccessToken(storedToken.User, out DateTime accessTokenExpiresAt);

            string newRefreshToken = _refreshTokenService.GenerateRefreshToken();
            string newRefreshTokenHash = _refreshTokenService.HashRefreshToken(newRefreshToken);

            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.ReplacedByTokenHash = newRefreshTokenHash;

            var newRefreshTokenEntity = new
            StudyFlow.Domain.Entities.RefreshToken
            {
                UserId = storedToken.UserId,
                TokenHash = newRefreshTokenHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };

            _dbContext.RefreshTokens.Add(newRefreshTokenEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<AuthResponseDto>.Success(new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiresAt = accessTokenExpiresAt
            });
        }

    }
}
