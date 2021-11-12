using System.Collections.Generic;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.Interfaces
{
    public interface IUsersProvider
    {
        User GetUser(string login);
        User GetUser(int Id);
        void AddUser(User user);
        void RemoveIdFromUser(int UserId, int AreaId);
        void UpdateUser(User user);
        List<User> GetAllUsers();
        void RemoveUser(int Id);
    }
}
