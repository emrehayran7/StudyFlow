using MediatR;
using StudyFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Commands.Auth.Logout.Request
{
    public record LogoutCommand(LogoutDto LogoutDto) : IRequest<Result>;

}
