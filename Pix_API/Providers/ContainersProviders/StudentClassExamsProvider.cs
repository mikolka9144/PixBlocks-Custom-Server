using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;
using System.Linq;
namespace Pix_API.Providers.ContainersProviders
{
    public class StudentClassExamsProvider : MultiplePoolStorageProvider<ServerExam>,IStudentClassExamsProvider
    {
        private IdAssigner idAssigner;

        public StudentClassExamsProvider(DataSaver<List<ServerExam>> saver) : base(saver)
        {
            var id_list = storage.SelectMany(s => s.Obj).Select(s => s.Exam_metadata.Id).ToList();
            idAssigner = new IdAssigner(id_list);
        }

        public void AddExam(ServerExam serverExam)
        {
            var class_exams = GetObjectOrCreateNew(serverExam.Exam_metadata.StudentsClassId);
            serverExam.Exam_metadata.Id = idAssigner.NextEmptyId;

            AddObject(serverExam, serverExam.Exam_metadata.StudentsClassId);
        }

        public void AddQuestionInExam(ExamQuestion question, int exam_id)
        {
            var exam = GetExam(exam_id);
            exam.questions.Add(question);
            AddOrUpdateObject(exam, exam.Exam_metadata.StudentsClassId, ((a, b) => a.Exam_metadata.Id == b.Exam_metadata.Id));
        }

        public List<ServerExam> GetAllExamsInClass(int class_id)
        {
            var exams = GetObjectOrCreateNew(class_id);
            return exams;
        }

        public ServerExam GetExam(int exam_id)
        {
            return storage.SelectMany(s => s.Obj).FirstOrDefault(s => s.Exam_metadata.Id == exam_id);
        }
    }

    public interface IStudentClassExamsProvider
    {
        List<ServerExam> GetAllExamsInClass(int class_id);
        void AddExam(ServerExam serverExam);
        ServerExam GetExam(int exam_id);
        void AddQuestionInExam(ExamQuestion question, int exam_id);
    }

    public class ServerExam
    {

        public ServerExam(Exam exam)
        {
            this.Exam_metadata = exam;
            questions = new List<ExamQuestion>();
        }
        public ServerExam()
        {

        }
        public Exam Exam_metadata { get; set; }
        public List<ExamQuestion> questions { get; set; }
    }
}
