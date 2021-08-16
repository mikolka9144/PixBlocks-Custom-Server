using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;

namespace Pix_API.Interfaces
{
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
