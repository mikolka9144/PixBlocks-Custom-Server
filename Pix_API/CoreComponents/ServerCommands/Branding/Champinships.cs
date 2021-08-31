using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public List<Championship> GetActiveChampionshipsInCountry(int countryId, AuthorizeData authorize)
        {
            User user = databaseProvider.GetUser(authorize.UserId);
            return championshipsProvider.GetAllChampionshipsForUser(countryId, user);
        }

        public List<Championship> GetAllActiveChampionships(AuthorizeData authorize)
        {
            databaseProvider.GetUser(authorize.UserId);
            return championshipsProvider.GetAllChampionships();
        }
    }
}
