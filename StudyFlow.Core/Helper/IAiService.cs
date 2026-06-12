using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Helper
{
    public interface IAiService
    {
        Task<string> AskAsync(string prompt, CancellationToken cancellationToken = default);
    }
}
