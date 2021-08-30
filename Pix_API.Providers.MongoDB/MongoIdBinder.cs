using MongoDB.Bson;

namespace Pix_API.Providers.MongoDB
{
	public class MongoIdBinder<T>
	{
		public ObjectId Id { get; }

		public T Obj { get; }

		public MongoIdBinder(T obj)
		{
			Id = ObjectId.GenerateNewId();
			Obj = obj;
		}
	}
}
