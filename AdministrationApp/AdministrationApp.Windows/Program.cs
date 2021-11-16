using System;
using ChamponshipMenagerApp;
using Xwt;

namespace AdministrationApp.Windows
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Initialize(ToolkitType.Wpf);

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
