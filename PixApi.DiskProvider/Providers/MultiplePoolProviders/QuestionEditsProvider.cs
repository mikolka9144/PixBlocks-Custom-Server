using System.Collections.Generic;
using System.Linq;
using Pix_API.Base.Disk;
using Pix_API.PixBlocks.Interfaces;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.PixBlocks.Disk
{
	public class QuestionEditsProvider : MultiplePoolStorageProvider<EditedQuestionCode>, IQuestionEditsProvider
	{
		public QuestionEditsProvider(DataSaver<List<EditedQuestionCode>> saver)
			: base(saver)
		{
		}

		private bool AreEqual(EditedQuestionCode result, EditedQuestionCode result2)
		{
			bool num = result.ExamId == result2.ExamId;
			bool flag = result.QuesionGuid == result2.QuesionGuid;
			return num && flag;
		}

		public void AddOrRemoveQuestionCode(EditedQuestionCode questionCode, int Id)
		{
			if (!questionCode.ID.HasValue)
			{
				questionCode.ID = storage.Count();
			}
			AddOrUpdateObject(questionCode, Id, AreEqual);
		}

		public List<EditedQuestionCode> GetAllQuestionCodes(int Id)
		{
			return GetObjectOrCreateNew(Id);
		}

		public EditedQuestionCode GetQuestionEditByGuid(int Id, string guid, int? examId)
		{
			return GetObjectOrCreateNew(Id).FirstOrDefault((EditedQuestionCode s) => s.QuesionGuid == guid && s.ExamId == examId);
        }

        public void RemoveQuestionCodesForUser(int userId)
        {
            RemoveObject(userId);
        }
    }
}
