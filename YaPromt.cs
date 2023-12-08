using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Promt
{
    internal class YaPromt
    {
        public string ModelUri { get; set; }
        public YaCompletionOptions CompletionOptions { get; set; } = new YaCompletionOptions();
        public List<YaMessage> Messages { get; set; } = new List<YaMessage>();
    }

    internal class YaCompletionOptions
    {
        public bool Stream { get; set; } = false;
        public double Temperature { get; set; } = 0.6;
        public int MaxTokens { get; set; } = 2000;
    }

    internal class YaMessage
    {
        public YaMessage(string text)
        {
            Text = text;
        }

        public string Role { get; set; } = "user";
        public string Text { get; set; }
    }
}
