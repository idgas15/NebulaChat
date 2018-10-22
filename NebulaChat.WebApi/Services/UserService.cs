using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using NebulaChat.Core.Dto;
using NebulaChat.Core.Models;
using NebulaChat.Data;
using NebulaChat.WebApi.Helpers;
using NebulaChat.WebApi.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NebulaChat.WebApi.Services
{
    public class UserService : IUserService
    {
        private NebulaChatDbContext chatContext;
        private readonly IHubContext<ChatHub> hubContext;
        private readonly IMapper mapper;

        public UserService(NebulaChatDbContext chatContext, IHubContext<ChatHub> hubContext, IMapper mapper)
        {
            this.chatContext = chatContext;
            this.hubContext = hubContext;
            this.mapper = mapper;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = chatContext.Users.SingleOrDefault(x => x.Username == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return chatContext.Users;
        }

        public User GetById(int id)
        {
            return chatContext.Users.Find(id);
        }

        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (chatContext.Users.Any(x => x.Username == user.Username))
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.Email = user.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            chatContext.Users.Add(user);
            var saved = chatContext.SaveChanges() > 0;

            if (saved)
            {
                var users = GetAll();

                var theUsers = mapper.Map<IEnumerable<ChatUserDto>>(users);

                hubContext.Clients.All.SendAsync("UsersRefresh", users);
            }
            return user;
        }

        public void Update(User userParam, string password = null)
        {
            var user = chatContext.Users.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (chatContext.Users.Any(x => x.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");
            }

            // update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            chatContext.Users.Update(user);
            chatContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = chatContext.Users.Find(id);
            if (user != null)
            {
                chatContext.Users.Remove(user);
                chatContext.SaveChanges();
            }
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        
    }
}
