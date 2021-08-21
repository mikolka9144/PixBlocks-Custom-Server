using System.Collections.Generic;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using MongoDB.Driver;
using PixBlocks.Server.DataModels.DataModels.Woocommerce;
using System;

namespace Pix_API.Providers.MongoDB
{
    internal class MongoUserDatabaseProvider : IUserDatabaseProvider
    {
        private IMongoCollection<User> db;
        private IdAssigner assigner;

        public MongoUserDatabaseProvider(MongoClient client)
        {
            db = client.GetDatabase("Pix").GetCollection<User>("users");
            assigner = new IdAssigner(Convert.ToInt32(db.CountDocuments(sim => true)));
        }
        public async void AddUser(User user)
        {
            user.Id = assigner.NextEmptyId;
            await db.InsertOneAsync(user);
        }

        public bool ContainsUserWithEmail(string email)
        {
            return db.Find(s => s.Email == email).Any();
        }

        public bool ContainsUserWithLogin(string login)
        {
            return db.Find(s => s.Student_login == login).Any();
        }

        public List<User> GetAllUsersBelongingToClass(int classId)
        {
            return db.Find(s => s.Student_studentsClassId == classId).ToList();
        }

        public User GetUser(string EmailOrLogin)
        {
            return db.Find(s => s.Email == EmailOrLogin || s.Student_login == EmailOrLogin).FirstOrDefault();
        }

        public User GetUser(int Id)
        {
            return db.Find(s => s.Id == Id).FirstOrDefault();
        }

        public async void UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq("Id", user.Id);
            await db.ReplaceOneAsync(filter, user);
        }
    }
}