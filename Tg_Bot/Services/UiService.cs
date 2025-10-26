using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Tg_Bot;

namespace Tg_Bot.Services
{
    internal class UiService
    {
        private static object userService;
        public static async Task Menu()
        {
            var dbContext = new ApplicationContext();
            var userService = new UserService(dbContext);

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
                        await UserService.SaveUserManually(userService);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Некорректный выбор");
                        break;
                }
            }
        }
    }
}


