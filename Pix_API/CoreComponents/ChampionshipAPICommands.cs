using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels.Championsships;

namespace Pix_API.CoreComponents
{
	public class ChampionshipAPICommands : ICommandRepository
	{
		private readonly IChampionshipsMetadataProvider championshipsProvider;

		public ChampionshipAPICommands(IChampionshipsMetadataProvider championshipsProvider)
		{
			this.championshipsProvider = championshipsProvider;
		}

		public void AddChampionship(Championship championship)
		{
			championshipsProvider.AddChampionships(championship);
		}

		public void UpdateChampionship(Championship championship)
		{
			championshipsProvider.UpdateChampionships(championship);
		}

		public void RemoveChampionships(int ChampionshipId)
		{
			championshipsProvider.RemoveChampionships(ChampionshipId);
		}
	}
}
