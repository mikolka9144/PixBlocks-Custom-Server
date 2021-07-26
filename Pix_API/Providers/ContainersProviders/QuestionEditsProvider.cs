using System;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using System.Collections.Generic;
using System.Linq;
using Pix_API.Providers.ContainersProviders;

namespace Pix_API.Providers
{
    public class QuestionEditsProvider : Storage_Provider<EditedQuestionCode>, IQuestionEditsProvider
    {
        public QuestionEditsProvider(DataSaver<List<EditedQuestionCode>> saver) : base(saver)
        {
        }

        public void AddOrRemoveQuestionCode(EditedQuestionCode questionCode, int Id)
        {
            if (!questionCode.ID.HasValue) questionCode.ID = storage.Count;
            AddOrUpdateObject(questionCode, Id, (arg1, arg2) => arg1.QuesionGuid == arg2.QuesionGuid);
        }

        public List<EditedQuestionCode> GetAllQuestionCodes(int Id)
        {
            return GetAllObjectsForUser(Id);
        }

        public EditedQuestionCode GetQuestionEditByGuid(int Id, string guid)
        {
            var questionCodes = GetAllObjectsForUser(Id);
            return questionCodes.FirstOrDefault(s => s.QuesionGuid == guid);
        }
    }

    public interface IQuestionEditsProvider
    {
        void AddOrRemoveQuestionCode(EditedQuestionCode questionCode, int Id);
        List<EditedQuestionCode> GetAllQuestionCodes(int Id);
        EditedQuestionCode GetQuestionEditByGuid(int Id,string guid);
    }
}
