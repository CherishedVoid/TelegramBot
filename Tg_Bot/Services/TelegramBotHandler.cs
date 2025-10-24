using System;
using Telegram.Bot;
using Telegram.Bot.Types;

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

    private async Task<bool> IsUserAdminAsync(ITelegramBotClient botClient, long chatId, long userId)
    {
        try
        {
            // Получаем всех администраторов чата
            var administrators = await botClient.GetChatAdministrators(chatId);

            // Проверяем, есть ли пользователь среди администраторов
            return administrators.Any(admin => admin.User.Id == userId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка проверки прав администратора: {ex.Message}");
            return false;
        }
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { From: { } user, Text: { } messageText, Chat: { } chat })
            return;

        int userId = (int)user.Id;
        string userName = user.Username ?? "Без username";
        string realName = $"{user.FirstName} {user.LastName}".Trim();

        await _userService.SaveUserAsync(userId, userName, realName);

        if (messageText == "/help" || messageText == "/help@TgAssistantGuildBot")
        {
            await _botClient.SendMessage(
                chatId: chat.Id,
                text: "Справка о боте:\n/start - предоставляет все данные пользователя телеграмма (нужно нажать хотя бы раз).\nПри изменении данных в телеграмм, требуется повторное взаимодействие.\n/teg - упоминает всех участников чата (только для администраторов).");
        }
        else if (messageText == "/teg" || messageText == "/teg@TgAssistantGuildBot")
        {
            // Проверяем, является ли пользователь администратором только для команды /teg
            bool isAdmin = await IsUserAdminAsync(botClient, chat.Id, user.Id);

            if (!isAdmin)
            {
                await _botClient.SendMessage(
                    chatId: chat.Id,
                    text: "❌ Команда /teg доступна только администраторам группы.");
                return;
            }

            try
            {
                // Получаем все username из базы данных
                var usernames = await _userService.GetAllUsernamesAsync();

                // Форматируем сообщение
                string message = _userService.FormatUsernamesForTelegram(usernames);

                await _botClient.SendMessage(
                    chatId: chat.Id,
                    text: message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении /teg: {ex.Message}");
                await _botClient.SendMessage(
                    chatId: chat.Id,
                    text: "Произошла ошибка при получении данных пользователей");
            }
        }
        // Добавьте другие команды здесь, которые доступны всем пользователям
        else if (messageText == "/start" || messageText == "/start@TgAssistantGuildBot")
        {
            await _botClient.SendMessage(
                chatId: chat.Id,
                text: $"Добро пожаловать! Ваши данные:\nID: {userId}\nUsername: {userName}\nИмя: {realName}\n\nИспользуйте /help для списка команд.");
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ошибка в боте: {exception.Message}");
        return Task.CompletedTask;
    }
}