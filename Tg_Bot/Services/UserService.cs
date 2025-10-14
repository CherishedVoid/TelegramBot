using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Tg_Bot;
using Tg_Bot.Models;


public class UserService
{
    private readonly ApplicationContext _dbContext;
    private readonly ITelegramBotClient _botClient;
    public UserService(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveUserAsync(int userId, string userName, string realName)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

        if (existingUser == null)
        {
            _dbContext.Users.Add(new Users
            {
                UserId = userId,
                UserName = userName,
                Nickname = realName

            });

            await _dbContext.SaveChangesAsync();
            Console.WriteLine($"Пользователь {realName} {userId} сохранён в БД.");
        }
        else
        {
            Console.WriteLine($"Пользователь {userName} {userId} уже есть в БД.");
        }
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
    // Обновленный метод для получения всех username/nickname из базы данных
    public async Task<List<string>> GetAllUsernamesAsync()
    {
        return await _dbContext.Users
            .Select(u => u.UserName == "Без username" ? u.Nickname : u.UserName)
            .Where(name => !string.IsNullOrEmpty(name))
            .ToListAsync();
    }

    // Метод для форматирования списка username для Telegram
    public string FormatUsernamesForTelegram(List<string> usernames)
    {
        if (usernames == null || !usernames.Any())
            return "В базе данных нет пользователей";
        
            return "Упоминания пользователей:\n" + string.Join("\n", usernames.Select(username => $"@{username}"));
    }
}