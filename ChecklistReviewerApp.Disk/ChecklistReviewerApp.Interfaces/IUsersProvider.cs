using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.Interfaces
{
    public interface IUsersProvider
    {
        User GetUser(string login);
        User GetUser(int Id);
        void AddUser(User user);
        //void AddAreaIdToUser(int User_Id);
        void RemoveIdFromUser(int UserId, int AreaId);
        void UpdateUser(User user);
    }
}
