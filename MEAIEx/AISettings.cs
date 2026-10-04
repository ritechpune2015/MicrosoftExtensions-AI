using System;
using System.Collections.Generic;
using System.Text;

namespace MEAIEx
{
    public class AISettings
    {
        public string ModelName { get; set; } = string.Empty;

        public float Temperature { get; set; } = 0.2f;

        public int MaxOutputTokens { get; set; } = 500;

        public int MaxConversationMessages { get; set; } = 20;
    }

}
