using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Pix_API.Interfaces;
using Pix_API.Providers.BaseClasses;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.Providers.MongoDB
{
	internal class MongoQuestionResultProvider : MongoIdSaver_Base<IdObjectBinder<QuestionResult>>, IQuestionResultsProvider
	{
		public MongoQuestionResultProvider(IMongoDatabase client, IMongoCollection<LastIndexHolder> index_collection)
			: base(client, index_collection, "questionResults")
		{
		}

		public async void AddOrUpdateQuestionResult(QuestionResult questionResult, int UserId)
		{
			if (!questionResult.ID.HasValue)
			{
				questionResult.ID = assigner.NextEmptyId;
				IdObjectBinder<QuestionResult> document = new IdObjectBinder<QuestionResult>(questionResult.ID.Value, questionResult);
				await db.InsertOneAsync(document);
			}
			else
			{
				IdObjectBinder<QuestionResult> replacement = new IdObjectBinder<QuestionResult>(questionResult.ID.Value, questionResult);
				await db.ReplaceOneAsync((IdObjectBinder<QuestionResult> s) => (int?)s.Id == questionResult.ID && s.Obj.UserID == UserId, replacement);
			}
		}

		public List<QuestionResult> GetAllQuestionsReultsForUser(int UserId)
		{
			return (from s in db.FindSync((IdObjectBinder<QuestionResult> s) => s.Obj.UserID == UserId).ToEnumerable()
				select s.Obj).ToList();
		}
	}
}
