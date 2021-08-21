using System;
using System.Collections.Generic;
using Pix_API.Providers.ContainersProviders;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using Pix_API.Providers.BaseClasses;
using System.Linq;
using Pix_API.Interfaces;

namespace Pix_API.Providers.StaticProviders
{
    public class ChampionshipProvider:SinglePoolStorageProvider<Championship>,IChampionshipsMetadataProvider
    {
        public ChampionshipProvider(DataSaver<Championship> saver) : base(saver)
        {
        }

        public List<Championship> GetAllChampionshipsForUser(int countryId, User authorize)
        {
            return storage.Where(s => s.CountryId == countryId).ToList();
        }
    }
}
