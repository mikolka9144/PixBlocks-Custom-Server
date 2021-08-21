using System;
using System.Collections.Generic;
using System.Linq;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
namespace Pix_API.Providers.ContainersProviders
{
    public class StudentClassesProvider : MultiplePoolStorageProvider<StudentsClass>, IStudentClassProvider
    {
        private readonly IdAssigner idAssigner;
        private readonly IUserDatabaseProvider userDatabase;
        public StudentClassesProvider(DataSaver<List<StudentsClass>> saver,IUserDatabaseProvider userDatabase) : base(saver)
        {
            var id_list = storage.SelectMany((arg) => arg).Select((arg) => arg.Id.Value).ToList();
            idAssigner = new IdAssigner(id_list);
            this.userDatabase = userDatabase;
        }

        public void AddClassForUser(StudentsClass studentsClass, int userId)
        {
            studentsClass.Id = idAssigner.NextEmptyId;
            AddObject(studentsClass, userId);
        }

        public void EditClassForUser(StudentsClass studentsClass, int userId)
        {
            AddOrUpdateObject(studentsClass, userId, (a, b) => a.Id == b.Id);
        }

        public List<StudentsClass> GetClassesForUser(int Id)
        {
            return GetObjectOrCreateNew(Id);
        }

        public StudentsClass GetStudentsClassByGlobalId(int classId)
        {
            return storage.SelectMany(s => s).FirstOrDefault(s => s.Id == classId);
        }

        public StudentsClass GetStudentsClassById(int userId,int classId)
        {
            var AllQuestionResults = GetObjectOrCreateNew(userId);
            return AllQuestionResults.FirstOrDefault(s => s.Id == classId);
        }

        public List<User> GetStudentsInClassForUser(int userId, int classId)
        {
            var AllQuestionResults = GetObjectOrCreateNew(userId);
            return AllQuestionResults.Any((arg) => arg.Id.Value == classId) ?
            userDatabase.GetAllUsersBelongingToClass(classId) : null; 
        }

        public void RemoveClassForUser(StudentsClass studentsClass, int userId)
        {
            var AllQuestionResults = GetObjectOrCreateNew(userId);
            AllQuestionResults.RemoveAll((obj) => obj.Id == studentsClass.Id);
        }
    }

}
