using MediatR;
using StudyFlow.Core.Commands.Auth.Response;
using StudyFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Auth.RefreshToken.Request
{
    public record RefreshTokenCommand(RefreshTokenDto RefreshTokenDto) : IRequest<Result<AuthResponseDto>>;

}
