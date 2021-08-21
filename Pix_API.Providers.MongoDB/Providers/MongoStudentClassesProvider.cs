using System;
using System.Collections.Generic;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using MongoDB.Driver;

namespace Pix_API.Providers.MongoDB
{
    internal class MongoStudentClassesProvider : IStudentClassProvider
    {
        private readonly IUserDatabaseProvider userDatabase;
        private IMongoCollection<StudentsClass> db;
        private IdAssigner assigner;

        public MongoStudentClassesProvider(MongoClient client,IUserDatabaseProvider userDatabase)
        {
            db = client.GetDatabase("Pix").GetCollection<StudentsClass>("classes");
            assigner = new IdAssigner(Convert.ToInt32(db.CountDocuments(sim => true)));
            CreateIndexes();
            this.userDatabase = userDatabase;
        }

        private void CreateIndexes()
        {
            var index = Builders<StudentsClass>.IndexKeys.Ascending(s => s.TeacherID);
            db.Indexes.CreateOneAsync(new CreateIndexModel<StudentsClass>(index));
        }

        public async void AddClassForUser(StudentsClass studentsClass, int userId)
        {
            studentsClass.Id = assigner.NextEmptyId;
            await db.InsertOneAsync(studentsClass);
        }

        public void EditClassForUser(StudentsClass studentsClass, int userId)
        {
            db.ReplaceOneAsync(s => s.Id == studentsClass.Id,studentsClass);
        }

        public List<StudentsClass> GetClassesForUser(int Id)
        {
            return db.FindSync(s => s.TeacherID == Id).ToList();
        }

        public StudentsClass GetStudentsClassByGlobalId(int classId)
        {
            return db.FindSync(sim => sim.Id == classId).FirstOrDefault();
        }

        public StudentsClass GetStudentsClassById(int userId, int classId)
        {
            var student_class = db.FindSync(sim => sim.Id == classId).FirstOrDefault();
            if (student_class.TeacherID == userId) return student_class;
            return null;
        }

        public List<User> GetStudentsInClassForUser(int userID, int classID)
        {
            return userDatabase.GetAllUsersBelongingToClass(classID);
        }

        public async void RemoveClassForUser(StudentsClass studentsClass, int userId)
        {
            var update = Builders<StudentsClass>.Update.Set(s => s.IsDeleted, true);
            await db.UpdateOneAsync(sim => sim.Id == studentsClass.Id, update);
        }
    }
}