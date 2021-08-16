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
        public UserDatabaseProvider(DataSaver<User>saver):base(saver)
        {
        }
        public void AddUser(User user)
        {
            user.SetupUser(storage.Count);
            AddSingleObject(user, user.Id.Value);
        }

        public bool ContainsUserWithEmail(string email)
        {
            return storage.Any(s => s.Obj.Email == email);
        }

        public bool ContainsUserWithLogin(string login)
        {
            return storage.Any(s => s.Obj.Student_login == login);
        }

        public List<User> GetAllUsersBelongingToClass(int classId)
        {
            return storage.FindAll((arg) => arg.Obj.Student_studentsClassId == classId).Select(s => s.Obj).ToList();
        }

        public User GetUser(string EmailOrLogin)
        {
            return storage.Find(s => s.Obj.Email == EmailOrLogin || s.Obj.Student_login == EmailOrLogin)?.Obj;
        }

        public User GetUser(int Id)
        {
            return GetSingleObjectOrCreateNew(Id);
        }

        public void UpdateUser(User user)
        {
            AddOrUpdateSingleObject(user, user.Id.Value);
        }

    }


}
