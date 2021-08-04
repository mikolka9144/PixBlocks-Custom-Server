using PixBlocks.ServerFasade.ServerAPI;
using Pix_API.Providers;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.DBModels;
using Newtonsoft.Json;
using PixBlocks.Server.DataModels.DataModels.Woocommerce;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using System;
using Pix_API.Providers.ContainersProviders;
using PixBlocks.Server.DataModels.Tools;

namespace Pix_API
{
    public class Main_Logic
    {
        private readonly ICountriesProvider countriesProvider;
        private readonly IUserDatabaseProvider databaseProvider;
        private readonly IQuestionResultsProvider questionResultsProvider;
        private readonly IQuestionEditsProvider questionEditsProvider;
        private readonly IToyShopProvider toyShopProvider;
        private readonly INotyficationProvider notyficationProvider;
        private readonly IChampionshipsProvider championshipsProvider;
        private readonly IStudentClassProvider studentClassProvider;

        public Main_Logic(ICountriesProvider countriesProvider,
            IUserDatabaseProvider databaseProvider, IQuestionResultsProvider questionResultsProvider,
            IQuestionEditsProvider questionEditsProvider, IToyShopProvider toyShopProvider,
            INotyficationProvider notyficationProvider,IChampionshipsProvider championshipsProvider,
            IStudentClassProvider studentClassProvider)
        {
            this.countriesProvider = countriesProvider;
            this.databaseProvider = databaseProvider;
            this.questionResultsProvider = questionResultsProvider;
            this.questionEditsProvider = questionEditsProvider;
            this.toyShopProvider = toyShopProvider;
            this.notyficationProvider = notyficationProvider;
            this.championshipsProvider = championshipsProvider;
            this.studentClassProvider = studentClassProvider;
        }

        public List<Countrie> GetAllCountries()
        => countriesProvider.GetAllCountries();
        public UserAddingResult RegisterNewUser(User user)
        {
            if (databaseProvider.ContainsUserWithEmail(user.Email))
            {
                return new UserAddingResult() { IsEmailExist = true };
            }
            databaseProvider.AddUser(user);
            return new UserAddingResult() { User = user };
        }
        public UserAuthorizeResult AuthorizeUser(string loginOrEmail, string password, bool md5, LicenseData licenseData)
        {
            var user_in_question = databaseProvider.GetUser(loginOrEmail);
            if (user_in_question != null)
            {
                var Md5Password = Utills.ConvertPasswordToMD5(password);
                var IsStudentLoginValid = password == user_in_question.Student_explicitPassword && user_in_question.Student_isStudent == true;
                if (user_in_question.Md5Password == Md5Password || IsStudentLoginValid)
                {
                    return new UserAuthorizeResult()
                    {
                        User = user_in_question,
                        IsPasswordCorrect = true,
                        PixBlocksLicense = new PixBlocksLicense(LicenseType.Free)
                    };
                }
            }
            return new UserAuthorizeResult() { IsPasswordCorrect = false };
        }
        public bool StudentsLoginsCheckAvaible(string studentLogin)
        {
            return !databaseProvider.ContainsUserWithLogin(studentLogin);
        }
        [RequiresAuthentication]
        public List<QuestionResult> GetAllQuestionsResults(User user, AuthorizeData authorize)
        {
                return questionResultsProvider.GetAllQuestionsReultsForUser(authorize.UserId);

        }
        [RequiresAuthentication]
        public QuestionResult AddOrUpdateQuestionResult(QuestionResult questionResult, AuthorizeData authorize)
        {
            questionResultsProvider.AddOrUpdateQuestionResult(questionResult, authorize.UserId);
            return questionResult;
        }
        [RequiresAuthentication]
        public List<EditedQuestionCode> GetAllQuestionsCodes(User user, AuthorizeData authorize)
        {
            return questionEditsProvider.GetAllQuestionCodes(authorize.UserId);
        }
        [RequiresAuthentication]
        public EditedQuestionCode AddOrUpdateEditedQuestionCode(EditedQuestionCode editedQuestionCode, User user, AuthorizeData authorize)
        {
                questionEditsProvider.AddOrRemoveQuestionCode(editedQuestionCode, authorize.UserId);
                return editedQuestionCode;
        }
        [RequiresAuthentication]
        public EditedQuestionCode GetQuestionCode(string questionGuid, int? examId, int? editedCodeId, bool isTeacherShared, DateTime? lastUpdateTime, AuthorizeData authorize)
        {
                var question_edit = questionEditsProvider.GetQuestionEditByGuid(authorize.UserId, questionGuid);
                return question_edit;
        }
        [RequiresAuthentication]
        public User UpdateOrDeleteUser(User user, AuthorizeData authorize)
        {
            if (authorize.UserId == user.Id)
            {
                databaseProvider.UpdateUser(user);
            }
            else
            {
                var user_in_question = databaseProvider.GetUser(user.Id.Value);
                var userBelongsToTeacher = studentClassProvider.IsClassBelongsToUser(authorize.UserId, 
                    user_in_question.Student_studentsClassId.Value);
                if(userBelongsToTeacher)
                {
                    databaseProvider.UpdateUser(user);
                }
            }
            return null;
        }
        [RequiresAuthentication]
        public GetToyShopDataResult GetUserToysShopInfo(User user, AuthorizeData authorize)
        {
                var toyShop = toyShopProvider.GetToyShop(authorize.UserId);
                if (toyShop != null)
                {
                    return new GetToyShopDataResult()
                    {
                        IsToyShopExist = true,
                        ToyShopData = toyShop
                    };
                }
                return new GetToyShopDataResult
                {
                    IsToyShopExist = false
                };
        }
        [RequiresAuthentication]
        public ToyShopData AddOrUpdateToyShopInfo(ToyShopData toyShopData, AuthorizeData authorize)
        {
            toyShopProvider.SaveOrUpdateToyShop(toyShopData, authorize.UserId);
            return toyShopData;
        }
        [RequiresAuthentication]
        public List<Notification> GetNotificationForUser(string LanguageKey, AuthorizeData authorizeData)
        {
                var user = databaseProvider.GetUser(authorizeData.UserId);
                return new List<Notification>{
                notyficationProvider.GetNotyficationForUser(LanguageKey, user) 
                };
        }
        [RequiresAuthentication]
        public List<Championship> GetActiveChampionshipsInCountry(int countryId, AuthorizeData authorize)
        {
            var user = databaseProvider.GetUser(authorize.UserId);
            return championshipsProvider.GetAllChampionshipsForUser(Convert.ToInt32(countryId), user);
        }
        //Pixblocks Students Classes
        [RequiresAuthentication]
        public List<StudentsClass> GetAllStudentsClasses(int teacherID, AuthorizeData authorize)
        {
            return studentClassProvider.GetClassesForUser(authorize.UserId);
        }
        [RequiresAuthentication]
        public StudentsClass AddStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            studentClassProvider.AddClassForUser(studentsClass, authorize.UserId);
            return studentsClass;
        }
        [RequiresAuthentication]
        public StudentsClass GetStudentsClassById(int id, AuthorizeData authorize)
        {
            return studentClassProvider.GetStudentsClassById(authorize.UserId, id);
        }
        [RequiresAuthentication]
        public StudentsClass EditStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            studentClassProvider.EditClassForUser(studentsClass, authorize.UserId);
            return null;
        }
        [RequiresAuthentication]
        public StudentsClass DeleteStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            studentClassProvider.RemoveClassForUser(studentsClass, authorize.UserId);
            return null;
        }
        [RequiresAuthentication]
        public List<User> GetAllStudentsInClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            return studentClassProvider.GetStudentsInClassForUser(authorize.UserId, studentsClass.Id.Value);
        }
    }

}