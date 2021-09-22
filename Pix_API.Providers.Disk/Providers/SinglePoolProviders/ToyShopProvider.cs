using Pix_API.Interfaces;
using Pix_API.Providers.BaseClasses;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.Providers
{
	public class ToyShopProvider : SinglePoolStorageProvider<ToyShopData>, IToyShopProvider
	{
		public ToyShopProvider(DataSaver<ToyShopData> saver)
			: base(saver)
		{
		}

		public ToyShopData GetToyShop(int Id)
		{
			return GetSingleObject(Id);
        }

        public void RemoveToyShop(int UserId)
        {
            RemoveObject(UserId);
        }

        public void SaveOrUpdateToyShop(ToyShopData toyShopData, int Id)
		{
			toyShopData.UserID = Id;
			AddOrUpdateSingleObject(toyShopData, Id);
		}
	}
}
