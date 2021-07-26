using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
namespace Pix_API.Models
{
    public class UserQuestionEditedCode
    {
        public UserQuestionEditedCode(int id,List<EditedQuestionCode> questionCodes)
        {
            Id = id;
            QuestionCodes = questionCodes;
        }

        public int Id { get; }
        public List<EditedQuestionCode> QuestionCodes { get; }
    }
}
