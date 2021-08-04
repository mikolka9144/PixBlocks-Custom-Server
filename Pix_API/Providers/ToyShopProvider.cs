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
        private List<ToyShopData> toyShops;
        public ToyShopProvider(DataSaver<ToyShopData> saver)
        {
            this.saver = saver;
            toyShops = saver.LoadAll().Select(s => s.Obj).ToList();
        }

        public ToyShopData GetToyShop(int Id)
        {
            return toyShops.FirstOrDefault(s => s.UserID == Id);
        }

        public void SaveOrUpdateToyShop(ToyShopData toyShopData, int Id)
        {
            var binder = new IdObjectBinder<ToyShopData>(Id, toyShopData);
            toyShops.RemoveAll(s => s.UserID == Id);
            binder.Obj.UserID = Id;
            toyShops.Add(binder.Obj);
            saver.Save(binder);
        }
    }

    public interface IToyShopProvider
    {
        ToyShopData GetToyShop(int Id);
        void SaveOrUpdateToyShop(ToyShopData toyShopData, int Id);
    }
}
