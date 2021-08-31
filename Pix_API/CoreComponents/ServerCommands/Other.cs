using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public User UpdateOrDeleteUser(User user, AuthorizeData authorize)
        {
            if (authorize.UserId == user.Id)
            {
                if (user.IsDeleted)
                {
                    databaseProvider.RemoveUser(user.Id.Value);
                }
                else
                {
                    databaseProvider.UpdateUser(user);
                }
            }
            else
            {
                User user2 = databaseProvider.GetUser(user.Id.Value);
                if (security.IsClassBelongsToUser(authorize.UserId, user2.Student_studentsClassId.Value))
                {
                    if (user.IsDeleted)
                    {
                        databaseProvider.RemoveUser(user.Id.Value);
                    }
                    else
                    {
                        databaseProvider.UpdateUser(user);
                    }
                }
            }
            return user;
        }
        public List<Notification> GetNotificationForUser(string LanguageKey, AuthorizeData authorizeData)
        {
            User user = databaseProvider.GetUser(authorizeData.UserId);
            return new List<Notification> { notyficationProvider.GetNotyficationForUser(LanguageKey, user) };
        }
    }
}