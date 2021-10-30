using System.Collections.Generic;
using Pix_API.Base.Disk;
using Pix_API.PixBlocks.Interfaces;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.PixBlocks.Disk
{
	public class UserCommentsProvider : MultiplePoolStorageProvider<Comment>, IUserCommentsProvider
	{
		public UserCommentsProvider(DataSaver<List<Comment>> saver)
			: base(saver)
		{
		}

		public void AddOrUpdateComment(Comment comment, int user_id)
		{
			AddObject(comment, user_id);
		}

		public List<Comment> GetAllCommentsForUser(int user_id)
		{
			return GetObjectOrCreateNew(user_id);
        }

        public void RemoveAllCommentsForUser(int userId)
        {
            RemoveObject(userId);
        }
    }
}
