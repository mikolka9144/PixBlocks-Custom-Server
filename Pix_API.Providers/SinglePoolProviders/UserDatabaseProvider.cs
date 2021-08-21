using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using System.Linq;
using Pix_API.Providers.ContainersProviders;
using Pix_API.Providers.BaseClasses;
using Pix_API.Interfaces;

namespace Pix_API.Providers
{
    public class UserDatabaseProvider:SinglePoolStorageProvider<User>,IUserDatabaseProvider
    {
        private IdAssigner idAssigner;

        public UserDatabaseProvider(DataSaver<User>saver):base(saver)
        {
            idAssigner = new IdAssigner(storage.Count());
        }
        public void AddUser(User user)
        {
            user.Id = idAssigner.NextEmptyId;
            AddSingleObject(user, user.Id.Value);
        }

        public bool ContainsUserWithEmail(string email)
        {
            return storage.Any(s => s.Email == email);
        }

        public bool ContainsUserWithLogin(string login)
        {
            return storage.Any(s => s.Student_login == login);
        }

        public List<User> GetAllUsersBelongingToClass(int classId)
        {
            return storage.Where((arg) => arg.Student_studentsClassId == classId).ToList();
        }

        public User GetUser(string EmailOrLogin)
        {
            return storage.FirstOrDefault(s => s.Email == EmailOrLogin || s.Student_login == EmailOrLogin);
        }

        public User GetUser(int Id)
        {
            return GetSingleObject(Id);
        }

        public void UpdateUser(User user)
        {
            AddOrUpdateSingleObject(user, user.Id.Value);
        }

    }


}
