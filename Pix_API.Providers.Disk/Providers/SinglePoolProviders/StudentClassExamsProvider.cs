using System.Collections.Generic;
using System.Linq;
using Pix_API.Interfaces;
using Pix_API.Providers.BaseClasses;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;

namespace Pix_API.Providers.ContainersProviders
{
	public class StudentClassExamsProvider : SinglePoolStorageProvider<ServerExam>, IStudentClassExamsProvider
	{
		private IdAssigner idAssigner;

		public StudentClassExamsProvider(DataSaver<ServerExam> saver)
			: base(saver)
		{
			idAssigner = new IdAssigner(new DiskIndexSaver("exams.index"));
		}

		public void AddExam(ServerExam serverExam)
		{
			serverExam.set_Id(idAssigner.NextEmptyId);
			AddSingleObject(serverExam, serverExam.Exam_metadata.Id);
		}

		public void AddQuestionInExam(ExamQuestion question, int exam_id)
		{
			ServerExam exam = GetExam(exam_id);
			exam.questions.Add(question);
			AddOrUpdateSingleObject(exam, exam_id);
		}

		public List<ServerExam> GetAllExamsInClass(int class_id)
		{
			return storage.Where((ServerExam s) => s.Exam_metadata.StudentsClassId == class_id).ToList();
		}

		public List<ServerExam> GetChampionshipExams(int championshipId)
		{
			return storage.Where((ServerExam s) => s.Exam_metadata.ChampionshipId.HasValue && s.Exam_metadata.ChampionshipId.Value == championshipId).ToList();
		}

		public ServerExam GetExam(int exam_id)
		{
			return GetSingleObject(exam_id);
		}

		public void RemoveAllExamsInClass(int class_id)
		{
			foreach (ServerExam item in storage.Where((ServerExam s) => s.Exam_metadata.StudentsClassId == class_id))
			{
				RemoveObject(item.Id);
			}
		}

		public void RemoveQuestionInExam(ExamQuestion examQuestion, int examId)
		{
			ServerExam exam = GetExam(examId);
			exam.questions.RemoveAll((ExamQuestion s) => s.QuestionGuid == examQuestion.QuestionGuid);
			AddOrUpdateSingleObject(exam, examId);
		}

		public void UpdateExam(ServerExam server_exam)
		{
			AddOrUpdateSingleObject(server_exam, server_exam.Exam_metadata.Id);
		}
	}
}
