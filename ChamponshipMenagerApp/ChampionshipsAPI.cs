using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using RestSharp;

namespace ChamponshipMenagerApp
{
    public class ChampionshipsAPI : IChampionshipsAPI
    {
        private readonly string url;

        public ChampionshipsAPI(string url)
        {
            this.url = url;
        }
        public T CallServerCommand<T>(string name, object p1)
        {
            var p1_serialized = JsonConvert.SerializeObject(p1);
            var client = new RestClient(url);
            var request = new RestRequest()
                .AddQueryParameter("me", name)
                .AddQueryParameter("p1", p1_serialized);
            var response = client.Get(request).Content;
            response = response.Substring(1, response.Length - 2);
            response = response.Replace("\\\"", "\"");
            return JsonConvert.DeserializeObject<T>(response);
        }

        public List<Countrie> GetAllCountries()
        {
            return CallServerCommand<List<Countrie>>("GetAllCountries", null);
        }
        public int AddChampionship(Championship championship)
        {
            return CallServerCommand<int>("AddChampionship", championship);
        }

        public void UpdateChampionship(Championship championship)
        {
            CallServerCommand<object>("UpdateChampionship", championship);
        }

        public void RemoveChampionships(int ChampionshipId)
        {
            CallServerCommand<object>("RemoveChampionship", ChampionshipId);
        }
        public List<Championship> GetAllActiveChampionships()
        {
            return CallServerCommand<List<Championship>>("GetAllChampionships", null);
        }
    }
    public interface IChampionshipsAPI
    {
        int AddChampionship(Championship championship);
        List<Championship> GetAllActiveChampionships();
        List<Countrie> GetAllCountries();
        void RemoveChampionships(int ChampionshipId);
        void UpdateChampionship(Championship championship);
    }
}

