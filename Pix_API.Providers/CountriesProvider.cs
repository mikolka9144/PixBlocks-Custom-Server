using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using System.Linq;
using Pix_API.Interfaces;
using System.IO;

namespace Pix_API.Providers
{
    public class CountriesProvider:ICountriesProvider
    {
        private List<Countrie> countries = new List<Countrie>();
        public CountriesProvider()
        {
            // country_name,Code
            // Polska,PL
            if(!File.Exists("countries.csv"))
                File.WriteAllText("countries.csv", "country_name,Code\nPolska,PL\nAngia,EN");
            var raw_csv = File.ReadAllText("./countries.csv");
            var lines = raw_csv.Split('\n').Skip(1);
            lines = lines.Take(lines.Count() - 1);
            for (int i = 0; i < lines.Count(); i++)
            {
                var seg = lines.ElementAt(i).Split(',');
                countries.Add(new Countrie()
                {
                    Name = seg[0],
                    Code = seg[1],
                    Id = i
                });
            }
        }
        public List<Countrie> GetAllCountries() => countries;
    }

}
