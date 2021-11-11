using System;
using System.Collections.Generic;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Linq;

namespace AdministrationApp
{
    public class MockAPI:IAPIClient
    {
        private List<ServerAreaToCheck> areas = new List<ServerAreaToCheck> {
                new ServerAreaToCheck{ Id = 0,name = "test",terrain="aaa"},
                new ServerAreaToCheck{ Id = 1,name = "elo ",terrain="aaa"},
                new ServerAreaToCheck{ Id = 3,name = "elaaao",terrain="aaa"},
                new ServerAreaToCheck{ Id = 4,name = "eldwdo",terrain="aaa"},
                new ServerAreaToCheck{Id = 2,name = "kjhg",terrain="cefg"}
            };
        private List<User> users = new List<User>
        {
            new User{Id = 0,login = "aaa",passwordHash = "rjgheuirfhju"},
            new User{Id = 1,login = "bbbbb",passwordHash = "rjgheuirfhju"},
            new User{Id = 4,login = "miko",passwordHash = "rjgheuirfhju"}
        };
        private List<ObjectInArea> objects = new List<ObjectInArea>();
        private int i = 5;
        private int Bi = 5;


        public List<ServerAreaToCheck> GetAllAreasToCheck() => areas.ToList();

        public List<User> GetAllUsers()
        {
            return users.ToList();
        }

        public bool IsServerOnline()
        {
            return true;
        }

        public bool LoginToServer(string login, string password)
        {
            Console.WriteLine(login);
            return true;
        }

        public void RemoveReport(ClientAreaToCheck area)
        {
            areas.RemoveAll(s => s.Id == area.Id);
        }

        public ObjectInArea GetObject(int Id)
        {
            return objects.FirstOrDefault(s => s.Id == Id);
        }

        public ObjectInArea AddObject(ObjectInArea area)
        {
            area.Id = Bi++;
            objects.Add(area);
            return area;
        }

        public void RemoveObject(int Id)
        {
            objects.RemoveAll(s => s.Id == Id);
        }

        public void UpdateObject(ObjectInArea area)
        {
            objects.RemoveAll(s => s.Id == area.Id);
            objects.Add(area); 
        }

        public ServerAreaToCheck AddArea(ServerAreaToCheck obj)
        {
            obj.Id = i++;
            areas.Add(obj);
            return obj;
        }

        public void EditReport(ServerAreaToCheck obj)
        {
            areas.RemoveAll(s => s.Id == obj.Id);
            areas.Add(obj);
        }

        public void RemoveReport(int areaId)
        {
            areas.RemoveAll(s => s.Id == areaId);
        }

        public void EditUser(User user)
        {
            RemoveUser(user);
            users.Add(user);
        }

        public void RemoveUser(User user)
        {
            users.RemoveAll(s => s.Id == user.Id);
        }
        private int x = 5;
        public User AddUser(User user)
        {
            user.Id = x++;
            users.Add(user);
            return user;
        }
    }
}
