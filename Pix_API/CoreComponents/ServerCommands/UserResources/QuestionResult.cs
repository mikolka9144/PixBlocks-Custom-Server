using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public List<QuestionResult> GetAllQuestionsResults(User user, AuthorizeData authorize)
        {
            if (user.Id == authorize.UserId)
            {
                return questionResultsProvider.GetAllQuestionsReultsForUser(authorize.UserId);
            }
            if (!serverUtills.IsClassBelongsToUser(authorize.UserId, user.Student_studentsClassId.Value)) return null;

            return questionResultsProvider.GetAllQuestionsReultsForUser(user.Id.Value);
        }

        public QuestionResult AddOrUpdateQuestionResult(QuestionResult questionResult, AuthorizeData authorize)
        {
            questionResultsProvider.AddOrUpdateQuestionResult(questionResult, authorize.UserId);
            return questionResult;
        }
    }
}
