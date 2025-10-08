using Microsoft.EntityFrameworkCore;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Tg_Bot.Models;


namespace Tg_Bot
{
    public class Host
    {

        public Action<ITelegramBotClient, Update>? OnMessage;
        private readonly TelegramBotClient _bot;

        public Host(string token)
        {
            _bot = new TelegramBotClient(token);
        }

        public void Start()
        {

            _bot.StartReceiving(
                updateHandler: HandleUpdateAsync,
                errorHandler: HandleErrorAsync
            );

            Console.WriteLine("Бот успешно запущен!");
        }

        private async Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine($"Ошибка: {exception.Message}");
            await Task.CompletedTask;
        }


        private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { Text: { } messageText } message)
                return;

            var chatId = message.Chat.Id;
            var userName = message.From?.FirstName ?? "Аноним";
            Console.WriteLine($"id: {chatId} \n{DateTime.Now} \nUserName {userName}: {messageText}");
            OnMessage?.Invoke(client, update);
            await Task.CompletedTask;
        }
                 }
}