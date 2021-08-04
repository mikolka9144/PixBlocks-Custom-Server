using System;
using System.Collections.Generic;
using System.Linq;
using PixBlocks.Server.DataModels.DataModels;
namespace Pix_API.Providers.ContainersProviders
{
    public class StudentClassesProvider : Storage_Provider<StudentsClass>, IStudentClassProvider
    {

        private int _next_empty_id;
        private readonly IUserDatabaseProvider userDatabase;

        private int NextEmptyId {
            get => _next_empty_id++;
        }
        public StudentClassesProvider(DataSaver<List<StudentsClass>> saver,IUserDatabaseProvider userDatabase) : base(saver)
        {
            var next_empty_id = storage.SelectMany((arg) => arg.Obj).Select((arg) => arg.Id).Max();
            if (next_empty_id.HasValue) _next_empty_id = next_empty_id.Value + 1;
            this.userDatabase = userDatabase;
        }

        public void AddClassForUser(StudentsClass studentsClass, int userId)
        {
            var AllQuestionResults = GetUserObjectOrCreateNew(userId);
            studentsClass.Id = NextEmptyId;
            AddObject(studentsClass, userId);
        }

        public void EditClassForUser(StudentsClass studentsClass, int userId)
        {
            AddOrUpdateObject(studentsClass, userId, (a, b) => a.Id == b.Id);
        }

        public List<StudentsClass> GetClassesForUser(int Id)
        {
            return GetAllObjectsForUser(Id);
        }
        public StudentsClass GetStudentsClassById(int userId,int classId)
        {
            var AllQuestionResults = GetUserObjectOrCreateNew(userId);
            return AllQuestionResults.Obj.FirstOrDefault(s => s.Id == classId);
        }

        public List<User> GetStudentsInClassForUser(int userId, int classId)
        {
            var AllQuestionResults = GetUserObjectOrCreateNew(userId);
            return AllQuestionResults.Obj.Any((arg) => arg.Id.Value == classId) ?
            userDatabase.GetAllUsersBelongingToClass(classId) : null; 
        }

        public void RemoveClassForUser(StudentsClass studentsClass, int userId)
        {
            var AllQuestionResults = GetUserObjectOrCreateNew(userId);
            AllQuestionResults.Obj.RemoveAll((obj) => obj.Id == studentsClass.Id);
        }

        public bool IsClassBelongsToUser(int userId, int classId)
        {
            var AllQuestionResults = GetUserObjectOrCreateNew(userId);
            return AllQuestionResults.Obj.Any(s => s.Id == classId);
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
        bool IsClassBelongsToUser(int userId, int classId);
    }
}
