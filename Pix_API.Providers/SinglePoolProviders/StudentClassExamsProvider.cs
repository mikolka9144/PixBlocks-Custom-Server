using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;
using System.Linq;
using Pix_API.Providers.BaseClasses;
using Pix_API.Interfaces;

namespace Pix_API.Providers.ContainersProviders
{
    public class StudentClassExamsProvider : SinglePoolStorageProvider<ServerExam>,IStudentClassExamsProvider
    {
        private IdAssigner idAssigner;

        public StudentClassExamsProvider(DataSaver<ServerExam> saver) : base(saver)
        {
            var id_list = storage.Select(s => s.Obj).Select(s => s.Exam_metadata.Id).ToList();
            idAssigner = new IdAssigner(id_list);
        }

        public void AddExam(ServerExam serverExam)
        { 
            serverExam.Exam_metadata.Id = idAssigner.NextEmptyId;

            AddSingleObject(serverExam, serverExam.Exam_metadata.Id);
        }

        public void AddQuestionInExam(ExamQuestion question, int exam_id)
        {
            var exam = GetExam(exam_id);
            exam.questions.Add(question);
            AddOrUpdateSingleObject(exam, exam_id);
        }

        public List<ServerExam> GetAllExamsInClass(int class_id)
        {
            return storage.FindAll(s =>s.Obj.Exam_metadata
                .StudentsClassId == class_id).Select(s => s.Obj).ToList();
        }

        public List<ServerExam> GetChampionshipExams(int championshipId)
        {
            return storage.FindAll(s =>
            {
                if (s.Obj.Exam_metadata.ChampionshipId.HasValue)
                {
                    return s.Obj.Exam_metadata.ChampionshipId.Value == championshipId;
                }
                return false;
            }).Select(s => s.Obj).ToList();
        }

        public ServerExam GetExam(int exam_id) => GetSingleObject(exam_id)?.Obj;

        public void RemoveQuestionInExam(ExamQuestion examQuestion, int examId)
        {
            var exam = GetExam(examId);
            exam.questions.RemoveAll(s => s.QuestionGuid == examQuestion.QuestionGuid);
            AddOrUpdateSingleObject(exam, examId);
        }

        public void UpdateExam(ServerExam server_exam)
        {
            AddOrUpdateSingleObject(server_exam, server_exam.Exam_metadata.Id);
        }
    }




}
