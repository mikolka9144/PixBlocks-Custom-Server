 using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using Xwt;

namespace ChamponshipMenagerApp
{
    public class ChampionshipListView : ListView
    {
        private readonly IChampionshipsAPI api;
        private readonly List<Countrie> countries;
        DataField<int> Id = new DataField<int>();
        DataField<string> name = new DataField<string>();
        DataField<DateTime> start_date = new DataField<DateTime>();
        DataField<DateTime?> end_date = new DataField<DateTime?>();
        DataField<string> country = new DataField<string>();
        private ListStore DataStore;
        private List<Championship> championships = new List<Championship>();

        public ChampionshipListView(IChampionshipsAPI api)
        {
            this.api = api;
            countries = api.GetAllCountries();
            DataStore = new ListStore(Id, name,start_date,end_date,country);
            DataSource = DataStore;
            Columns.Add("Id", Id);
            Columns.Add("Name", name);
            Columns.Add("Start date", start_date);
            Columns.Add("End date", end_date);
            Columns.Add("Country", country);
            GridLinesVisible = GridLines.Vertical;
            api.GetAllActiveChampionships().ForEach(s => Add(s,false));
        }
        public int Add(Championship championship,bool SendToServer = true)
        {
            if (SendToServer)
            {
                var newIndex = api.AddChampionship(championship);
                championship.Id = newIndex;
            }

            var row = DataStore.AddRow();
            DataStore.SetValues(row, 
                Id, championship.Id, 
                name, championship.Name,
                start_date,championship.Start_date,
                end_date,championship.End_date,
                country,countries[championship.CountryId].Name);
            championships.Add(championship);
            return row;
        }
        public void Remove(int row)
        {
            int championship = championships[row].Id;
            DataStore.RemoveRow(row);
            championships.RemoveAt(row);
            api.RemoveChampionships(championship);

        }
        public void Edit(int row,Championship championship)
        {
            DataStore.SetValues(row,
                Id, championship.Id,
                name, championship.Name,
                start_date, championship.Start_date,
                end_date, championship.End_date,
                country, countries[championship.CountryId].Name);
            championships[row] = championship;
            api.UpdateChampionship(championship);
        }
        public Championship Get(int row) => championships[row];
    }
}
