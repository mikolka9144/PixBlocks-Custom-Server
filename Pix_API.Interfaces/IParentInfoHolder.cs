using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Interfaces
{
	public interface IParentInfoHolder
	{
		void AddOrUpdateParentInfoForUser(ParentInfo parentInfo, int User_Id);
	}
}
