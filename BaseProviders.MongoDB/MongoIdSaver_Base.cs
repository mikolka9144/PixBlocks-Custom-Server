using MongoDB.Driver;

namespace Pix_API.Providers.MongoDB
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
