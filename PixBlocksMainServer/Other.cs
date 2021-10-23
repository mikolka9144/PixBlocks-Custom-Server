using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public User UpdateOrDeleteUser(User user, AuthorizeData authorize)
        {
            User server_user = databaseProvider.GetUser(user.Id.Value);
            var IsAuthValid = user.Id == authorize.UserId;
            if (IsAuthValid || serverUtills.IsClassBelongsToUser(authorize.UserId, server_user.Student_studentsClassId.Value))
            {
                if (user.IsDeleted) { serverUtills.RemoveUserData(user); return null; }
                if (user.ChampionshipId != server_user.ChampionshipId)
                {
                    user.ChampionshipDateAdd = DateTime.Now;
                }
                databaseProvider.UpdateUser(user);

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