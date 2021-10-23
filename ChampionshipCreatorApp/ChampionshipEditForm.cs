using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using Xwt;

namespace ChamponshipMenagerApp
{
    public class ChampionshipEditForm:VBox
    {
       
        private readonly ChampionshipListView listView;
        private readonly int? championshipRow;
        private readonly int championshipId = -1;

        private readonly TextEntry Name_box = new TextEntry();
        private readonly TextEntry Regulations_box = new TextEntry();
        private readonly TextEntry Shedule_box = new TextEntry();
        private readonly DatePicker DateStart_box = new DatePicker();
        private readonly DatePicker DateEnd_box = new DatePicker();
        private readonly ComboBox countryId = new ComboBox();

        public ChampionshipEditForm(ChampionshipListView listView,List<Countrie> countries,int? championshipRow = null)
        {
            this.listView = listView;
            this.championshipRow = championshipRow;
            countries.ForEach(s => countryId.Items.Add(s, s.Name));
            countryId.SelectedIndex = 0;
            Table table_form = SetupTableForm();

            PackStart(table_form, true);

            var Save_btn = new Button()
            {
                Label = championshipRow != null ? "Edit" : "Add"
            };
            Save_btn.Clicked += Save_Btn_Clicked;
            PackEnd(Save_btn);
            if (championshipRow.HasValue)
            {
                var championship = listView.Get(championshipRow.Value);
                championshipId = championship.Id;

                Name_box.Text = championship.Name;
                Regulations_box.Text = championship.Regulations;
                Shedule_box.Text = championship.Schedule;
                DateStart_box.DateTime = championship.Start_date;
                DateEnd_box.DateTime = championship.End_date ?? DateTime.Now;
                countryId.SelectedText = countries.Find(s => s.Id == championship.CountryId).Name;
            }
        }

        private Table SetupTableForm()
        {
            var table_form = new Table();

            table_form.Add(new Label("Name"), 0, 0);
            table_form.Add(Name_box, 0, 1, 1, 1, true);

            table_form.Add(new Label("Regulations (url recomended)"), 0, 2);
            table_form.Add(Regulations_box, 0, 3, 1, 1, true);

            table_form.Add(new Label("Shedule (url recomended)"), 0, 4);
            table_form.Add(Shedule_box, 0, 5, 1, 1, true);

            table_form.Add(new Label("Championship start time"), 0, 6);
            table_form.Add(DateStart_box, 0, 7, 1, 1, true);

            table_form.Add(new Label("Championship end time"), 0, 8);
            table_form.Add(DateEnd_box, 0, 9, 1, 1, true);

            table_form.Add(new Label("Country"), 0, 10);
            table_form.Add(countryId, 0, 11, 1, 1, true);
            return table_form;
        }

        void Save_Btn_Clicked(object sender, EventArgs e)
        {
            var country = countryId.SelectedItem as Countrie;
            var championship = new Championship()
            {
                Id = championshipId,
                Name = Name_box.Text,
                Regulations = Regulations_box.Text,
                Schedule = Shedule_box.Text,
                Start_date = DateStart_box.DateTime,
                End_date = DateEnd_box.DateTime,
                CountryId = country.Id
            };
            if (championshipId == -1)
            {
                listView.Add(championship);
            }
            else listView.Edit(championshipRow.Value, championship);
            ParentWindow.Close();
        }
        public static Dialog EditDialog(ChampionshipListView listView, List<Countrie> countries,int championship)
        {
            var controller = new ChampionshipEditForm(listView,countries,championship);
            var dialog = new Dialog()
            {
                Content = controller,
                Title = "Edit Championship"
            };
            return dialog;
        }
        public static Window AddDialog(ChampionshipListView listView, List<Countrie> countries)
        {
            var controller = new ChampionshipEditForm(listView,countries);
            var dialog = new Window()
            {
                Content = controller,
                Title = "New Championship",
            };
            return dialog;
        }

    }
    public enum ChampionshipAction
    {
        Add,
        Edit
    }
}
