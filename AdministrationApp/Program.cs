using System;
using Xwt;
using ChamponshipMenagerApp;

namespace AdministrationApp
{
    class MainClass
    {

        public static void Main(string[] args)
        {
            Application.Initialize(ToolkitType.Gtk);

            var window = new LoginWindow();
            window.Size = new Size(400, 400);
            window.Show();
            Application.Run();
            window.Dispose();
        }
    }
}
