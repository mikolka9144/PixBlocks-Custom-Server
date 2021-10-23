using System;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.DBModels;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public GetToyShopDataResult GetUserToysShopInfo(User user, AuthorizeData authorize)
        {
            var toyShop = toyShopProvider.GetToyShop(authorize.UserId);
            if (toyShop != null)
            {
                return new GetToyShopDataResult
                {
                    IsToyShopExist = true,
                    ToyShopData = toyShop
                };
            }
            return new GetToyShopDataResult
            {
                IsToyShopExist = false
            };
        }

        public ToyShopData AddOrUpdateToyShopInfo(ToyShopData toyShopData, AuthorizeData authorize)
        {
            toyShopProvider.SaveOrUpdateToyShop(toyShopData, authorize.UserId);
            return toyShopData;
        }
    }
}
