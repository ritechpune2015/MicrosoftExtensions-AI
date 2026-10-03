using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string apiKey = config["OpenAIKey"]!;
string modelName = config["ModelName"]!;

IChatClient chatClient =
    new OpenAIClient(apiKey).GetChatClient(modelName).AsIChatClient();

Console.WriteLine("Client Created!");

var messages = new List<ChatMessage>();

messages.Add(
    new ChatMessage(
        ChatRole.System,
        """
        You are an AI customer support assistant.

        Your responsibilities:
        - Help customers with their questions.
        - Be polite and professional.
        - Keep answers simple.
        - Ask for additional information when required.
        - Never invent order information.
        """
    )
);

var options = new ChatOptions
{
    Temperature = 0.2f,
    MaxOutputTokens = 2000
};


Console.WriteLine("========================================");
Console.WriteLine("       RI-TECH AI CUSTOMER SUPPORT");
Console.WriteLine("========================================");
Console.WriteLine();
Console.WriteLine("Type 'exit' to stop.");
Console.WriteLine();



while (true)
{
    Console.Write("Customer: ");

    string? userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput))
        continue;

    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    messages.Add(
        new ChatMessage(
            ChatRole.User,
            userInput
        )
    );

    var response =
        await chatClient.GetResponseAsync(messages,options);

    Console.WriteLine();

    Console.WriteLine("AI:");
    Console.WriteLine(response.Text);

    Console.WriteLine();

    messages.Add(
        new ChatMessage(
            ChatRole.Assistant,
            response.Text
        )
    );
}


