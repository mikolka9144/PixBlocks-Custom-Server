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
using PixBlocks.Server.DataModels.DataModels.ExamInfo;
using System.Linq;

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
        private readonly IStudentClassExamsProvider studentClassExamsProvider;

        public Main_Logic(ICountriesProvider countriesProvider,
            IUserDatabaseProvider databaseProvider, IQuestionResultsProvider questionResultsProvider,
            IQuestionEditsProvider questionEditsProvider, IToyShopProvider toyShopProvider,
            INotyficationProvider notyficationProvider,IChampionshipsProvider championshipsProvider,
            IStudentClassProvider studentClassProvider,IStudentClassExamsProvider studentClassExamsProvider)
        {
            this.countriesProvider = countriesProvider;
            this.databaseProvider = databaseProvider;
            this.questionResultsProvider = questionResultsProvider;
            this.questionEditsProvider = questionEditsProvider;
            this.toyShopProvider = toyShopProvider;
            this.notyficationProvider = notyficationProvider;
            this.championshipsProvider = championshipsProvider;
            this.studentClassProvider = studentClassProvider;
            this.studentClassExamsProvider = studentClassExamsProvider;
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

        public List<QuestionResult> GetAllQuestionsResults(User user, AuthorizeData authorize)
        {
                return questionResultsProvider.GetAllQuestionsReultsForUser(authorize.UserId);

        }

        public QuestionResult AddOrUpdateQuestionResult(QuestionResult questionResult, AuthorizeData authorize)
        {
            questionResultsProvider.AddOrUpdateQuestionResult(questionResult, authorize.UserId);
            return questionResult;
        }

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
                var question_edit = questionEditsProvider.GetQuestionEditByGuid(authorize.UserId, questionGuid);
                return question_edit;
        }
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
        public ToyShopData AddOrUpdateToyShopInfo(ToyShopData toyShopData, AuthorizeData authorize)
        {
            toyShopProvider.SaveOrUpdateToyShop(toyShopData, authorize.UserId);
            return toyShopData;
        }
        public List<Notification> GetNotificationForUser(string LanguageKey, AuthorizeData authorizeData)
        {
                var user = databaseProvider.GetUser(authorizeData.UserId);
                return new List<Notification>{
                notyficationProvider.GetNotyficationForUser(LanguageKey, user) 
                };
        }
        public List<Championship> GetActiveChampionshipsInCountry(int countryId, AuthorizeData authorize)
        {
            var user = databaseProvider.GetUser(authorize.UserId);
            return championshipsProvider.GetAllChampionshipsForUser(Convert.ToInt32(countryId), user);
        }
        //Pixblocks Students Classes
        public List<StudentsClass> GetAllStudentsClasses(int teacherID, AuthorizeData authorize)
        {
            return studentClassProvider.GetClassesForUser(authorize.UserId);
        }
        public StudentsClass AddStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            studentClassProvider.AddClassForUser(studentsClass, authorize.UserId);
            return studentsClass;
        }
        public StudentsClass GetStudentsClassById(int id, AuthorizeData authorize)
        {
            return studentClassProvider.GetStudentsClassById(authorize.UserId, id);
        }
        public StudentsClass EditStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            studentClassProvider.EditClassForUser(studentsClass, authorize.UserId);
            return null;
        }
        public StudentsClass DeleteStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            studentClassProvider.RemoveClassForUser(studentsClass, authorize.UserId);
            return null;
        }
        public List<User> GetAllStudentsInClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            return studentClassProvider.GetStudentsInClassForUser(authorize.UserId, studentsClass.Id.Value);
        }
        public List<Exam> GetAllExamsInClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            return studentClassExamsProvider.GetAllExamsInClass(studentsClass.Id.Value).Select(s => s.Exam_metadata).ToList();
        }
        public List<ExamQuestion> GetAllQuestionsInAllExamsInStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            if(studentClassProvider.IsClassBelongsToUser(authorize.UserId, studentsClass.Id.Value))
            {
                return studentClassExamsProvider.GetAllExamsInClass(studentsClass.Id.Value).SelectMany(s => s.questions).ToList();
            }
            return null;
        }
        public Exam AddNewExam(Exam exam, AuthorizeData authorize)
        {
            if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, exam.StudentsClassId))
            {
                exam.SetupExam();
                studentClassExamsProvider.AddExam(new ServerExam(exam));
            }
            return exam;
        }
        public List<ExamQuestion> GetAllQuestionsInExam(Exam exam, AuthorizeData authorize)
        {
            return studentClassExamsProvider.GetExam( exam.Id).questions;
        }
        public bool AddQuestionInExam(ExamQuestion examQuestion, AuthorizeData authorize)
        {
            var class_id = studentClassExamsProvider.GetExam(examQuestion.ExamId).Exam_metadata.StudentsClassId;
            if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, class_id))
            {
                studentClassExamsProvider.AddQuestionInExam(examQuestion, examQuestion.ExamId);
                return true;
            }
            return false;
        }
    }

}