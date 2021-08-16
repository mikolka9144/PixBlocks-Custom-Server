using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using Pix_API.Providers.ContainersProviders;
using Pix_API.Interfaces;

namespace Pix_API.Providers
{
    public class QuestionResultProvider : MultiplePoolStorageProvider<QuestionResult>, IQuestionResultsProvider
    {
        private bool AreEqual(QuestionResult result,QuestionResult result2)
        {
            var AreSameExamQuestions = result.ExamId == result2.ExamId;
            var AreSameQuestionRelated = result.QuestionGuid == result2.QuestionGuid;
            return AreSameExamQuestions && AreSameQuestionRelated;
        }

        public QuestionResultProvider(DataSaver<List<QuestionResult>> saver) : base(saver)
        {
        }

        public void AddOrUpdateQuestionResult(QuestionResult questionResult, int Id)
        {
            AddOrUpdateObject(questionResult, Id,AreEqual);
        }

        public List<QuestionResult> GetAllQuestionsReultsForUser(int Id)
        {
            return GetSingleObjectOrCreateNew(Id);
        }
    }


}
