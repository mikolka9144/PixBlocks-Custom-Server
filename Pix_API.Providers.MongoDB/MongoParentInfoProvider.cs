using MongoDB.Driver;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Providers.MongoDB
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
	}
}
