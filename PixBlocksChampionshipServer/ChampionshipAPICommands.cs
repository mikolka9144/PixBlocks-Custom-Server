using System.Collections.Generic;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;

namespace Pix_API.PixBlocks
{
	public class ChampionshipAPICommands : ICommandRepository
	{
		private readonly IChampionshipsMetadataProvider championshipsProvider;
        private readonly ICountriesProvider countriesProvider;

        public ChampionshipAPICommands(IChampionshipsMetadataProvider championshipsProvider, ICountriesProvider countriesProvider)
        {
            this.championshipsProvider = championshipsProvider;
            this.countriesProvider = countriesProvider;
        }
        public List<Countrie> GetAllCountries(object _)
        {
            return countriesProvider.GetAllCountries();
        }
        public int AddChampionship(Championship championship)
		{
			championshipsProvider.AddChampionships(championship);
            return championship.Id;
		}

		public void UpdateChampionship(Championship championship)
		{
			championshipsProvider.UpdateChampionships(championship);
		}

		public void RemoveChampionship(int ChampionshipId)
		{
			championshipsProvider.RemoveChampionships(ChampionshipId);
		}
        public List<Championship> GetAllChampionships(object _)
        {
            return championshipsProvider.GetAllChampionships();
        }
    }
}
