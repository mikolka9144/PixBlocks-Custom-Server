using System.Linq;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;
using Pix_API.Interfaces;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public List<Exam> GetAllExamsInClass(StudentsClass studentsClass, AuthorizeData authorize)
    {
        return studentClassExamsProvider.GetAllExamsInClass(studentsClass.Id.Value).Select(s => s.Exam_metadata).ToList();
    }
    public List<ExamQuestion> GetAllQuestionsInAllExamsInStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
    {
        if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, studentsClass.Id.Value))
        {
            return studentClassExamsProvider.GetAllExamsInClass(studentsClass.Id.Value).SelectMany(s => s.questions).ToList();
        }
        return null;
    }
    public Exam AddNewExam(Exam exam, AuthorizeData authorize)
    {
        if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, exam.StudentsClassId))
        {
            exam.SetupExam();
            studentClassExamsProvider.AddExam(new ServerExam(exam));
        }
        return exam;
    }
    public List<ExamQuestion> GetAllQuestionsInExam(Exam exam, AuthorizeData authorize)
    {
        return studentClassExamsProvider.GetExam(exam.Id).questions;
    }
    public bool AddQuestionInExam(ExamQuestion examQuestion, AuthorizeData authorize)
    {
        var class_id = studentClassExamsProvider.GetExam(examQuestion.ExamId).Exam_metadata.StudentsClassId;
        if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, class_id))
        {
            studentClassExamsProvider.AddQuestionInExam(examQuestion, examQuestion.ExamId);
            return true;
        }
        return false;
    }
    public bool DeleteQuestionInExam(ExamQuestion examQuestion, AuthorizeData authorize)
    {
        var class_id = studentClassExamsProvider.GetExam(examQuestion.ExamId).Exam_metadata.StudentsClassId;
        if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, class_id))
        {
            studentClassExamsProvider.RemoveQuestionInExam(examQuestion, examQuestion.ExamId);
            return true;
        }
        return false;
    }
    public List<Exam> GetAllActiveExamsForStudent(User participant, AuthorizeData authorizeData)
    {
            var student_exams = new List<Exam>();
            if (participant.Student_studentsClassId.HasValue)
            {
                 student_exams.AddRange(studentClassExamsProvider.GetAllExamsInClass(participant.Student_studentsClassId.Value).Select(s => s.Exam_metadata).Where(s => s.IsActive));
            }
            if (participant.ChampionshipId.HasValue)
            {
                student_exams.AddRange(studentClassExamsProvider.GetChampionshipExams(participant.ChampionshipId.Value).Select(s => s.Exam_metadata));
            }
            return student_exams;
    }
    public Exam UpdateOrDeleteExam(Exam exam, AuthorizeData authorize)
    {
        if (studentClassProvider.IsExamCreatedByUser(studentClassExamsProvider.GetExam(exam.Id).Exam_metadata, authorize.UserId))
        {
            var server_exam = studentClassExamsProvider.GetExam(exam.Id);
            server_exam.Exam_metadata = exam;
            studentClassExamsProvider.UpdateExam(server_exam);
        }
        return null;
    }
    }
}
