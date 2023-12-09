using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot.Requests;
using TelegramBot.Promt;
using TelegramBot.Response;

namespace TelegramBot
{
    internal class YaGPT
    {
        HttpClient client = new HttpClient();
        IConfiguration configuration;

        private string iamToken;
        private string yandexFolderId;
        private string yandexOAuth;

        const string apiUrl = "https://llm.api.cloud.yandex.net/foundationModels/v1/completion";

        public YaGPT(IConfiguration configuration)
        {
            iamToken = configuration.GetValue<string>("IamToken");
            yandexFolderId = configuration.GetValue<string>("YandexFolderId");
            yandexOAuth = configuration.GetValue<string>("YandexOAuth");
            this.configuration = configuration;
        }

        public async Task<string> SendRequest(string message)
        {
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

        public async Task UpdateIAMToken()
        {
            var apiTokenUrl = "https://iam.api.cloud.yandex.net/iam/v1/tokens";
            
            YaIAMTokenRequest tokenRequest = new YaIAMTokenRequest();
            tokenRequest.YandexPassportOauthToken = yandexOAuth;

            JsonContent content = JsonContent.Create(tokenRequest);
            var responseMessage = await client.PostAsync(apiTokenUrl, content);

            var response = await responseMessage.Content.ReadFromJsonAsync<YaIAMTokenResponse>();

            iamToken = response.IAMToken;

            UpdateAppSetting("IamToken", iamToken);
        }

        public void UpdateAppSetting(string key, string value)
        {
            var configJson = File.ReadAllText("appsettings.json");
            var config = JsonSerializer.Deserialize<Dictionary<string, object>>(configJson);
            config[key] = value;
            var updatedConfigJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("appsettings.json", updatedConfigJson);
        }
    }
}
