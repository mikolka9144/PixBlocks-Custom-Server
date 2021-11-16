using System;
using System.Collections.Generic;
using Pix_API.Base.Utills;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Security;
using AdministrationApp.Models;

namespace Pix_API.ChecklistReviewerApp.MainServer
{
    public partial class ClientAppCommands : ICommandRepository
    {
        /// <summary>
        /// Adds the area.
        /// </summary>
        /// <returns>The Id of added Area.</returns>
        public ServerAreaToCheck AddArea(string token,[ FromBody] AdminAreaToCheck obj)
        {
            ThrowIsUserIsntAdmin(token);
            var serverArea = obj.ConvertToServer(imageManager,null);
            areaProvider.AddArea(serverArea);
            return serverArea;
        }

        public ServerObjectInArea AddObject(string token,[FromBody] ClientObjectInArea area)
        {
            ThrowIsUserIsntAdmin(token);
            var serverObject = area.ConvertToServer(imageManager.UploadBase64(area.image));
            var ID = objectsProvider.AddObject(serverObject);
            serverObject.Id = ID;
            return serverObject;
        }

        public ServerAreaToCheck EditArea(string token,[FromBody] AdminAreaToCheck obj)
        {
            ThrowIsUserIsntAdmin(token);
            var currentServerArea = areaProvider.GetArea(obj.Id);
            var serverArea = obj.ConvertToServer(imageManager,currentServerArea.imageId);
            areaProvider.EditArea(serverArea);
            return serverArea;
        }

        public List<ServerAreaToCheck> GetAllAreasToCheck(string token)
        {
            ThrowIsUserIsntAdmin(token);
            return areaProvider.GetAllAreas();
        }

        public ServerObjectInArea GetObject(string token,int Id)
        {
            ThrowIsUserIsntAdmin(token);
            return objectsProvider.GetObject(Id);
        }

        public void RemoveObject(string token, int Id)
        {
            ThrowIsUserIsntAdmin(token);
            imageManager.RemoveImage(objectsProvider.GetObject(Id).ImageId);
            objectsProvider.RemoveObject(Id);
        }

        public void RemoveArea(string token,int areaId)
        {
            ThrowIsUserIsntAdmin(token);
            imageManager.RemoveImage(areaProvider.GetArea(areaId).imageId);
            areaProvider.RemoveArea(areaId);
        }

        public ServerObjectInArea UpdateObject(string token,[FromBody] ClientObjectInArea obj)
        {
            ThrowIsUserIsntAdmin(token);
            var serverObj = objectsProvider.GetObject(obj.Id);
            imageManager.EditImage(serverObj.ImageId, obj.image);
            objectsProvider.UpdateObject(obj.ConvertToServer(serverObj.ImageId));
            return serverObj;
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
            var report = reportsProvider.GetReport(Id);
            foreach (var item in report.Objects)
            {
                imageManager.RemoveImage(item.ImageId);
            }
            reportsProvider.RemoveReport(Id);
        }
        public string GetImage(string token,int ImageId)
        {
            ThrowIsUserIsntAdmin(token);
            return imageManager.GetBase64Image(ImageId);
        }
        private void ThrowIsUserIsntAdmin(string token)
        {
            if (!IsAdmin(token)) throw new SecurityException("Non-admin user called a Admin olny command");
        }
    }
}
