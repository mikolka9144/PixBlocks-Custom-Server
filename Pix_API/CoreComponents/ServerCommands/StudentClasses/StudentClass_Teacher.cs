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
            if (!security.IsClassBelongsToUser(authorize.UserId, studentsClass.Id.Value)) return null;

            studentClassProvider.EditClassForUser(studentsClass, authorize.UserId);
            return null;
        }

        public StudentsClass DeleteStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            if (!security.IsClassBelongsToUser(authorize.UserId, studentsClass.Id.Value)) return null;

            foreach (User item in databaseProvider.GetAllUsersBelongingToClass(studentsClass.Id.Value))
            {
                if (item.Email == null)
                {
                    databaseProvider.RemoveUser(item.Id.Value);
                    continue;
                }
                item.Student_isStudent = false;
                item.Student_studentsClassId = null;
                databaseProvider.UpdateUser(item);
            }
            studentClassExamsProvider.RemoveAllExamsInClass(studentsClass.Id.Value);
            studentClassProvider.RemoveClassForUser(studentsClass, authorize.UserId);
            return null;
        }

        public List<User> GetAllStudentsInClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            if (!security.IsClassBelongsToUser(authorize.UserId, studentsClass.Id.Value)) return null;

            return studentClassProvider.GetStudentsInClassForUser(authorize.UserId, studentsClass.Id.Value);
        }

        public List<QuestionResult> GetAllResultsForStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            if (!security.IsClassBelongsToUser(authorize.UserId, studentsClass.Id.Value)) return null;

            List<QuestionResult> list = new List<QuestionResult>();
            {
                foreach (User item in studentClassProvider.GetStudentsInClassForUser(authorize.UserId, studentsClass.Id.Value))
                {
                    list.AddRange(questionResultsProvider.GetAllQuestionsReultsForUser(item.Id.Value));
                }
                return list;
            }
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
