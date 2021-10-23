using System.Collections.Generic;
using System.Linq;
using Base.Providers.ContainersProviders;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;

namespace Pix_API.Providers.StaticProviders
{
	public class ChampionshipProvider : SinglePoolStorageProvider<Championship>, IChampionshipsMetadataProvider
	{
		private IdAssigner idAssigner;

		public ChampionshipProvider(DataSaver<Championship> saver)
			: base(saver)
		{
			idAssigner = new IdAssigner(new DiskIndexSaver("championship.index"));
		}

		public void AddChampionships(Championship championship)
		{
			int id = (championship.Id = idAssigner.NextEmptyId);
			AddSingleObject(championship, id);
		}

		public List<Championship> GetAllChampionships()
		{
			return base.storage.ToList();
		}

		public List<Championship> GetAllChampionshipsForUser(int countryId, User authorize)
		{
			return base.storage.Where((Championship s) => s.CountryId == countryId).ToList();
		}

		public void RemoveChampionships(int ChampionshipId)
		{
			RemoveObject(ChampionshipId);
		}

		public void UpdateChampionships(Championship championship)
		{
			AddOrUpdateSingleObject(championship, championship.Id);
		}
	}
}
