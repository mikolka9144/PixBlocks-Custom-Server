using System;
using System.Collections.Generic;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using MongoDB.Driver;
using Pix_API.Providers.BaseClasses;
using System.Linq.Expressions;
using System.Linq;

namespace Pix_API.Providers.MongoDB
{
    internal class MongoUserCommentsProvider : IUserCommentsProvider
    {
        private IMongoCollection<MongoIdBinder<Comment>> db;
        private IdAssigner assigner;

        public MongoUserCommentsProvider(MongoClient client)
        {
            db = client.GetDatabase("Pix").GetCollection<MongoIdBinder<Comment>>("classes");
            assigner = new IdAssigner(Convert.ToInt32(db.CountDocuments(sim => true)));
        }
        public async void AddOrUpdateComment(Comment comment, int user_id)
        {
            Func<Comment, bool> check = (Comment sim) => sim.CategoryGuid == comment.CategoryGuid && sim.ExamID == comment.ExamID && sim.UserID == user_id;
            var HasComment = await db.FindAsync(s => check(s.Obj));
            if (HasComment.Any())
            {
                var update = Builders<MongoIdBinder<Comment>>.Update.Set(s => s.Obj.Text, comment.Text);
                await db.UpdateOneAsync(s => check(s.Obj),update);
            }
            else
            {
                await db.InsertOneAsync(new MongoIdBinder<Comment>(comment));
            }

        }

        public List<Comment> GetAllCommentsForUser(int user_id)
        {
            return db.FindSync(sim => sim.Obj.UserID == user_id).ToEnumerable().Select(s => s.Obj).ToList();
        }
    }
}