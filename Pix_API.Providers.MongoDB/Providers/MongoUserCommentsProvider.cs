using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.Providers.MongoDB.Providers
{
	internal class MongoUserCommentsProvider : MongoIdSaver_Base<MongoIdBinder<Comment>>, IUserCommentsProvider
	{
		public MongoUserCommentsProvider(IMongoDatabase client, IMongoCollection<LastIndexHolder> index_collection)
			: base(client, index_collection, "comments")
		{
		}

		public async void AddOrUpdateComment(Comment comment, int user_id)
		{
			Func<Comment, bool> check = (Comment sim) => sim.CategoryGuid == comment.CategoryGuid && sim.ExamID == comment.ExamID && sim.UserID == user_id;
			if ((await db.FindAsync((MongoIdBinder<Comment> s) => check(s.Obj))).Any())
			{
				UpdateDefinition<MongoIdBinder<Comment>> update = Builders<MongoIdBinder<Comment>>.Update.Set((MongoIdBinder<Comment> s) => s.Obj.Text, comment.Text);
				await db.UpdateOneAsync((MongoIdBinder<Comment> s) => check(s.Obj), update);
			}
			else
			{
				await db.InsertOneAsync(new MongoIdBinder<Comment>(comment));
			}
		}

		public List<Comment> GetAllCommentsForUser(int user_id)
		{
			return (from s in db.FindSync((MongoIdBinder<Comment> sim) => sim.Obj.UserID == user_id).ToEnumerable()
				select s.Obj).ToList();
		}
	}
}
