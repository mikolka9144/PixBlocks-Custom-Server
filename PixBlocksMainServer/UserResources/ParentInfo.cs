using System;
using System.Collections.Generic;
using System.Linq;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using PixBlocks.Server.DataModels.DataModels.DBModels;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using PixBlocks.Server.DataModels.DataModels.Woocommerce;

namespace Pix_API.CoreComponents.ServerCommands
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
