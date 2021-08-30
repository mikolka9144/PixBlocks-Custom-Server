using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Providers
{
	public class CountriesProvider : ICountriesProvider
	{
		private List<Countrie> countries = new List<Countrie>();

		public CountriesProvider()
		{
			if (!File.Exists("countries.csv"))
			{
				File.WriteAllText("countries.csv", "country_name,Code\nPolska,PL\nAngia,EN");
			}
			IEnumerable<string> source = File.ReadAllText("./countries.csv").Split('\n').Skip(1);
			source = source.Take(source.Count() - 1);
			for (int i = 0; i < source.Count(); i++)
			{
				string[] array = source.ElementAt(i).Split(',');
				countries.Add(new Countrie
				{
					Name = array[0],
					Code = array[1],
					Id = i
				});
			}
		}

		public List<Countrie> GetAllCountries()
		{
			return countries;
		}
	}
}
