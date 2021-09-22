using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.Interfaces
{
	public interface IQuestionResultsProvider
	{
		List<QuestionResult> GetAllQuestionsReultsForUser(int Id);

		void AddOrUpdateQuestionResult(QuestionResult questionResult, int Id);
        void RemoveAllQuestionResultsForUser(int userId);
    }
}
