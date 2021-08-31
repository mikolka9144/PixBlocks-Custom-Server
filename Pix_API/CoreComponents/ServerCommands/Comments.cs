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
            if (!security.IsClassBelongsToUser(authorize.UserId, studentsClass.Id.Value)) return null;

            List<Comment> list = new List<Comment>();
            {
                foreach (User item in studentClassProvider.GetStudentsInClassForUser(authorize.UserId, studentsClass.Id.Value))
                {
                    list.AddRange(userCommentsProvider.GetAllCommentsForUser(item.Id.Value));
                }
                return list;
            }
        }

        public Comment AddOrUpdateComment(Comment comment, AuthorizeData authorize)
        {
            User user = databaseProvider.GetUser(comment.UserID);
            if (!security.IsClassBelongsToUser(authorize.UserId, user.Student_studentsClassId.Value)) return null;

            userCommentsProvider.AddOrUpdateComment(comment, comment.UserID);
            return comment;
        }

        public List<Comment> GetAllCommentsForUser(User user, AuthorizeData authorize)
        {
            User user2 = databaseProvider.GetUser(user.Id.Value);
            if (!security.IsClassBelongsToUser(authorize.UserId, user2.Student_studentsClassId.Value)) return null;

            return userCommentsProvider.GetAllCommentsForUser(user.Id.Value);
        }
    }
}
