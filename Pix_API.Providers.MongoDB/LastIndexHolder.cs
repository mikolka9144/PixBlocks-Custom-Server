using MongoDB.Bson.Serialization.Attributes;

namespace Pix_API.Providers.MongoDB
{
	public class LastIndexHolder
	{
		[BsonId]
		public string name;

		public int value;
	}
}
