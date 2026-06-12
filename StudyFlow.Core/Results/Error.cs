using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Results
{
    public record Error(string Code, string Description, ErrorType Type);
}
