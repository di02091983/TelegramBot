using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests;
using TelegramBot.Promt;
using TelegramBot.Response;

namespace TelegramBot
{
    internal class YaGPT
    {
        HttpClient client;
        private string iamToken;
        private string yandexFolderId;

        const string apiUrl = "https://llm.api.cloud.yandex.net/foundationModels/v1/completion";

        public YaGPT(IConfiguration configuration)
        {
            iamToken = configuration.GetValue<string>("IamToken");
            yandexFolderId = configuration.GetValue<string>("YandexFolderId");
        }

        public async Task<string> SendRequest(string message)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {iamToken}");
            client.DefaultRequestHeaders.Add("x-folder-id", yandexFolderId);

            var promt =  new YaPromt();
            promt.ModelUri = $"gpt://{yandexFolderId}/yandexgpt-lite";
            promt.Messages.Add(new Promt.YaMessage(message));

            JsonContent content = JsonContent.Create(promt);

            var responseMessage = await client.PostAsync(apiUrl, content);

            var response = await responseMessage.Content.ReadFromJsonAsync<YaResponse>();

            return response.Result.Alternatives.FirstOrDefault().Message.Text;
        }
    }
}
