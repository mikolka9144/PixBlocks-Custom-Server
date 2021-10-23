using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChamponshipMenagerApp;
using Xwt;

namespace ChampionshipCreatorApp.WPF
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Initialize(ToolkitType.Wpf);

            var mainWindow = new LoginWindow();

            mainWindow.Show();
            Application.Run();
            mainWindow.Dispose();
        }
    }
}
