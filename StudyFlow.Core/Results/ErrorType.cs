using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Results
{
    public enum ErrorType
    {
        Validation,
        NotFound,
        Conflict,
        Unauthorized,
        Forbidden,
        Failure
    }
}
