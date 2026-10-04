using MEAIEx;
using MEAIEx.Services;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;

var configuration =
    new ConfigurationBuilder()
        .AddJsonFile(
            "appsettings.json",
            optional: false)
        .AddUserSecrets<Program>()
        .Build();

string apiKey = configuration["OpenAIKey"]!;
//string modelName = configuration["ModelName"]!;

var settings = configuration.GetSection("AISettings").Get<AISettings>()!;

IChatClient chatClient =
    new OpenAIClient(apiKey)
        .GetChatClient(settings.ModelName)
        .AsIChatClient();

IAIChatService chatService = new AIChatService(chatClient, settings);

Console.WriteLine("======================================");
Console.WriteLine("     RI-TECH AI CUSTOMER SUPPORT");
Console.WriteLine("======================================");
Console.WriteLine("Type 'exit' to stop.");
Console.WriteLine();

while (true)
{
    Console.Write("Customer: ");

    string? input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        continue;

    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    string response =
        await chatService.SendMessageAsync(input);

    Console.WriteLine();
    Console.WriteLine("AI:");
    Console.WriteLine(response);
    Console.WriteLine();
}
