using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public StudentsClass EditStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            studentClassProvider.EditClassForUser(studentsClass, authorize.UserId);
            return null;
        }
        public StudentsClass DeleteStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            studentClassProvider.RemoveClassForUser(studentsClass, authorize.UserId);
            return null;
        }
        public List<User> GetAllStudentsInClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            var result = studentClassProvider.GetStudentsInClassForUser(authorize.UserId, studentsClass.Id.Value);
            return result;
        }

        public List<QuestionResult> GetAllResultsForStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, studentsClass.Id.Value))
            {
                var results = new List<QuestionResult>();
                var users = studentClassProvider.GetStudentsInClassForUser(authorize.UserId, studentsClass.Id.Value);
                foreach (var user in users)
                {
                    results.AddRange(questionResultsProvider.GetAllQuestionsReultsForUser(user.Id.Value));
                }
                return results;
            }
            return null;
        }
        public List<StudentsClass> GetAllStudentsClasses(int teacherID, AuthorizeData authorize)
        {
            return studentClassProvider.GetClassesForUser(authorize.UserId);
        }
        public StudentsClass AddStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            studentsClass.TeacherID = authorize.UserId;
            studentClassProvider.AddClassForUser(studentsClass, authorize.UserId);
            return studentsClass;
        }
    }
}
