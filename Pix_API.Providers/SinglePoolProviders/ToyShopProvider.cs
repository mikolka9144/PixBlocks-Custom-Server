using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using Pix_API.Providers.ContainersProviders;
using System.Linq;
using Pix_API.Providers.BaseClasses;
using Pix_API.Interfaces;

namespace Pix_API.Providers
{
    public class ToyShopProvider:SinglePoolStorageProvider<ToyShopData>,IToyShopProvider
    {
        public ToyShopProvider(DataSaver<ToyShopData> saver):base(saver)
        {
        }

        public ToyShopData GetToyShop(int Id) => GetSingleObjectOrCreateNew(Id);

        public void SaveOrUpdateToyShop(ToyShopData toyShopData, int Id)
        {
            toyShopData.UserID = Id;
            AddOrUpdateSingleObject(toyShopData, Id);
        }
    }


}
