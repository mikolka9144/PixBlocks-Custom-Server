using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using MongoDB.Driver;
using Pix_API.Providers.BaseClasses;
using System;

namespace Pix_API.Providers.MongoDB
{
    internal class MongoToyShopProvider : IToyShopProvider
    {
        private IMongoCollection<IdObjectBinder<ToyShopData>> db;

        public MongoToyShopProvider(MongoClient client)
        {
            db = client.GetDatabase("Pix").GetCollection<IdObjectBinder<ToyShopData>>("toyShops");
        }
        public ToyShopData GetToyShop(int UserId)
        {
            return db.FindSync(sim => sim.Id == UserId).FirstOrDefault()?.Obj;
        }

        public async void SaveOrUpdateToyShop(ToyShopData toyShopData, int UserId)
        {
            toyShopData.ID = UserId;
            var WasToyShopCreated = await db.FindAsync(sim => sim.Id == UserId);
            if (WasToyShopCreated.Any())
            {
                var update = Builders<IdObjectBinder<ToyShopData>>.Update
                    .Set(s => s.Obj.ToyShopInfoBase64,toyShopData.ToyShopInfoBase64)
                    .Set(s => s.Obj.UpdateTime, DateTime.Now);
                await db.UpdateOneAsync(sim => sim.Id == UserId, update);
            }
            else
            {
                await db.InsertOneAsync(new IdObjectBinder<ToyShopData>(UserId, toyShopData));
            }
        }
    }
}