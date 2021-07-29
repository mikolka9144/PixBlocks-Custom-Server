using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;
namespace Pix_API.Providers
{
    public interface IChampionshipsProvider
    {
        List<Championship> GetAllChampionshipsForUser(int countryId, User authorize);
    }
}
