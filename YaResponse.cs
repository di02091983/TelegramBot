using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Response
{
    internal class YaResponse
    {
        public YaResult Result { get; set; }
    }

    internal class YaResult
    {
        public IEnumerable<YaAlternative> Alternatives { get; set; }
        public YaUsage Usage { get; set; }
        public string modelVersion { get; set; }
    }

    internal class YaAlternative
    {
        public YaMessage Message { get; set; }
        public string Status { get; set; }
    }

    internal class YaMessage
    {
        public string Role { get; set; }
        public string Text { get; set; }
    }

    internal class YaUsage
    {
        public string InputTextTokens { get; set; }
        public string CompletionTokens { get; set; }
        public string totalTokens { get; set; }
    }
}
