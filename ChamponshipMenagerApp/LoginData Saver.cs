using System;
using System.IO;

namespace ChamponshipMenagerApp
{
    public static class LoginData_Saver
    {
        public static void Save(string login) => File.WriteAllText("./login.cfg", login);
        public static string Load() => File.Exists("./login.cfg") ? File.ReadAllText("./login.cfg") : "";
    }
}
