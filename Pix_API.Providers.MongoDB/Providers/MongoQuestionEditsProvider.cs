using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Pix_API.Interfaces;
using Pix_API.Providers.BaseClasses;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.Providers.MongoDB.Providers
{
	internal class MongoQuestionEditsProvider : MongoIdSaver_Base<IdObjectBinder<EditedQuestionCode>>, IQuestionEditsProvider
	{
		public MongoQuestionEditsProvider(IMongoDatabase client, IMongoCollection<LastIndexHolder> index_collection)
			: base(client, index_collection, "questonEdits")
		{
			var keys = Builders<IdObjectBinder<EditedQuestionCode>>.IndexKeys.Ascending((IdObjectBinder<EditedQuestionCode> s) => s.Obj.UserId);
			db.Indexes.CreateOne(new CreateIndexModel<IdObjectBinder<EditedQuestionCode>>(keys));
		}

		public async void AddOrRemoveQuestionCode(EditedQuestionCode questionCode, int User_Id)
		{
			if (!questionCode.ID.HasValue)
			{
				questionCode.ID = assigner.NextEmptyId;
				IdObjectBinder<EditedQuestionCode> document = new IdObjectBinder<EditedQuestionCode>(questionCode.ID.Value, questionCode);
				await db.InsertOneAsync(document);
			}
			else
			{
				IdObjectBinder<EditedQuestionCode> replacement = new IdObjectBinder<EditedQuestionCode>(questionCode.ID.Value, questionCode);
				await db.ReplaceOneAsync((IdObjectBinder<EditedQuestionCode> s) => s.Id == questionCode.ID && s.Obj.UserId == User_Id, replacement);
			}
		}

		public List<EditedQuestionCode> GetAllQuestionCodes(int UserId)
		{
			return (from s in db.FindSync((IdObjectBinder<EditedQuestionCode> s) => s.Obj.UserId == UserId).ToEnumerable()
				select s.Obj).ToList();
		}

		public EditedQuestionCode GetQuestionEditByGuid(int UserId, string guid, int? examId)
		{
			return db.FindSync((IdObjectBinder<EditedQuestionCode> sim) => sim.Obj.UserId == UserId && sim.Obj.QuesionGuid == guid && sim.Obj.ExamId == examId).FirstOrDefault()?.Obj;
        }

        public async void RemoveQuestionCodesForUser(int userId)
        {
            await db.DeleteOneAsync(s => s.Id == userId);
        }
    }
}
