using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.PixBlocks.MainServer
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
                User user = databaseProvider.GetUser(authorize.UserId);
                StudentsClass studentsClassByGlobalId = studentClassProvider.GetStudentsClassByGlobalId(user.Student_studentsClassId.Value);
                return questionEditsProvider.GetQuestionEditByGuid(studentsClassByGlobalId.TeacherID, questionGuid, null);
            }
            return questionEditsProvider.GetQuestionEditByGuid(authorize.UserId, questionGuid, examId);
        }
    }
}
