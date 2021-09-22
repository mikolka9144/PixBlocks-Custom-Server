using System.Collections.Generic;
using MongoDB.Driver;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Providers.MongoDB.Providers
{
	internal class MongoStudentClassesProvider : MongoIdSaver_Base<StudentsClass>, IStudentClassProvider
	{
		private readonly IUserDatabaseProvider userDatabase;

		public MongoStudentClassesProvider(IMongoDatabase client, IMongoCollection<LastIndexHolder> index_collection, IUserDatabaseProvider databaseProvider)
			: base(client, index_collection, "classes")
		{
			userDatabase = databaseProvider;
		}

		private void CreateIndexes()
		{
			IndexKeysDefinition<StudentsClass> keys = Builders<StudentsClass>.IndexKeys.Ascending((StudentsClass s) => s.TeacherID);
			db.Indexes.CreateOneAsync(new CreateIndexModel<StudentsClass>(keys));
		}

		public void AddClassForUser(StudentsClass studentsClass, int userId)
		{
			studentsClass.Id = assigner.NextEmptyId;
            db.InsertOneAsync(studentsClass);
		}

		public void EditClassForUser(StudentsClass studentsClass, int userId)
		{
			db.ReplaceOneAsync((StudentsClass s) => s.Id == studentsClass.Id, studentsClass);
		}

		public List<StudentsClass> GetClassesForUser(int Id)
		{
			return db.FindSync((StudentsClass s) => s.TeacherID == Id).ToList();
		}

		public StudentsClass GetStudentsClassByGlobalId(int classId)
		{
			return db.FindSync((StudentsClass sim) => sim.Id == (int?)classId).FirstOrDefault();
		}

		public StudentsClass GetStudentsClassById(int userId, int classId)
		{
			StudentsClass studentsClass = db.FindSync((StudentsClass sim) => sim.Id == (int?)classId).FirstOrDefault();
			if (studentsClass.TeacherID == userId)
			{
				return studentsClass;
			}
			return null;
		}

		public List<User> GetStudentsInClassForUser(int userID, int classID)
		{
			return userDatabase.GetAllUsersBelongingToClass(classID);
		}

		public async void RemoveClass(StudentsClass studentsClass)
		{
			await db.DeleteOneAsync((StudentsClass sim) => sim.Id == studentsClass.Id);
		}
	}
}
