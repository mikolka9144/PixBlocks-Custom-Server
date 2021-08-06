using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using Pix_API.Providers.ContainersProviders;
namespace Pix_API.Providers
{
    public class QuestionResultProvider : MultiplePoolStorageProvider<QuestionResult>, IQuestionResultsProvider
    {
        public QuestionResultProvider(DataSaver<List<QuestionResult>> saver) : base(saver)
        {
        }

        public void AddOrUpdateQuestionResult(QuestionResult questionResult, int Id)
        {
            AddOrUpdateObject(questionResult, Id, (arg1, arg2) => arg1.ID == arg2.ID);
        }

        public List<QuestionResult> GetAllQuestionsReultsForUser(int Id)
        {
            return GetObjectOrCreateNew(Id);
        }
    }

    public interface IQuestionResultsProvider
    {
        List<QuestionResult> GetAllQuestionsReultsForUser(int Id);
        void AddOrUpdateQuestionResult(QuestionResult questionResult, int Id);
    }
}
