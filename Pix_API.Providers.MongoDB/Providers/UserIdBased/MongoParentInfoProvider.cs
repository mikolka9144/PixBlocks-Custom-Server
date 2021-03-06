using MongoDB.Driver;

using Pix_API.PixBlocks.Interfaces;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.PixBlocks.MongoDB
{
	internal class MongoParentInfoProvider : IParentInfoHolder
	{
		private IMongoCollection<ParentInfo> db;

		public MongoParentInfoProvider(IMongoDatabase client)
		{
			db = client.GetCollection<ParentInfo>("parentInfo");
		}

		public void AddOrUpdateParentInfoForUser(ParentInfo parentInfo, int User_Id)
		{
			parentInfo.Id = User_Id;
			db.InsertOne(parentInfo);
        }

        public async void RemoveParentInfo(int userId)
        {
            await db.DeleteOneAsync(s => s.Id == userId);
        }
    }
}
