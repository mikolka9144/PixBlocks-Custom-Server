using System;
using System.Collections.Generic;
using System.Linq;
using PixBlocks.Server.DataModels.DataModels;
namespace Pix_API.Providers.ContainersProviders
{
    public class StudentClassesProvider : MultiplePoolStorageProvider<StudentsClass>, IStudentClassProvider
    {
        private readonly IdAssigner idAssigner;
        private readonly IUserDatabaseProvider userDatabase;
        public StudentClassesProvider(DataSaver<List<StudentsClass>> saver,IUserDatabaseProvider userDatabase) : base(saver)
        {
            var id_list = storage.SelectMany((arg) => arg.Obj).Select((arg) => arg.Id.Value).ToList();
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
            return GetSingleObjectOrCreateNew(Id);
        }
        public StudentsClass GetStudentsClassById(int userId,int classId)
        {
            var AllQuestionResults = GetSingleObjectOrCreateNew(userId);
            return AllQuestionResults.FirstOrDefault(s => s.Id == classId);
        }

        public List<User> GetStudentsInClassForUser(int userId, int classId)
        {
            var AllQuestionResults = GetSingleObjectOrCreateNew(userId);
            return AllQuestionResults.Any((arg) => arg.Id.Value == classId) ?
            userDatabase.GetAllUsersBelongingToClass(classId) : null; 
        }

        public void RemoveClassForUser(StudentsClass studentsClass, int userId)
        {
            var AllQuestionResults = GetSingleObjectOrCreateNew(userId);
            AllQuestionResults.RemoveAll((obj) => obj.Id == studentsClass.Id);
        }
    }
    public interface IStudentClassProvider
    {
        List<StudentsClass> GetClassesForUser(int Id);
        StudentsClass GetStudentsClassById(int userId, int classId);
        void AddClassForUser(StudentsClass studentsClass, int userId);
        void EditClassForUser(StudentsClass studentsClass, int userId);
        void RemoveClassForUser(StudentsClass studentsClass, int userId);
        List<User> GetStudentsInClassForUser(int userID,int classID);
    }
}
