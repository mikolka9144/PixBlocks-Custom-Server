using System;
using System.Collections.Generic;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace AdministrationApp
{
    public interface IAPIClient
    {
        bool LoginToServer(string text, string password);
        bool IsServerOnline();

        List<User> GetAllUsers();
        List<ServerAreaToCheck> GetAllAreasToCheck();

        ObjectInArea GetObject(int Id);
        ObjectInArea AddObject(ObjectInArea area);
        void RemoveObject(int Id);
        void UpdateObject(ObjectInArea area);

        ServerAreaToCheck AddArea(ServerAreaToCheck obj);
        void EditReport(ServerAreaToCheck obj);
        void RemoveReport(int areaId);

        void EditUser(User user);
        void RemoveUser(User user);
        User AddUser(User user);
    }
}
