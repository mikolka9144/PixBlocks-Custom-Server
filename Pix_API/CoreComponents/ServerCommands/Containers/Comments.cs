using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public List<Comment> GetAllCommentsFromStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, studentsClass.Id.Value))
            {
                var results = new List<Comment>();
                var users = studentClassProvider.GetStudentsInClassForUser(authorize.UserId, studentsClass.Id.Value);
                foreach (var user in users)
                {
                    results.AddRange(userCommentsProvider.GetAllCommentsForUser(user.Id.Value));
                }
                return results;
            }
            return null;
        }
        public Comment AddOrUpdateComment(Comment comment, AuthorizeData authorize)
        {
            var user = databaseProvider.GetUser(comment.UserID);
            if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, user.Student_studentsClassId.Value))
            {
                userCommentsProvider.AddOrUpdateComment(comment, comment.UserID);
            }
            return null;
        }
        public List<Comment> GetAllCommentsForUser(User user, AuthorizeData authorize)
        {
            var server_user = databaseProvider.GetUser(user.Id.Value);
            if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, server_user.Student_studentsClassId.Value))
            {
                return userCommentsProvider.GetAllCommentsForUser(user.Id.Value);
            }
            return null;
        }
    }
}
