using System;
using System.Collections.Generic;
using System.Linq;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public List<Exam> GetAllExamsInClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            return (from s in studentClassExamsProvider.GetAllExamsInClass(studentsClass.Id.Value)
                    select s.Exam_metadata).ToList();
        }

        public List<ExamQuestion> GetAllQuestionsInAllExamsInStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            if (!security.IsClassBelongsToUser(authorize.UserId, studentsClass.Id.Value)) return null;

            return studentClassExamsProvider.GetAllExamsInClass(studentsClass.Id.Value).SelectMany((s) => s.questions).ToList();
        }

        public Exam AddNewExam(Exam exam, AuthorizeData authorize)
        {
            if (!security.IsClassBelongsToUser(authorize.UserId, exam.StudentsClassId)) return null;

            exam.SetupExam();
            studentClassExamsProvider.AddExam(new ServerExam(exam));
            return exam;
        }

        public List<ExamQuestion> GetAllQuestionsInExam(Exam exam, AuthorizeData authorize)
        {
            return studentClassExamsProvider.GetExam(exam.Id).questions;
        }

        public bool AddQuestionInExam(ExamQuestion examQuestion, AuthorizeData authorize)
        {
            int studentsClassId = studentClassExamsProvider.GetExam(examQuestion.ExamId).Exam_metadata.StudentsClassId;
            if (!security.IsClassBelongsToUser(authorize.UserId, studentsClassId)) return false;

            studentClassExamsProvider.AddQuestionInExam(examQuestion, examQuestion.ExamId);
            return true;
        }

        public bool DeleteQuestionInExam(ExamQuestion examQuestion, AuthorizeData authorize)
        {
            int studentsClassId = studentClassExamsProvider.GetExam(examQuestion.ExamId).Exam_metadata.StudentsClassId;
            if (!security.IsClassBelongsToUser(authorize.UserId, studentsClassId)) return false;

            studentClassExamsProvider.RemoveQuestionInExam(examQuestion, examQuestion.ExamId);
            return true;
        }

        public List<Exam> GetAllActiveExamsForStudent(User participant, AuthorizeData authorizeData)
        {
            List<Exam> list = new List<Exam>();
            if (participant.Student_studentsClassId.HasValue)
            {
                list.AddRange(from s in studentClassExamsProvider.GetAllExamsInClass(participant.Student_studentsClassId.Value)
                              select s.Exam_metadata into s
                              where s.IsActive
                              select s);
            }
            if (participant.ChampionshipId.HasValue)
            {
                list.AddRange(from s in studentClassExamsProvider.GetChampionshipExams(participant.ChampionshipId.Value)
                              select s.Exam_metadata);
            }
            return list;
        }

        public Exam UpdateOrDeleteExam(Exam exam, AuthorizeData authorize)
        {
            ServerExam exam2 = studentClassExamsProvider.GetExam(exam.Id);
            if (!security.IsExamCreatedByUser(exam2.Exam_metadata, authorize.UserId)) return null;

            exam2.Exam_metadata = exam;
            studentClassExamsProvider.UpdateExam(exam2);
            return null;
        }
    }
}
