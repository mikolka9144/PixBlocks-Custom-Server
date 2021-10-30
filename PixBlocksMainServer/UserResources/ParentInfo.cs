
using PixBlocks.Server.DataModels.DataModels;

using PixBlocks.Server.DataModels.DataModels.DBModels;


namespace Pix_API.PixBlocks.MainServer
{
	public partial class Main_Logic 
	{
		public AccountActivationStatus IsParentEmailActivated(int childID)
		{
			return new AccountActivationStatus
			{
				IsEmailActivated = true
			};
		}

		public ParentInfo AddOrUpdateParentInfo(ParentInfo parentInfo, AuthorizeData authorize)
		{
			parentInfoProvider.AddOrUpdateParentInfoForUser(parentInfo, authorize.UserId);
			return parentInfo;
		}
	}
}
