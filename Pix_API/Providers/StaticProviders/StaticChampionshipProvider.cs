using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;

namespace Pix_API.Providers.StaticProviders
{
    public class StaticChampionshipProvider:IChampionshipsProvider
    {

        public List<Championship> GetAllChampionshipsForUser(int countryId, User authorize)
        {
            return new List<Championship>{
                new Championship()
                {
                    Name = "test championships",
                    Schedule = "test shedule",
                    Id = 1
                }
            };
        }
    }
}
