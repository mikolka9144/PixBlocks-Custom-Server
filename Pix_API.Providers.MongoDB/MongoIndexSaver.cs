using MongoDB.Driver;

namespace Pix_API.Providers.MongoDB
{
	public class MongoIndexSaver : ILastIndexSaver
	{
		private readonly string index_Name;

		public IMongoCollection<LastIndexHolder> Collection { get; }

		public MongoIndexSaver(IMongoCollection<LastIndexHolder> collection, string index_name)
		{
			Collection = collection;
			index_Name = index_name;
			if (!Collection.FindSync((LastIndexHolder s) => s.name == index_Name).Any())
			{
				collection.InsertOne(new LastIndexHolder
				{
					name = index_name,
					value = 0
				});
			}
		}

		public int LoadLastUnusedIndex()
		{
			return Collection.FindSync((LastIndexHolder s) => s.name == index_Name).First().value;
		}

		public void SaveNewLastUnusedIndex(int index)
		{
			Collection.UpdateOne((LastIndexHolder s) => s.name == index_Name, Builders<LastIndexHolder>.Update.Set((LastIndexHolder s) => s.value, index));
		}
	}
}
