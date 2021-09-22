using System;
using System.Linq;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;

namespace Pix_API
{
    public class ServerUtills
    {
        private IStudentClassProvider classProvider;
        private IUserDatabaseProvider databaseProvider;
        private readonly IToyShopProvider toyShopProvider;
        private readonly IParentInfoHolder parentInfoHolder;
        private readonly IStudentClassExamsProvider classExamsProvider;
        private readonly ISchoolProvider schoolProvider;
        private readonly IQuestionResultsProvider questionResultsProvider;
        private readonly IQuestionEditsProvider questionEditsProvider;
        private readonly IUserCommentsProvider commentsProvider;

        public ServerUtills(IStudentClassProvider studentClass,IUserDatabaseProvider databaseProvider,IToyShopProvider toyShopProvider, IParentInfoHolder parentInfoHolder,IStudentClassExamsProvider classExamsProvider,ISchoolProvider schoolProvider,IQuestionResultsProvider questionResultsProvider,IQuestionEditsProvider questionEditsProvider,IUserCommentsProvider commentsProvider)
        {
            classProvider = studentClass;
            this.databaseProvider = databaseProvider;
            this.toyShopProvider = toyShopProvider;
            this.parentInfoHolder = parentInfoHolder;
            this.classExamsProvider = classExamsProvider;
            this.schoolProvider = schoolProvider;
            this.questionResultsProvider = questionResultsProvider;
            this.questionEditsProvider = questionEditsProvider;
            this.commentsProvider = commentsProvider;
        }



        public User RemoveUserFromClass(User user)
        {

            user.Student_studentsClassId = null;
            user.Student_isStudent = false;
            user.Student_isAssignedToStudentsClass = false;
            user.Student_isAcceptedToStudentsClass = null;
            databaseProvider.UpdateUser(user);
            return user;
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

        public void RemoveUserData(User user)//TODO Not tested
        {
            var UserId = user.Id.Value;
            databaseProvider.RemoveUser(UserId);
            toyShopProvider.RemoveToyShop(UserId);
            parentInfoHolder.RemoveParentInfo(UserId);
            questionEditsProvider.RemoveQuestionCodesForUser(UserId);
            questionResultsProvider.RemoveAllQuestionResultsForUser(UserId);
            schoolProvider.RemoveSchool(UserId);
            if (user.Teacher_isTeacher)
            {
                commentsProvider.RemoveAllCommentsForUser(UserId);
                classProvider.GetClassesForUser(UserId).ForEach(RemoveClass);
            }
        }
        public void RemoveClass(StudentsClass studentsClass)
        {
            classExamsProvider.RemoveAllExamsInClass(studentsClass.Id.Value);
            foreach (var student in classProvider.GetStudentsInClassForUser(studentsClass.TeacherID, studentsClass.Id.Value))
            {
                if (student.Email == null) RemoveUserData(student);
                else RemoveUserFromClass(student);
            }
            classProvider.RemoveClass(studentsClass);
        }
    }
}
