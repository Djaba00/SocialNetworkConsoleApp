using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.PLL.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.PLL.Views
{
    public class AuthenticationView
    {
        UserService userService;

        public AuthenticationView(UserService userService)
        {
            this.userService = userService;
        }

        public void Show() 
        {
            var autenticationData = new UserAuthenticationData();

            Console.Write("Введите Email: ");
            autenticationData.Email = Console.ReadLine();

            Console.Write("Введите пароль: ");
            autenticationData.Password = Console.ReadLine();

            try
            {
                var user = this.userService.Authenticate(autenticationData);

                SuccessMessage.Show("Вы успешно вошли в социальную сеть!");
                SuccessMessage.Show($"Добро пожаловать {user.FirstName}");

                Program.userMenuView.Show(user);
            }
            catch(WrongPasswordException)
            {
                AlertMessage.Show("Неверный пароль!");
            }
            catch(UserNotFoundException)
            {
                AlertMessage.Show("Пользователь с таким email не найден!");
            }
        }
    }
}
