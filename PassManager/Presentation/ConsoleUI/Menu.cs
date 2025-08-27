using Microsoft.Extensions.DependencyInjection;
using PassManager.Application.Services;
using PassManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassManager.Presentation.ConsoleUI
{
    public class Menu
    {
        public static async Task RunAsync(IServiceProvider sp)
        {
            var userService = sp.GetRequiredService<UserService>();
            var passService = sp.GetRequiredService<PasswordService>();

            // ابتدا: ثبت یا ورود کاربر
            if (!await userService.AnyUserAsync())
            {
                Console.WriteLine("کاربری برای برنامه تعریف نشده. یک حساب مدیر ایجاد کنید.");
                // گرفتن username/password با ConsoleUtils.ReadPassword()
                await userService.CreateUserAsync(username, password);
                Console.WriteLine("کاربر ایجاد شد. لطفا وارد شوید.");
            }

            // Login loop
            User logged = null;
            byte[] sessionKey = null;
            while (logged == null)
            {
                Console.Write("Username: ");
                var u = Console.ReadLine();
                var p = ConsoleUtils.ReadPassword("Password: ");
                var (ok, user) = await userService.AuthenticateAsync(u, p);
                if (ok)
                {
                    logged = user;
                    sessionKey = userService.DeriveAesKeyForSession(p, user.KeySalt);
                }
                else Console.WriteLine("نام کاربری یا رمز اشتباه است.");
            }

            // Main menu loop
            while (true)
            {
                Console.Clear();
                Console.WriteLine("PassManager - Main Menu");
                Console.WriteLine("1) Add  2) List  3) View  4) Edit  5) Delete  6) Generate  7) Export  8) Change master password  0) Exit");
                var k = Console.ReadKey(true).KeyChar;
                switch (k)
                {
                    case '1':
                        await AddFlow(passService, sessionKey);
                        break;
                    case '2':
                        await ListFlow(passService);
                        break;
                    case '3':
                        await ViewFlow(passService, sessionKey);
                        break;
                    // ... بقیه case ها
                    case '0':
                        return;
                }
            }
        }
    }
}
