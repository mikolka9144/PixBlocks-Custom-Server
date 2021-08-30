using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.Interfaces
{
	public interface IQuestionEditsProvider
	{
		void AddOrRemoveQuestionCode(EditedQuestionCode questionCode, int User_Id);

		List<EditedQuestionCode> GetAllQuestionCodes(int Id);

		EditedQuestionCode GetQuestionEditByGuid(int Id, string guid, int? examId);
	}
}
