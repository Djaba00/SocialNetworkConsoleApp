using System;
using SocialNetwork.BLL.Services;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using SocialNetwork.DAL.Repositories;
using System.Net.Http.Headers;
using Castle.Components.DictionaryAdapter.Xml;

namespace SocialNetwork.Tests
{
    public class UserServiceTest
    {   
        List<UserEntity> usersDB = new List<UserEntity>()
            {
                new UserEntity()
                {
                    id = 1,
                    firstname = "Timofey",
                    lastname = "Shipelenko",
                    password = "10101010",
                    email = "shipelenko@gmail.com"
                }
            };

        List<User> users = new List<User>()
        {
            new User(1, "Timofey", "Shipelenko", "10101010", "shipelenko9@gmail.com", "-", "-", "-", It.IsAny<IEnumerable<Message>>(),
                It.IsAny<IEnumerable<Message>>(), It.IsAny<IEnumerable<User>>())
        };

        [Fact]
        public void AuthenticateMustReturnTrueValue()
        {
            var mockUR = new Mock<IUserRepository>();

            var userAutData = new UserAuthenticationData()
            {
                Email = "shipelenko@gmail.com",
                Password = "10101010"
            };

            mockUR.Setup(ur => ur.FindByEmail(userAutData.Email)).Returns(usersDB.First(u => u.email == userAutData.Email));

            var userService = new UserService() { userRepository = mockUR.Object };

            var login = userService.Authenticate(userAutData);

            Assert.Contains(login, users);
            
        }
    }
}
