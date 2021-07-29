using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using Pix_API.Providers.ContainersProviders;
using System.Linq;
namespace Pix_API.Providers
{
    public class ToyShopProvider:IToyShopProvider
    {
        private readonly DataSaver<ToyShopData> saver;
        private List<IdObjectBinder<ToyShopData>> toyShops;
        public ToyShopProvider(DataSaver<ToyShopData> saver)
        {
            this.saver = saver;
            toyShops = saver.LoadAll();
        }

        public ToyShopData GetToyShop(int Id)
        {
            return toyShops.FirstOrDefault(s => s.Id == Id).Obj;
        }

        public void SaveOrUpdateToyShop(ToyShopData toyShopData, int Id)
        {
            var binder = new IdObjectBinder<ToyShopData>(Id, toyShopData);
            toyShops.RemoveAll(s => s.Id == Id);
            toyShops.Add(binder);
            saver.Save(binder);
        }
    }

    public interface IToyShopProvider
    {
        ToyShopData GetToyShop(int Id);
        void SaveOrUpdateToyShop(ToyShopData toyShopData, int Id);
    }
}
