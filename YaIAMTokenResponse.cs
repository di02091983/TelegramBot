using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    internal class YaIAMTokenResponse
    {
        public string IAMToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
