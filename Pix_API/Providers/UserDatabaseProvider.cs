using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using System.Linq;
using Pix_API.Providers.ContainersProviders;

namespace Pix_API.Providers
{
    public class UserDatabaseProvider:IUserDatabaseProvider
    {
        private readonly DataSaver<User> saver;
        private List<User> users = new List<User>();
        public UserDatabaseProvider(DataSaver<User>saver)
        {
            var id_users = saver.LoadAll();
            users = id_users.Select((arg) => arg.Obj).ToList();
            this.saver = saver;
        }
        public void AddUser(User user)
        {
            user.SetupUser(users.Count);
            users.Add(user);
            saver.SaveInBackground(new IdObjectBinder<User>(user.Id.Value, user));
        }

        public bool ContainsUserWithEmail(string email)
        {
            return users.Any(s => s.Email == email);
        }

        public User GetUser(string email)
        {
            return users.Find(s => s.Email == email);
        }

        public User GetUser(int Id)
        {
            return users.Find(s => s.Id == Id);
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

    }

    public interface IUserDatabaseProvider
    {
        User GetUser(string email);
        User GetUser(int Id);
        void AddUser(User user);
        void UpdateUser(User user);
        bool ContainsUserWithEmail(string email);
    }
}
