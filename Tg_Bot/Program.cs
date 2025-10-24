using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Tg_Bot;
using Tg_Bot.Services;

class Program
{
    static async Task Main()
    {
        // Инициализация БД
        var dbContext = new ApplicationContext();

        // Инициализация сервисов
        var botToken = ("8266835873:AAE2aJOS9nLZ3iRNs3uZG_v3q4oGATWzk7o");
        var groupId = -1002491503978;
        var _bot = new TelegramBotClient("8266835873:AAE2aJOS9nLZ3iRNs3uZG_v3q4oGATWzk7o");
        var botClient = new TelegramBotClient(botToken);
        var userService = new UserService(dbContext);
        var botHandler = new TelegramBotHandler(botToken, userService);
        var bot = new TelegramBotClient(botToken);

        botHandler.StartReceiving();
        await userService.SendWelcomeToGroup(bot, groupId);

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // Получаем все типы updates
        };

        await UiService.Menu();
    }
}

