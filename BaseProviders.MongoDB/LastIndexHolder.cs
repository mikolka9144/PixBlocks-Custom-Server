using MongoDB.Bson.Serialization.Attributes;

namespace Pix_API.Base.MongoDB
{
	public class LastIndexHolder
	{
		[BsonId]
		public string name;

		public int value;
	}
}
