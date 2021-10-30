using System.Collections.Generic;
using MongoDB.Driver;
using Pix_API.Base.MongoDB;
using Pix_API.PixBlocks.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;

namespace Pix_API.PixBlocks.MongoDB.Providers
{
	internal class MongoChampionshipProvider : MongoIdSaver_Base<Championship>, IChampionshipsMetadataProvider
	{
		public MongoChampionshipProvider(IMongoDatabase client, IMongoCollection<LastIndexHolder> index_collection)
			: base(client, index_collection, "championships")
		{
			IndexKeysDefinition<Championship> keys = Builders<Championship>.IndexKeys.Ascending((Championship s) => s.CountryId);
			db.Indexes.CreateOne(new CreateIndexModel<Championship>(keys));
		}

		public async void AddChampionships(Championship championship)
		{
			championship.Id = assigner.NextEmptyId;
			await db.InsertOneAsync(championship);
		}

		public List<Championship> GetAllChampionships()
		{
			return db.FindSync((Championship s) => true).ToList();
		}

		public List<Championship> GetAllChampionshipsForUser(int countryId, User authorize)
		{
			return db.FindSync((Championship sim) => sim.CountryId == countryId).ToList();
		}

		public async void RemoveChampionships(int ChampionshipId)
		{
			await db.DeleteOneAsync((Championship sim) => sim.Id == ChampionshipId);
		}

		public async void UpdateChampionships(Championship championship)
		{
			await db.ReplaceOneAsync((Championship s) => s.Id == championship.Id, championship);
		}
	}
}
