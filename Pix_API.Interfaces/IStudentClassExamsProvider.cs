using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;

namespace Pix_API.Interfaces
{
	public interface IStudentClassExamsProvider
	{
		List<ServerExam> GetAllExamsInClass(int class_id);

		void RemoveAllExamsInClass(int class_id);

		void AddExam(ServerExam serverExam);

		ServerExam GetExam(int exam_id);

		void AddQuestionInExam(ExamQuestion question, int exam_id);

		void RemoveQuestionInExam(ExamQuestion examQuestion, int examId);

		void UpdateExam(ServerExam server_exam);

		List<ServerExam> GetChampionshipExams(int championshipId);
	}
}
