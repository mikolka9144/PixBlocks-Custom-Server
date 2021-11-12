using System;
using System.Collections.Generic;
using Pix_API.Base.Utills;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Security;

namespace Pix_API.ChecklistReviewerApp.MainServer
{
    public partial class ClientAppCommands : ICommandRepository
    {
        /// <summary>
        /// Adds the area.
        /// </summary>
        /// <returns>The Id of added Area.</returns>
        public int AddArea(string token,[ FromBody] ServerAreaToCheck obj)
        {
            ThrowIsUserIsntAdmin(token);
            var ID = areaProvider.AddArea(obj);
            return ID;
        }

        public int AddObject(string token,[FromBody] ObjectInArea area)
        {
            ThrowIsUserIsntAdmin(token);
            var ID = objectsProvider.AddObject(area);
            return ID;
        }

        public void EditReport(string token,[FromBody] ServerAreaToCheck obj)
        {
            ThrowIsUserIsntAdmin(token);
            areaProvider.EditArea(obj);
        }

        public List<ServerAreaToCheck> GetAllAreasToCheck(string token)
        {
            ThrowIsUserIsntAdmin(token);
            return areaProvider.GetAllAreas();
        }

        public ObjectInArea GetObject(string token,int Id)
        {
            ThrowIsUserIsntAdmin(token);
            return objectsProvider.GetObject(Id);
        }

        public void RemoveObject(string token, int Id)
        {
            ThrowIsUserIsntAdmin(token);
            objectsProvider.RemoveObject(Id);
        }

        public void RemoveArea(string token,int areaId)
        {
            ThrowIsUserIsntAdmin(token);
            areaProvider.RemoveArea(areaId);
        }

        public void UpdateObject(string token,[FromBody] ObjectInArea obj)
        {
            ThrowIsUserIsntAdmin(token);
            objectsProvider.UpdateObject(obj);
        }
        public bool IsAdmin(string token)
        {
            var userId = tokenProvider.GetUserForToken(token);
            var user = usersProvider.GetUser(userId);
            return user.IsAdmin;
        }

        public List<User> GetAllUsers(string token)
        {
            ThrowIsUserIsntAdmin(token);
            return usersProvider.GetAllUsers();
        }
        public void EditUser(string token,[FromBody] User user)
        {
            ThrowIsUserIsntAdmin(token);
            usersProvider.UpdateUser(user);
        }

        public void RemoveUser(string token,int userId)
        {
            ThrowIsUserIsntAdmin(token);
            usersProvider.RemoveUser(userId);
        }

        public int AddUser(string token,[FromBody]User user)
        {
            ThrowIsUserIsntAdmin(token);
            usersProvider.AddUser(user);
            return user.Id;
        }
        public List<ServerAreaReport> GetAllReports(string token)
        {
            ThrowIsUserIsntAdmin(token);
            return reportsProvider.GetAllReports();
        }
        public void RemoveReport(string token, int Id)
        {
            ThrowIsUserIsntAdmin(token);
            reportsProvider.RemoveReport(Id);
        }
        private void ThrowIsUserIsntAdmin(string token)
        {
            if (!IsAdmin(token)) throw new SecurityException("Non-admin user called a Admin olny command");
        }
    }
}
