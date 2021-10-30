using System.Linq;
using Pix_API.Base.Disk;
using Pix_API.Base.Utills;
using Pix_API.ChecklistReviewerApp.Interfaces;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.Disk
{
    public class FileUsersProvider:SinglePoolStorageProvider<User>,IUsersProvider 
    {
        private readonly IdAssigner saver;

        public FileUsersProvider(DataSaver<User> saver, ILastIndexSaver index_saver) : base(saver)
        {
            this.saver = new IdAssigner(index_saver);
        }

        public void AddUser(User user)
        {
            user.Id = saver.NextEmptyId;
            AddSingleObject(user, user.Id);
        }

        public User GetUser(string login) => storage.FirstOrDefault(s => s.login == login);

        public User GetUser(int Id) => GetSingleObject(Id);

        public void RemoveIdFromUser(int UserId, int AreaId)
        {
            var user = storage.First(s => s.Id == UserId);
            user.areasToCheckIds.Remove(AreaId);
            AddOrUpdateSingleObject(user, user.Id);
        }

        public void UpdateUser(User user)
        {
            AddOrUpdateSingleObject(user, user.Id);
        }
    }
}
