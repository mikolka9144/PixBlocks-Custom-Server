using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;

namespace Pix_API.Interfaces
{
	public interface IChampionshipsMetadataProvider
	{
		List<Championship> GetAllChampionshipsForUser(int countryId, User authorize);

		void AddChampionships(Championship championship);

		void UpdateChampionships(Championship championship);

		void RemoveChampionships(int ChampionshipId);

		List<Championship> GetAllChampionships();
	}
}
