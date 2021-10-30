using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;

namespace Pix_API.PixBlocks.Interfaces
{
	public class ServerExam
	{
		public int Id;

		public Exam Exam_metadata { get; set; }

		public List<ExamQuestion> questions { get; set; }

		public ServerExam(Exam exam)
		{
			Exam_metadata = exam;
			questions = new List<ExamQuestion>();
		}
        public void set_Id(int Id)
        {
            this.Id = Id;
            Exam_metadata.Id = Id;
        }
        public ServerExam()
		{
		}
	}
}
