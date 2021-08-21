using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using MongoDB.Driver;
using System;
using Pix_API.Providers.SinglePoolProviders;

namespace Pix_API.Providers.MongoDB
{
    internal class MongoSchoolsProvider : ISchoolProvider
    {
        private IMongoCollection<School> db;

        public MongoSchoolsProvider(MongoClient client)
        {
            db = client.GetDatabase("Pix").GetCollection<School>("schools");
        }
        public async void AddSchool(School school)
        {
            school.Id = school.CreatorUserID;
            await db.InsertOneAsync(school);
        }

        public School GetSchool(int UserOwner_Id)
        {
            return db.FindSync(sim => sim.Id == UserOwner_Id).FirstOrDefault();
        }

        public async void UpdateSchool(School school, int UserOwner_Id)
        {
            school.Id = UserOwner_Id;
            await db.ReplaceOneAsync(s => s.Id == UserOwner_Id, school);
        }
    }
}