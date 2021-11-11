using System;
using Xwt;
using AdministrationApp.Controls.Panels;
using AdministrationApp.Windows;
namespace AdministrationApp
{
    public class MainControlPanel:Window
    {
        private readonly IAPIClient client;

        public MainControlPanel(IAPIClient client)
        {
            Closed += MainControlPanel_Closed;

            var areas = client.GetAllAreasToCheck();
            var tabs = new Notebook();
            tabs.Add(new AreasTab(client,areas),"Areas");
            tabs.Add(new UsersTab(client, areas), "Users");
            Content = tabs;
            this.client = client;
        }


        private void MainControlPanel_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
