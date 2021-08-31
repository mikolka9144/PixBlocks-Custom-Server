using System;
using System.Linq;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;

namespace Pix_API
{
    public class SecurityChecks
    {
        private IStudentClassProvider classProvider;
        private IUserDatabaseProvider databaseProvider;

        public SecurityChecks(IStudentClassProvider studentClass,IUserDatabaseProvider databaseProvider)
        {
            classProvider = studentClass;
            this.databaseProvider = databaseProvider;
        }
        public bool IsExamCreatedByUser(Exam server_exam, int UserId)
        { 
            int studentsClassId = server_exam.StudentsClassId;
            return IsClassBelongsToUser(UserId, studentsClassId);
        }

        public bool IsClassBelongsToUser(int userId, int classId)
        {
            return classProvider.GetClassesForUser(userId).Any(s => s.Id == classId);
        }
        public bool IsAuthorizeValid(AuthorizeData authorize)
        {
            User user = databaseProvider.GetUser(authorize.UserId);
            bool num = authorize.PasswordMD5 == user.Md5Password;
            bool flag = authorize.ExplicitPassword == user.Student_explicitPassword && user.Student_isStudent;
            return num || flag;
        }
    }
}
