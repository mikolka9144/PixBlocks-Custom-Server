using System.Collections.Generic;
using Pix_API.Base.Disk;
using Pix_API.PixBlocks.Interfaces;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.PixBlocks.Disk
{
    public class QuestionResultProvider : MultiplePoolStorageProvider<QuestionResult>, IQuestionResultsProvider
    {
        private bool AreEqual(QuestionResult result, QuestionResult result2)
        {
            bool num = result.ExamId == result2.ExamId;
            bool flag = result.QuestionGuid == result2.QuestionGuid;
            return num && flag;
        }

        public QuestionResultProvider(DataSaver<List<QuestionResult>> saver)
            : base(saver)
        {
        }

        public void AddOrUpdateQuestionResult(QuestionResult questionResult, int Id)
        {
            AddOrUpdateObject(questionResult, Id, AreEqual);
        }

        public List<QuestionResult> GetAllQuestionsReultsForUser(int Id)
        {
            return GetObjectOrCreateNew(Id);
        }

        public void RemoveAllQuestionResultsForUser(int userId)
        {
            RemoveObject(userId);
        }
    }
}