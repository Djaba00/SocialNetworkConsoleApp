using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace SocialNetwork.BLL.Services
{
    public class UserService
    {
        public IUserRepository userRepository;
        IFriendRepository friendRepository;
        MessageService messageService;
        public UserService()
        {
            this.userRepository = new UserRepository();
            this.messageService = new MessageService();
            this.friendRepository = new FriendRepository();
        }

        public void Register(UserRegistrationData userRegistrationData)
        {
            if (String.IsNullOrEmpty(userRegistrationData.FirstName))
                throw new ArgumentNullException();

            if (String.IsNullOrEmpty(userRegistrationData.LastName))
                throw new ArgumentNullException();

            if(String.IsNullOrEmpty(userRegistrationData.Email))    
                throw new ArgumentNullException();  

            if(String.IsNullOrEmpty(userRegistrationData.Password))
                throw new ArgumentNullException();

            if(userRegistrationData.Password.Length < 8)
                throw new ArgumentOutOfRangeException();

            if(!new EmailAddressAttribute().IsValid(userRegistrationData.Email))
                throw new ArgumentException();

            if(userRepository.FindByEmail(userRegistrationData.Email) != null)
                throw new ArgumentException();

            var userEntity = new UserEntity()
            {
                firstname = userRegistrationData.FirstName,
                lastname = userRegistrationData.LastName,
                password = userRegistrationData.Password,
                email = userRegistrationData.Email
            };

            if (this.userRepository.Create(userEntity) == 0)
                throw new Exception();
        }

        public User Authenticate(UserAuthenticationData userAuthenticationData)
        {
            var findUserEntity = userRepository.FindByEmail(userAuthenticationData.Email);

            if(findUserEntity == null)
                throw new UserNotFoundException();

            if (findUserEntity.password != userAuthenticationData.Password)
                throw new WrongPasswordException();

            return ConstructUserModel(findUserEntity);
        }

        public void AddFriend(UserAddingFriendData userAddingFriendData)
        {
            var findFriendEntity = userRepository.FindByEmail(userAddingFriendData.FriendEmail);
            if (findFriendEntity is null)
                throw new UserNotFoundException();

            var friendEntity = new FriendEntity()
            {
                user_id = userAddingFriendData.UserId,
                friend_id = findFriendEntity.id
            };

            if(this.friendRepository.Create(friendEntity) == 0)
                throw new Exception();
        }

        public IEnumerable<User> GetFriendsByUserId(int userId)
        {
            return friendRepository.FindAllByUserId(userId).Select(f => FindById(f.friend_id));
        }

        public User FindByEmail(string email)
        {
            var findUserEntity = userRepository.FindByEmail(email);

            if(findUserEntity is null)
                throw new UserNotFoundException();

            return ConstructUserModel(findUserEntity);
        }

        public User FindById(int userId)
        {
            var findUserEntity = userRepository.FindById(userId);

            if(findUserEntity is null)
                throw new UserNotFoundException();

            return ConstructUserModel(findUserEntity);
        }

        public void Update(User user)
        {
            var updatableUserEntity = new UserEntity()
            {
                id = user.Id,
                firstname = user.FirstName,
                lastname = user.LastName,
                password = user.Password,
                email = user.Email,
                photo = user.Photo,
                favorite_movie = user.FavoriteMovie,
                favorite_book = user.FavoriteBook
            };

            if (this.userRepository.Update(updatableUserEntity) == 0)
                throw new Exception();
        }

        private User ConstructUserModel(UserEntity userEntity)
        {
            var incomingMessages = this.messageService.GetIncomingMessagesByUserId(userEntity.id);
            var outgoingMessages = this.messageService.GetOutgoingMessagesByUserId(userEntity.id);
            var friends = GetFriendsByUserId(userEntity.id);

            return new User(userEntity.id,
                          userEntity.firstname,
                          userEntity.lastname,
                          userEntity.password,
                          userEntity.email,
                          userEntity.photo,
                          userEntity.favorite_movie,
                          userEntity.favorite_book,
                          incomingMessages,
                          outgoingMessages,
                          friends
                          );
        }
    }
}
