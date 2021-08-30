using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.Interfaces
{
	public interface IUserCommentsProvider
	{
		List<Comment> GetAllCommentsForUser(int user_id);

		void AddOrUpdateComment(Comment comment, int user_id);
	}
}
