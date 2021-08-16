using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.Interfaces
{
    public interface IUserDatabaseProvider
    {
        User GetUser(string EmailOrLogin);
        User GetUser(int Id);
        void AddUser(User user);
        void UpdateUser(User user);
        bool ContainsUserWithEmail(string email);
        bool ContainsUserWithLogin(string login);
        List<User> GetAllUsersBelongingToClass(int classId);
    }
    public interface IToyShopProvider
    {
        ToyShopData GetToyShop(int Id);
        void SaveOrUpdateToyShop(ToyShopData toyShopData, int Id);
    }
    public interface IStudentClassExamsProvider
    {
        List<ServerExam> GetAllExamsInClass(int class_id);
        void AddExam(ServerExam serverExam);
        ServerExam GetExam(int exam_id);
        void AddQuestionInExam(ExamQuestion question, int exam_id);
        void RemoveQuestionInExam(ExamQuestion examQuestion, int examId);
        void UpdateExam(ServerExam server_exam);
        List<ServerExam> GetChampionshipExams(int championshipId);
    }
    public interface ISchoolProvider
    {
        void AddSchool(School school);
        void UpdateSchool(School school, int UserOwner_Id);
        School GetSchool(int UserOwner_Id);
    }
    public interface ICountriesProvider
    {
        List<Countrie> GetAllCountries();
    }
    public interface IQuestionEditsProvider
    {
        void AddOrRemoveQuestionCode(EditedQuestionCode questionCode, int Id);
        List<EditedQuestionCode> GetAllQuestionCodes(int Id);
        EditedQuestionCode GetQuestionEditByGuid(int Id, string guid, int? examId);
    }
    public interface IQuestionResultsProvider
    {
        List<QuestionResult> GetAllQuestionsReultsForUser(int Id);
        void AddOrUpdateQuestionResult(QuestionResult questionResult, int Id);
    }
    public interface IStudentClassProvider
    {
        List<StudentsClass> GetClassesForUser(int Id);
        StudentsClass GetStudentsClassById(int userId, int classId);
        StudentsClass GetStudentsClassByGlobalId(int classId);
        void AddClassForUser(StudentsClass studentsClass, int userId);
        void EditClassForUser(StudentsClass studentsClass, int userId);
        void RemoveClassForUser(StudentsClass studentsClass, int userId);
        List<User> GetStudentsInClassForUser(int userID, int classID);
    }
    public interface IUserCommentsProvider
    {
        List<Comment> GetAllCommentsForUser(int user_id);
        void AddOrUpdateComment(Comment comment, int user_id);
    }
    public interface IBrandingProvider
    {
        string GetBase64LogoForUser(int userId);
    }
}
