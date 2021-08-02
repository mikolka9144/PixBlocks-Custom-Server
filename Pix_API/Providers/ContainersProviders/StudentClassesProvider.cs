using System;
using System.Collections.Generic;
using System.Linq;
using PixBlocks.Server.DataModels.DataModels;
namespace Pix_API.Providers.ContainersProviders
{
    public class StudentClassesProvider : Storage_Provider<StudentsClass>, IStudentClassProvider
    {
        private readonly IStudentClassBinding classBinding;

        public StudentClassesProvider(DataSaver<List<StudentsClass>> saver, IStudentClassBinding classBinding) : base(saver)
        {
            this.classBinding = classBinding;
        }

        public void AddClassForUser(StudentsClass studentsClass, int userId)
        {
                var AllQuestionResults = GetUserObjectOrCreateNew(userId);
                studentsClass.Id = AllQuestionResults.Obj.Count;
                AddObject(studentsClass, userId);
                classBinding.Add(userId, studentsClass.Id.Value);
        }

        public void EditClassForUser(StudentsClass studentsClass, int userId)
        {
            AddOrUpdateObject(studentsClass, userId, (a, b) => a.Id == b.Id);
        }

        public List<StudentsClass> GetClassesForUser(int Id)
        {
            return GetAllObjectsForUser(Id);
        }
        public StudentsClass GetStudentsClassForUser(int userId,int classId)
        {
            var AllQuestionResults = GetUserObjectOrCreateNew(userId);
            return AllQuestionResults.Obj.FirstOrDefault(s => s.Id == classId);
        }

        public List<int> GetStudentsClassForUserInClassOfUser(int userId, int classId)
        {
            var AllQuestionResults = GetUserObjectOrCreateNew(userId);
            return classBinding.Get(userId,classId).students_ids;
        }

        public void RemoveClassForUser(StudentsClass studentsClass, int userId)
        {
            var AllQuestionResults = GetUserObjectOrCreateNew(userId);
            AllQuestionResults.Obj.RemoveAll((obj) => obj.Id == studentsClass.Id);
            classBinding.Remove(userId, studentsClass.Id.Value );
        }
    }
    public interface IStudentClassProvider
    {
        List<StudentsClass> GetClassesForUser(int Id);
        StudentsClass GetStudentsClassForUser(int userId, int classId);
        void AddClassForUser(StudentsClass studentsClass, int userId);
        void EditClassForUser(StudentsClass studentsClass, int userId);
        void RemoveClassForUser(StudentsClass studentsClass, int userId);
        List<int> GetStudentsClassForUserInClassOfUser(int userID,int classID);
    }
}
