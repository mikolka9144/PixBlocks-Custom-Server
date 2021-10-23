using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;

namespace ChamponshipMenagerApp
{
    public class MockAPI:IChampionshipsAPI
    {
        private List<Championship> championships;

        public MockAPI()
        {
            championships = new List<Championship>()
            {
                new Championship()
                {
                    Id = 0,
                    Name = "No comment",
                    Start_date = DateTime.Now,
                    End_date = DateTime.Now.AddDays(2),
                    CountryId = 0
                }
            };
        }

        public int AddChampionship(Championship championship)
        {
            championship.Id = championships.Count;
            championships.Add(championship);
            return championship.Id;
        }

        public List<Championship> GetAllActiveChampionships()
        {
            return championships;
        }

        public List<Countrie> GetAllCountries() => new List<Countrie>()
        {
            new Countrie(0,"Polska","PL"),
            new Countrie(1,"Czechy","CZ"),
            new Countrie(2,"Niemcy","DE")
        };
        public void RemoveChampionships(int ChampionshipId)
        {
            championships.RemoveAll(s => s.Id == ChampionshipId);
        }

        public void UpdateChampionship(Championship championship)
        {
            //
        }
    }
}
