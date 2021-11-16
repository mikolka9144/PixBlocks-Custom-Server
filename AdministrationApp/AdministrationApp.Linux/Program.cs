using System;
using ChamponshipMenagerApp;
using Xwt;

namespace AdministrationApp.Linux
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Initialize(ToolkitType.Gtk);

            var window = new LoginWindow
            {
                Size = new Size(400, 400)
            };
            window.Show();
            Application.Run();
            window.Dispose();
        }
    }
}
