using Pix_API.Interfaces;
using Pix_API.Providers.BaseClasses;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Providers
{
	public class DiskParentInfoProvider : SinglePoolStorageProvider<ParentInfo>, IParentInfoHolder
	{
		public DiskParentInfoProvider(DataSaver<ParentInfo> saver)
			: base(saver)
		{
		}

		public void AddOrUpdateParentInfoForUser(ParentInfo parentInfo, int User_Id)
		{
			AddOrUpdateSingleObject(parentInfo, User_Id);
        }

        public void RemoveParentInfo(int userId)
        {
            RemoveObject(userId);
        }
    }
}
