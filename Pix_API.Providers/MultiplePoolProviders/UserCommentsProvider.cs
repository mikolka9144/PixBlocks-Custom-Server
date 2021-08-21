using System;
using System.Collections.Generic;
using Pix_API.Interfaces;
using Pix_API.Providers.ContainersProviders;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
namespace Pix_API.Providers.MultiplePoolProviders
{
    public class UserCommentsProvider : MultiplePoolStorageProvider<Comment>,IUserCommentsProvider
    {
        public UserCommentsProvider(DataSaver<List<Comment>> saver) : base(saver)
        {
        }

        public void AddOrUpdateComment(Comment comment, int user_id)
        {
            AddObject(comment, user_id);//TODO
        }

        public List<Comment> GetAllCommentsForUser(int user_id)
        {
            return GetObjectOrCreateNew(user_id);
        }
    }


}
