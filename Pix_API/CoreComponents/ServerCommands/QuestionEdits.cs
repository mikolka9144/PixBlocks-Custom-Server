using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.CoreComponents.ServerCommands
{
    public partial class Main_Logic
    {
        public List<EditedQuestionCode> GetAllQuestionsCodes(User user, AuthorizeData authorize)
        {
            return questionEditsProvider.GetAllQuestionCodes(authorize.UserId);
        }
        public EditedQuestionCode AddOrUpdateEditedQuestionCode(EditedQuestionCode editedQuestionCode, User user, AuthorizeData authorize)
        {
            questionEditsProvider.AddOrRemoveQuestionCode(editedQuestionCode, authorize.UserId);
            return editedQuestionCode;
        }
        public EditedQuestionCode GetQuestionCode(string questionGuid, int? examId, int? editedCodeId, bool isTeacherShared, DateTime? lastUpdateTime, AuthorizeData authorize)
        {
            if (isTeacherShared)
            {
                var user = databaseProvider.GetUser(authorize.UserId);
                var user_class = studentClassProvider.GetStudentsClassByGlobalId(user.Student_studentsClassId.Value);
                return questionEditsProvider.GetQuestionEditByGuid(user_class.TeacherID, questionGuid, null);
            }
            return questionEditsProvider.GetQuestionEditByGuid(authorize.UserId, questionGuid, examId);

        }
    }
}
