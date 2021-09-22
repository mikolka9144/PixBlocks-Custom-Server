using System;
using MongoDB.Driver;
using Pix_API.Interfaces;
using Pix_API.Providers.BaseClasses;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.Providers.MongoDB
{
	internal class MongoToyShopProvider : IToyShopProvider
	{
		private IMongoCollection<IdObjectBinder<ToyShopData>> db;

		public MongoToyShopProvider(IMongoDatabase client)
		{
			db = client.GetCollection<IdObjectBinder<ToyShopData>>("toyShops");
		}

		public ToyShopData GetToyShop(int UserId)
		{
			return db.FindSync((IdObjectBinder<ToyShopData> sim) => sim.Id == UserId).FirstOrDefault()?.Obj;
        }

        public async void RemoveToyShop(int UserId)
        {
            await db.DeleteOneAsync(s => s.Id == UserId);
        }

        public async void SaveOrUpdateToyShop(ToyShopData toyShopData, int UserId)
		{
			toyShopData.ID = UserId;
			if ((await db.FindAsync((IdObjectBinder<ToyShopData> sim) => sim.Id == UserId)).Any())
			{
				UpdateDefinition<IdObjectBinder<ToyShopData>> update = Builders<IdObjectBinder<ToyShopData>>.Update.Set((IdObjectBinder<ToyShopData> s) => s.Obj.ToyShopInfoBase64, toyShopData.ToyShopInfoBase64).Set((IdObjectBinder<ToyShopData> s) => s.Obj.UpdateTime, DateTime.Now);
				await db.UpdateOneAsync((IdObjectBinder<ToyShopData> sim) => sim.Id == UserId, update);
			}
			else
			{
				await db.InsertOneAsync(new IdObjectBinder<ToyShopData>(UserId, toyShopData));
			}
		}
	}
}
