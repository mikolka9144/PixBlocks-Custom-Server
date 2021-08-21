using System.Collections.Generic;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using MongoDB.Driver;
using System;
using Pix_API.Providers.BaseClasses;
using System.Linq;

namespace Pix_API.Providers.MongoDB
{
    internal class MongoQuestionResultProvider : IQuestionResultsProvider
    {
        private IMongoCollection<IdObjectBinder<QuestionResult>> db;
        private IdAssigner assigner;

        public MongoQuestionResultProvider(MongoClient client)
        {
            db = client.GetDatabase("Pix").GetCollection<IdObjectBinder<QuestionResult>>("QuestionResults");
            assigner = new IdAssigner(Convert.ToInt32(db.CountDocuments(s => true)));
        }
        public async void AddOrUpdateQuestionResult(QuestionResult questionResult, int UserId)
        {
            if (questionResult.ID is null)
            {
                questionResult.ID = assigner.NextEmptyId;
                var obj = new IdObjectBinder<QuestionResult>(questionResult.ID.Value, questionResult);
                await db.InsertOneAsync(obj);
            }
            else
            {
                var obj = new IdObjectBinder<QuestionResult>(questionResult.ID.Value, questionResult);
                await db.ReplaceOneAsync(s => s.Id == questionResult.ID && s.Obj.UserID == UserId, obj);
            }
        }

        public List<QuestionResult> GetAllQuestionsReultsForUser(int UserId)
        {
            return db.FindSync(s => s.Obj.UserID == UserId).ToEnumerable().Select(s => s.Obj).ToList();

        }
    }
}