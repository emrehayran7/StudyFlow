using MediatR;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Core.Commands.Auth.Logout.Request;
using StudyFlow.Core.Helper;
using StudyFlow.Core.Results;
using StudyFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Auth.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly StudyFlowDbContext _dbContext;
        private readonly IRefreshTokenService _refreshTokenService;

        public LogoutCommandHandler(StudyFlowDbContext dbContext, IRefreshTokenService refreshTokenService)
        {
            _dbContext = dbContext;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            string refreshTokenHash = _refreshTokenService.HashRefreshToken(request.LogoutDto.RefreshToken);

            StudyFlow.Domain.Entities.RefreshToken storedToken = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.TokenHash == refreshTokenHash, cancellationToken);

            if (storedToken == null)
            {
                return Result.Failure(AuthErrors.InvalidRefreshToken);
            }

            storedToken.RevokedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
