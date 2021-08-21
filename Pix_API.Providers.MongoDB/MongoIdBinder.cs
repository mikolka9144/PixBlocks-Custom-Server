using System;
using MongoDB.Bson;
namespace Pix_API.Providers.MongoDB
{
    public class MongoIdBinder<T>
    {
        public MongoIdBinder(T obj)
        {
            this.Id = ObjectId.GenerateNewId();
            Obj = obj;
        }

        public ObjectId Id { get; }
        public T Obj { get; }
    }
}
