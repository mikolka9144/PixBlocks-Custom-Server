using System.Collections.Generic;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using MongoDB.Driver;
using System;
using Pix_API.Providers.BaseClasses;
using System.Linq;

namespace Pix_API.Providers.MongoDB
{
    internal class MongoQuestionEditsProvider : IQuestionEditsProvider
    {
        private IMongoCollection<IdObjectBinder<EditedQuestionCode>> db;
        private IdAssigner assigner;

        public MongoQuestionEditsProvider(MongoClient client)
        {
            db = client.GetDatabase("Pix").GetCollection<IdObjectBinder<EditedQuestionCode>>("questionCodes");
            assigner = new IdAssigner(Convert.ToInt32(db.CountDocuments(s => true)));
        }
        public async void AddOrRemoveQuestionCode(EditedQuestionCode questionCode, int User_Id)
        {
            if(questionCode.ID is null)
            {
                questionCode.ID = assigner.NextEmptyId;
                var obj = new IdObjectBinder<EditedQuestionCode>(questionCode.ID.Value, questionCode);
                await db.InsertOneAsync(obj);
            }
            else
            {
                var obj = new IdObjectBinder<EditedQuestionCode>(questionCode.ID.Value, questionCode);
                await db.ReplaceOneAsync(s =>s.Id == questionCode.ID && s.Obj.UserId == User_Id,obj);
            }
        }

        public List<EditedQuestionCode> GetAllQuestionCodes(int UserId)
        {
            return db.FindSync(s => s.Obj.UserId == UserId).ToEnumerable().Select(s => s.Obj).ToList();
        }

        public EditedQuestionCode GetQuestionEditByGuid(int UserId, string guid, int? examId)
        {
            return db.FindSync(sim => sim.Obj.QuesionGuid == guid && sim.Obj.UserId == UserId && sim.Obj.ExamId == examId).FirstOrDefault()?.Obj;
        }
    }
}