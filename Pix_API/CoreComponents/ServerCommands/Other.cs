using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public User UpdateOrDeleteUser(User user, AuthorizeData authorize)
        {
            if (authorize.UserId == user.Id)
            {
                databaseProvider.UpdateUser(user);
            }
            else
            {
                var user_in_question = databaseProvider.GetUser(user.Id.Value);
                var userBelongsToTeacher = studentClassProvider.IsClassBelongsToUser(authorize.UserId,
                    user_in_question.Student_studentsClassId.Value);
                if (userBelongsToTeacher)
                {
                    databaseProvider.UpdateUser(user);
                }
            }
            return null;
        }

        public List<Notification> GetNotificationForUser(string LanguageKey, AuthorizeData authorizeData)
        {
            var user = databaseProvider.GetUser(authorizeData.UserId);
            return new List<Notification>{
                notyficationProvider.GetNotyficationForUser(LanguageKey, user)
                };
        }
        public List<Championship> GetActiveChampionshipsInCountry(int countryId, AuthorizeData authorize)
        {
            var user = databaseProvider.GetUser(authorize.UserId);
            return championshipsProvider.GetAllChampionshipsForUser(countryId, user);
        }
    }
}
