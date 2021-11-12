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
        List<ServerAreaReport> GetAllReports();
        ObjectInArea GetObject(int Id);
        ObjectInArea AddObject(ObjectInArea area);
        void RemoveObject(int Id);
        void UpdateObject(ObjectInArea area);

        ServerAreaToCheck AddArea(ServerAreaToCheck obj);
        void EditArea(ServerAreaToCheck obj);
        void RemoveArea(int areaId);

        void EditUser(User user);
        void RemoveReport(int id);
        void RemoveUser(int Id);
        User AddUser(User user);
    }
}
