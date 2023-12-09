using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace TelegramBot
{
    internal class Program
    {
        static ITelegramBotClient bot;
        static YaGPT yandexGPT;

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(JsonSerializer.Serialize(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
                    return;
                }
                else
                {
                    var response = await yandexGPT.SendRequest(message.Text.ToLower());

                    await botClient.SendTextMessageAsync(message.Chat, response);
                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(JsonSerializer.Serialize(exception));
        }

        static async Task Main(string[] args)
        {
            try
            {
                IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

                yandexGPT = new YaGPT(Configuration);

                var token = Configuration.GetValue<string>("BotToken");

                bot = new TelegramBotClient(token);
                Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

                var cts = new CancellationTokenSource();
                var cancellationToken = cts.Token;
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = { }, // receive all update types
                };
                
                bot.StartReceiving(
                    HandleUpdateAsync,
                    HandleErrorAsync,
                    receiverOptions,
                    cancellationToken
                );

                await Task.Run(() =>
                {
                    for(;;){
                        yandexGPT.UpdateIAMToken();

                        Thread.Sleep(TimeSpan.FromHours(8));
                    }
                });

            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex );
            }
        }
    }
}