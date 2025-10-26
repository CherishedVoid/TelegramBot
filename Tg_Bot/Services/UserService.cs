using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Tg_Bot;
using Tg_Bot.Models;
public class UserService
{
    private readonly ApplicationContext _dbContext;
    public UserService(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task SaveUserAsync(int userId, string userName, string realName)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

        if (existingUser == null)
        {
            // Добавляем нового пользователя
            _dbContext.Users.Add(new Users
            {
                UserId = userId,
                UserName = userName,
                Nickname = realName
            });
            Console.WriteLine($"Пользователь {realName} {userId} сохранён в БД.");
        }
        else
        {
            existingUser.UserName = userName;
            existingUser.Nickname = realName;
            Console.WriteLine($"Данные пользователя {realName} {userId} обновлены в БД.");
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdatingUsersAsync()
    {
        // Логика получения данных пользователей для консоли
        var users = await _dbContext.Users.ToListAsync();
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.UserId}, Nickname: {user.Nickname}, Username: {user.UserName}");
        }
    }

    public async Task SendWelcomeToGroup(ITelegramBotClient botClient, long groupId)
    {
        var message = "👋 Приветствуем в группе!\n\n" +
                     "Команды:\n" +
                     "/help - справка\n" +
                     "/start - начало работы\n" +
                     "/teg - упомянуть участников";
        await botClient.SendMessage(groupId, message);
    }

    // Метод возвращает список пользователей
    public async Task<List<Users>> GetAllUsersAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    // Метод для форматирования упоминаний с HTML-ссылками
    public string FormatUsersForTelegram(List<Users> users)
    {
        if (users == null || !users.Any())
            return "В базе данных нет пользователей";

        var mentions = new List<string>();

        foreach (var user in users)
        {
            string userName = user.UserName ?? "Без username";
            string nickName = user.Nickname ?? "Без имени";

            if (!string.IsNullOrEmpty(userName) && userName != "Без username")
            {
                mentions.Add($"@{userName}");
            }
            //  HTML-ссылка по ID, если отсутствует username
            else
            {
                string displayName = string.IsNullOrEmpty(nickName) ? "Пользователь" : nickName;
                string mentionLink = $"<a href=\"tg://user?id={user.UserId}\">{displayName}</a>";
                mentions.Add(mentionLink);
            }
        }
        return "Упоминания пользователей:\n" + string.Join("\n", mentions);
    }
     public static async Task SaveUserManually(UserService userService)
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