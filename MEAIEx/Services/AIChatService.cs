using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEAIEx.Services
{
    public class AIChatService : IAIChatService
    {
        private readonly IChatClient _chatClient;
        private readonly AISettings _settings;

        private readonly List<ChatMessage> _messages = new();

        public AIChatService(
            IChatClient chatClient,
            AISettings settings)
        {
            _chatClient = chatClient;
            _settings = settings;

            AddSystemMessage();
        }


        private void AddSystemMessage()
        {
            _messages.Add(
                new ChatMessage(
                    ChatRole.System,
                    """
                You are an AI customer support assistant.

                Rules:

                1. Be polite and professional.
                2. Keep answers concise.
                3. Ask questions when information is missing.
                4. Never invent customer information.
                5. Do not claim a refund was processed
                   unless the system confirms it.
                """
                )
            );
        }

        public async Task<string> SendMessageAsync(string message,
            CancellationToken cancellationToken = default)
        {
            _messages.Add(
                new ChatMessage(
                    ChatRole.User,
                    message
                )
            );

            TrimConversationHistory();

            var options = new ChatOptions
            {
                Temperature = _settings.Temperature,
                MaxOutputTokens = _settings.MaxOutputTokens
            };

            var response =
                await _chatClient.GetResponseAsync(
                    _messages,
                    options,
                    cancellationToken);

            _messages.Add(
                new ChatMessage(
                    ChatRole.Assistant,
                    response.Text
                )
            );

            return response.Text;
        }

        private void TrimConversationHistory()
        {
            int maxMessages =
                _settings.MaxConversationMessages;

            if (_messages.Count <= maxMessages)
                return;

            var systemMessage = _messages[0];

            var recentMessages =
                _messages
                    .Skip(_messages.Count - (maxMessages - 1))
                    .ToList();

            _messages.Clear();

            _messages.Add(systemMessage);

            _messages.AddRange(recentMessages);
        }
    }
}


