using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using Xwt;
using Xwt.Drawing;

namespace ChamponshipMenagerApp
{
    public class MainWindow : Window
    {
        public IChampionshipsAPI api;
        private ChampionshipListView grid;
        private List<Countrie> Countrie;

        public MainWindow(IChampionshipsAPI api)
        {
            Title = "Championships";
            Width = 500;
            Height = 400;
            Closed += Window_Closed;
            var controller = new VBox();

            Countrie = api.GetAllCountries();
            var Hbox = new HBox();
            var addButton = new Button("Add");
            addButton.Clicked += AddButton_Clicked;
            var removeButton = new Button("Remove");
            removeButton.Clicked += RemoveButton_Clicked;
            var editButton = new Button("Edit");
            editButton.Clicked += EditButton_Clicked;
            grid = new ChampionshipListView(api);
            controller.PackStart(grid, true);

            Hbox.PackStart(addButton, true);
            Hbox.PackStart(editButton, true);
            Hbox.PackStart(removeButton, true);
            controller.PackEnd(Hbox);
            Content = controller;
        }


        void AddButton_Clicked(object sender, EventArgs e)
        {
            var dialog = ChampionshipEditForm.AddDialog(grid, Countrie);
            dialog.Show();
        }

        void EditButton_Clicked(object sender, EventArgs e)
        {
            var index = grid.SelectedRow;
            if (index == -1)
            {
                MessageDialog.ShowError("You need to Select Champonship first");
                return;
            }
            var dialog = ChampionshipEditForm.EditDialog(grid, Countrie, index);
            dialog.Show();
        }
        void RemoveButton_Clicked(object sender, EventArgs e)
        {
            var index = grid.SelectedRow;
            if (index == -1)
            {
                MessageDialog.ShowError("You need to Select Champonship first");
                return;
            }
            grid.Remove(index);
        }
        void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
