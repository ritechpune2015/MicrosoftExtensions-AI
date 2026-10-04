using System;
using System.Collections.Generic;
using System.Text;

namespace MEAIEx.Services
{
    public interface IAIChatService
    {
        Task<string> SendMessageAsync(string message, CancellationToken cancellationToken = default);
    }
}
