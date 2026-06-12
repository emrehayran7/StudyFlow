using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using StudyFlow.Core.Helper;

namespace StudyFlow.Infrastructure.Services;

public class OpenAiService : IAiService
{
    private readonly ChatClient _chatClient;

    public OpenAiService(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("OpenAI API key not found.");

        _chatClient = new ChatClient("gpt-4o-mini", apiKey);
    }

    public async Task<string> AskAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var response = await _chatClient.CompleteChatAsync(
            [new UserChatMessage(prompt)],
            cancellationToken: cancellationToken
        );

        return response.Value.Content[0].Text;
    }
}
