using MongoDB.Driver;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Providers.MongoDB
{
	internal class MongoSchoolsProvider : ISchoolProvider
	{
		private IMongoCollection<School> db;

		public MongoSchoolsProvider(IMongoDatabase client)
		{
			db = client.GetCollection<School>("schools");
		}

		public async void AddSchool(School school)
		{
			school.Id = school.CreatorUserID;
			await db.InsertOneAsync(school);
		}

		public School GetSchool(int UserOwner_Id)
		{
			return db.FindSync((School sim) => sim.Id == UserOwner_Id).FirstOrDefault();
		}

		public async void UpdateSchool(School school, int UserOwner_Id)
		{
			school.Id = UserOwner_Id;
			await db.ReplaceOneAsync((School s) => s.Id == UserOwner_Id, school);
		}
	}
}
