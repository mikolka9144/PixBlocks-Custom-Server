using System;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using System.Collections.Generic;
using System.Linq;
using Pix_API.Providers.ContainersProviders;
using Pix_API.Interfaces;

namespace Pix_API.Providers
{
    public class QuestionEditsProvider :MultiplePoolStorageProvider<EditedQuestionCode>, IQuestionEditsProvider
    {
        public QuestionEditsProvider(DataSaver<List<EditedQuestionCode>> saver) : base(saver)
        {
        }

        private bool AreEqual(EditedQuestionCode result, EditedQuestionCode result2)
        {
            var AreSameExamQuestions = result.ExamId == result2.ExamId;
            var AreSameQuestionRelated = result.QuesionGuid == result2.QuesionGuid;
            return AreSameExamQuestions && AreSameQuestionRelated;
        }

        public void AddOrRemoveQuestionCode(EditedQuestionCode questionCode, int Id)
        {
            if (!questionCode.ID.HasValue) questionCode.ID = storage.Count();
            AddOrUpdateObject(questionCode, Id,AreEqual);
        }

        public List<EditedQuestionCode> GetAllQuestionCodes(int Id)
        {
            return GetObjectOrCreateNew(Id);
        }

        public EditedQuestionCode GetQuestionEditByGuid(int Id, string guid, int? examId)
        {
            var questionCodes = GetObjectOrCreateNew(Id);
            return questionCodes.FirstOrDefault(s => s.QuesionGuid == guid && s.ExamId == examId);
        }
    }


}
