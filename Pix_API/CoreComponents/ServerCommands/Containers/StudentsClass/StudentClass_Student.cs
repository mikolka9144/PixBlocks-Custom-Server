using System;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public StudentsClass GetStudentsClassById(int id, AuthorizeData authorize)
        {
            return studentClassProvider.GetStudentsClassByGlobalId(id);
        }
        public User AddUserToStudentsClass(StudentsClass studentsClass, User newStudent, AuthorizeData authorize)
        {
            var user = databaseProvider.GetUser(authorize.UserId);
            user.Student_studentsClassId = studentsClass.Id;
            user.Student_isStudent = true;
            user.Student_isAssignedToStudentsClass = true;
            user.Student_isAcceptedToStudentsClass = true;//TODO
            databaseProvider.UpdateUser(user);
            return user;
        }
        public User RemoveUserFromStudentsClass(User userToRemove, AuthorizeData authorize)
        {
            var user = databaseProvider.GetUser(authorize.UserId);
            user.Student_studentsClassId = null;
            user.Student_isStudent = false;
            user.Student_isAssignedToStudentsClass = false;
            user.Student_isAcceptedToStudentsClass = null;
            databaseProvider.UpdateUser(user);
            return user;
        }
    }
}
