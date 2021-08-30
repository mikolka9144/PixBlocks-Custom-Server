using System.Collections.Generic;
using MongoDB.Driver;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Providers.MongoDB
{
	internal class MongoUserDatabaseProvider : MongoIdSaver_Base<User>, IUserDatabaseProvider
	{
		public MongoUserDatabaseProvider(IMongoDatabase client, IMongoCollection<LastIndexHolder> index_collection)
			: base(client, index_collection, "users")
		{
		}

		public async void AddUser(User user)
		{
			user.Id = assigner.NextEmptyId;
			await db.InsertOneAsync(user);
		}

		public bool ContainsUserWithEmail(string email)
		{
			return db.Find((User s) => s.Email == email).Any();
		}

		public bool ContainsUserWithLogin(string login)
		{
			return db.Find((User s) => s.Student_login == login).Any();
		}

		public List<User> GetAllUsersBelongingToClass(int classId)
		{
			return db.Find((User s) => s.Student_studentsClassId == (int?)classId).ToList();
		}

		public User GetUser(string EmailOrLogin)
		{
			return db.Find((User s) => s.Email == EmailOrLogin || s.Student_login == EmailOrLogin).FirstOrDefault();
		}

		public User GetUser(int Id)
		{
			return db.Find((User s) => s.Id == (int?)Id).FirstOrDefault();
		}

		public async void RemoveUser(int Id)
		{
			await db.DeleteOneAsync((User s) => s.Id == (int?)Id);
		}

		public async void UpdateUser(User user)
		{
			FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", user.Id);
			await db.ReplaceOneAsync(filter, user);
		}
	}
}
