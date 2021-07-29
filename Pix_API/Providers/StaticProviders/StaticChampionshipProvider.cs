using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;

namespace Pix_API.Providers.StaticProviders
{
    public class StaticChampionshipProvider:IChampionshipsProvider
    {
        public StaticChampionshipProvider()
        {
        }

        public List<Championship> GetAllChampionshipsForUser(int countryId, User authorize)
        {
            /*return new List<Championship>{
                new Championship()
                {
                    Name = "test championships",
                    Schedule = "test shedule"
                }
            };*/
            return new List<Championship>();
        }
    }
}
