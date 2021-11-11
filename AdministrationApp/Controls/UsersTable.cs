using System;
using Xwt;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
namespace AdministrationApp.Controls
{
    public class UsersTable:ObjectsList<User>
    {
        public void AddUser(User user)
        {
            AddObject(user, user.login);
        }
        public void RemoveUser(User user)
        {
            RemoveObject(s => s.Id == user.Id);
        }
        public void UpdateUser(User user)
        {
            EditObject(user, user.login, s => s.Id == user.Id);
        }
    }
}
