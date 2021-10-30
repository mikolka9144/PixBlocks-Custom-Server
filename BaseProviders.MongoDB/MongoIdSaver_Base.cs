using MongoDB.Driver;
using Pix_API.Base.Utills;

namespace Pix_API.Base.MongoDB
{
	public abstract class MongoIdSaver_Base<T>
	{
		protected IMongoCollection<T> db;

		protected IdAssigner assigner;

        protected MongoIdSaver_Base(IMongoDatabase client, IMongoCollection<LastIndexHolder> index_collection, string collectionName)
		{
			db = client.GetCollection<T>(collectionName);
			assigner = new IdAssigner(new MongoIndexSaver(index_collection, collectionName));
		}
	}
}
