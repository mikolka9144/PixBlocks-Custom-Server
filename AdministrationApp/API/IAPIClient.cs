using System;
using System.Collections.Generic;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using AdministrationApp.Models;

namespace AdministrationApp
{
    public interface IAPIClient
    {
        bool LoginToServer(string text, string password);
        bool IsServerOnline();
        string GetImage(int ImageId);

        List<User> GetAllUsers();
        List<ServerAreaToCheck> GetAllAreasToCheck();
        List<ServerAreaReport> GetAllReports();
        ServerObjectInArea GetObject(int Id);
        ServerObjectInArea AddObject(ClientObjectInArea area);
        void RemoveObject(int Id);
        ServerObjectInArea UpdateObject(ClientObjectInArea area);

        ServerAreaToCheck AddArea(AdminAreaToCheck obj);
        ServerAreaToCheck EditArea(AdminAreaToCheck obj);
        void RemoveArea(int areaId);

        void EditUser(User user);
        void RemoveReport(int id);
        void RemoveUser(int Id);
        User AddUser(User user);

    }
}
