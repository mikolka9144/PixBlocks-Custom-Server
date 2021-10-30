using System.Collections.Generic;
using MongoDB.Driver;
using Pix_API.Base.MongoDB;
using Pix_API.PixBlocks.Interfaces;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;

namespace Pix_API.PixBlocks.MongoDB.Providers
{
	internal class MongoStudentClassExamsProvider : MongoIdSaver_Base<ServerExam>, IStudentClassExamsProvider
	{
		public MongoStudentClassExamsProvider(IMongoDatabase client, IMongoCollection<LastIndexHolder> index_collection)
			: base(client, index_collection, "exams")
		{
		}

		public async void AddExam(ServerExam serverExam)
		{
			serverExam.set_Id(assigner.NextEmptyId);
			await db.InsertOneAsync(serverExam);
		}

		public void AddQuestionInExam(ExamQuestion question, int exam_id)
		{
			UpdateDefinition<ServerExam> update = Builders<ServerExam>.Update.AddToSet((ServerExam s) => s.questions, question);
			db.UpdateOne((ServerExam sim) => sim.Id == exam_id, update);
		}

		public List<ServerExam> GetAllExamsInClass(int class_id)
		{
			return db.FindSync((ServerExam sim) => sim.Exam_metadata.StudentsClassId == class_id).ToList();
		}

		public List<ServerExam> GetChampionshipExams(int championshipId)
		{
			return db.FindSync((ServerExam sim) => sim.Exam_metadata.ChampionshipId == (int?)championshipId).ToList();
		}

		public ServerExam GetExam(int exam_id)
		{
			return db.FindSync((ServerExam sim) => sim.Id == exam_id).FirstOrDefault();
		}

		public async void RemoveAllExamsInClass(int class_id)
		{
			await db.DeleteManyAsync((ServerExam sim) => sim.Exam_metadata.StudentsClassId == class_id);
		}

		public async void RemoveQuestionInExam(ExamQuestion examQuestion, int examId)
		{
			ServerExam serverExam = db.FindSync((ServerExam sim) => sim.Id == examId).FirstOrDefault();
			serverExam.questions.RemoveAll((ExamQuestion s) => s.QuestionGuid == examQuestion.QuestionGuid);
			UpdateDefinition<ServerExam> update = Builders<ServerExam>.Update.Set((ServerExam s) => s.questions, serverExam.questions);
			await db.UpdateOneAsync((ServerExam sim) => sim.Id == examId, update);
		}

		public async void UpdateExam(ServerExam server_exam)
		{
			await db.ReplaceOneAsync((ServerExam s) => s.Id == server_exam.Id, server_exam);
		}
	}
}
