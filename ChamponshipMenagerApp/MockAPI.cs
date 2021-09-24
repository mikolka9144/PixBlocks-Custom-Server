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
            championships = new List<Championship>();
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
            new Countrie(0,"test","PL"),
            new Countrie(1,"testwww","PL"),
            new Countrie(2,"testcxxxx","PL")
        };
        public void RemoveChampionships(int ChampionshipId)
        {
            throw new NotImplementedException();
        }

        public void UpdateChampionship(Championship championship)
        {
            throw new NotImplementedException();
        }
    }
}
