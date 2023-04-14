using System;
using SocialNetwork.BLL.Services;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using Xunit;
using Moq;


namespace SocialNetwork.Tests
{
    public class UserServiceTest
    {
        [Fact]
        public void AuthenticateMustReturnUserModel()
        {
            // Arrage
            var mockUserRepository = new Mock<IUserRepository>();
            var userService = new UserService() { userRepository = mockUserRepository.Object };

            var userAutData = new UserAuthenticationData()
            {
                Email = "shipelenko@gmail.com",
                Password = "10101010"
            };

            UserEntity userEntity = new UserEntity()
            {
                id = 1,
                firstname = "Timofey",
                lastname = "Shipelenko",
                password = "10101010",
                email = "shipelenko@gmail.com"
            };

            mockUserRepository.Setup(u => u.FindByEmail(userAutData.Email)).Returns(userEntity);

            //Act
            var result =  userService.Authenticate(userAutData);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(userAutData.Email, result.Email);
            Assert.Equal(userAutData.Password, result.Password);
        }
    }
}
