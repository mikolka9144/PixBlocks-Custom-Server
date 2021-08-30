using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.Interfaces
{
	public interface IToyShopProvider
	{
		ToyShopData GetToyShop(int UserId);

		void SaveOrUpdateToyShop(ToyShopData toyShopData, int UserId);
	}
}
