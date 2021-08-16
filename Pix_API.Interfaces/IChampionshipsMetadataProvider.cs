using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using Pix_API.Interfaces;
namespace Pix_API.Interfaces
{
    public interface IChampionshipsMetadataProvider
    {
        List<Championship> GetAllChampionshipsForUser(int countryId, User authorize);
    }
}
