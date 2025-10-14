using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Tg_Bot;

class Program
{
    static async Task Main()
    {

        // Инициализация БД
        var dbContext = new ApplicationContext();

        // Инициализация сервисов
        var botToken = ("8266835873:AAE2aJOS9nLZ3iRNs3uZG_v3q4oGATWzk7o");
        var groupId = -4855178515;
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

        using var cts = new CancellationTokenSource();

        Console.WriteLine("Бот готов к работе!");

        // Консольный интерфейс
        while (true)
        {
            Console.WriteLine("\nВыберите операцию:");
            Console.WriteLine("1 - Получение данных пользователей");
            Console.WriteLine("2 - Сохранить пользователя (вручную)");
            Console.WriteLine("3 - Найти пользователя по ID");
            Console.WriteLine("0 - Выйти\n");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await userService.UpdatingUsersAsync();
                    break;
                case "2":
                    await SaveUserManually(userService);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Некорректный выбор");
                    break;
            }
        }
    }
    private static async Task SaveUserManually(UserService userService)
    {
        Console.WriteLine("Введите ID пользователя:");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Некорректный ID!");
            return;
        }

        Console.WriteLine("Введите username:");
        string userName = Console.ReadLine() ?? "Без username";

        Console.WriteLine("Введите реальное имя:");
        string realName = Console.ReadLine() ?? "Без имени";

        await userService.SaveUserAsync(userId, userName, realName);
    }
    public async Task HandleUpdateAsync(ITelegramBotClient client, Telegram.Bot.Types.Update update, CancellationToken cancellationToken, long groupId)
    {
        if (update.Message is not { Text: { } messageText } message)
            return;
    }


}

