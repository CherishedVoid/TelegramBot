using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class TelegramBotHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly UserService _userService;

    public TelegramBotHandler(string token, UserService userService)
    {
        _botClient = new TelegramBotClient(token);
        _userService = userService;
    }

    public void StartReceiving()
    {
        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            errorHandler: HandleErrorAsync
        );
        Console.WriteLine("Бот запущен и ожидает сообщений...");
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { From: { } user, Text: { } messageText })
            return;
        int userId = (int)user.Id;
        string userName = user.Username ?? "Без username";
        string realName = $"{user.FirstName} {user.LastName}".Trim();

        await _userService.SaveUserAsync(userId, userName, realName);


        if (messageText == "/help" || messageText == "/help@TgAssistantGuildBot")
        {
            await _botClient.SendMessage(
        chatId: update.Message.Chat.Id,
        text: "Справка о боте:\n/start - предоставляет все данные пользователя телеграмма (нужно нажать хотя бы раз)\n/teg - упоминает всех участников чата");
        }

        if (messageText == "/teg" || messageText == "/teg@TgAssistantGuildBot")
        {
            await _botClient.SendMessage(
        chatId: update.Message.Chat.Id,
        text: "Начинаю...\n");
        }

        else
        {
            await _botClient.SendMessage(
     chatId: update.Message.Chat.Id,
     text: "Спасибо за предоставленные данные\nУдачного дня!");
        }
    }
    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ошибка в боте: {exception.Message}");
        return Task.CompletedTask;
    }
    
    }
    