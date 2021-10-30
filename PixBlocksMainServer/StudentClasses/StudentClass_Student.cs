using System;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.PixBlocks.MainServer
{
    public partial class Main_Logic
    {
        public StudentsClass GetStudentsClassById(int id, AuthorizeData authorize)
        {
            return studentClassProvider.GetStudentsClassByGlobalId(id);
        }

        public User AddUserToStudentsClass(StudentsClass studentsClass, User newStudent, AuthorizeData authorize)
        {
            User user = databaseProvider.GetUser(authorize.UserId);
            user.Student_studentsClassId = studentsClass.Id;
            user.Student_isStudent = true;
            user.Student_isAssignedToStudentsClass = true;
            user.Student_isAcceptedToStudentsClass = false;
            databaseProvider.UpdateUser(user);
            return user;
        }

        public User RemoveUserFromStudentsClass(User userToRemove, AuthorizeData authorize) 
        {
            User user = databaseProvider.GetUser(authorize.UserId);
            return serverUtills.RemoveUserFromClass(user); 
        }
    }
}