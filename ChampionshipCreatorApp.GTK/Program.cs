using System;
using Xwt;
using ChamponshipMenagerApp;

namespace ChampionshipCreatorApp.GTK
{
    class MainClass
    {
        private static LoginWindow mainWindow;

        static void Main()
        {
            Application.Initialize(ToolkitType.Gtk);

            mainWindow = new LoginWindow();

            mainWindow.Show();
            Application.Run();
            mainWindow.Dispose();
        }
    }
}
