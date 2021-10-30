using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.PixBlocks.Interfaces
{
	public interface IParentInfoHolder
	{
		void AddOrUpdateParentInfoForUser(ParentInfo parentInfo, int User_Id);
        void RemoveParentInfo(int userId);
    }
}
