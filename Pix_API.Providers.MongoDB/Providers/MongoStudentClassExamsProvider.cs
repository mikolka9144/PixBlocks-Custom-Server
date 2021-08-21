using System;
using System.Collections.Generic;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;
using MongoDB.Driver;

namespace Pix_API.Providers.MongoDB
{
    internal class MongoStudentClassExamsProvider : IStudentClassExamsProvider
    {
        private IMongoCollection<ServerExam> db;
        private IdAssigner assigner;

        public MongoStudentClassExamsProvider(MongoClient client)
        {
            db = client.GetDatabase("Pix").GetCollection<ServerExam>("exams");
            assigner = new IdAssigner(Convert.ToInt32(db.CountDocuments(sim => true)));
        }
        public async void AddExam(ServerExam serverExam)
        {
            serverExam.set_Id(assigner.NextEmptyId);
            await db.InsertOneAsync(serverExam);
        }

        public void AddQuestionInExam(ExamQuestion question, int exam_id)
        {
            var update = Builders<ServerExam>.Update.AddToSet(s => s.questions, question);
            db.UpdateOne(sim => sim.Id == exam_id,update);
        }

        public List<ServerExam> GetAllExamsInClass(int class_id)
        {
            var pointer = db.FindSync(sim => sim.Exam_metadata.StudentsClassId == class_id);
            return pointer.ToList();
        }

        public List<ServerExam> GetChampionshipExams(int championshipId)
        {
            return db.FindSync(sim => sim.Exam_metadata.ChampionshipId == championshipId).ToList();
        }

        public ServerExam GetExam(int exam_id)
        {
            return db.FindSync(sim => sim.Id == exam_id).FirstOrDefault();
        }

        public async void RemoveQuestionInExam(ExamQuestion examQuestion, int examId)
        {
            var exam = db.FindSync(sim => sim.Id == examId).FirstOrDefault();
            exam.questions.RemoveAll(s => s.QuestionGuid == examQuestion.QuestionGuid);
            var update = Builders<ServerExam>.Update.Set(s => s.questions, exam.questions);
            await db.UpdateOneAsync(sim => sim.Id == examId, update);
        }

        public async void UpdateExam(ServerExam server_exam)
        {
            await db.ReplaceOneAsync(s => s.Id == server_exam.Id, server_exam);
        }
    }
}