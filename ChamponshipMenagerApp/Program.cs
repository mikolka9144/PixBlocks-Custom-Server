using System;
using Xwt;
namespace ChamponshipMenagerApp
{
    class MainClass
    {
        private static LoginWindow mainWindow;

        static void Main()
        {
            InitaliseLibraly();

            mainWindow = new LoginWindow();

            mainWindow.Show();
            Application.Run();
            mainWindow.Dispose();
        }

        private static void InitaliseLibraly()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    Application.Initialize(ToolkitType.Gtk);
                    break;
                case PlatformID.Win32NT:
                    Application.Initialize(ToolkitType.Wpf);
                    break;
            }
        }



    }
}
