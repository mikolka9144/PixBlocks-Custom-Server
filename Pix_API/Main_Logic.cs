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

        public Main_Logic(ICountriesProvider countriesProvider,
            IUserDatabaseProvider databaseProvider, IQuestionResultsProvider questionResultsProvider,
            IQuestionEditsProvider questionEditsProvider, IToyShopProvider toyShopProvider,
            INotyficationProvider notyficationProvider,IChampionshipsProvider championshipsProvider)
        {
            this.countriesProvider = countriesProvider;
            this.databaseProvider = databaseProvider;
            this.questionResultsProvider = questionResultsProvider;
            this.questionEditsProvider = questionEditsProvider;
            this.toyShopProvider = toyShopProvider;
            this.notyficationProvider = notyficationProvider;
            this.championshipsProvider = championshipsProvider;
        }

        public List<Countrie> GetAllCountries(string[] parameters)
        => countriesProvider.GetAllCountries();
        public UserAddingResult RegisterNewUser(string[] parameters)
        {
            var user = JsonConvert.DeserializeObject<User>(parameters[0]);
            if (databaseProvider.ContainsUserWithEmail(user.Email))
            {
                return new UserAddingResult() { IsEmailExist = true };
            }
            databaseProvider.AddUser(user);
            return new UserAddingResult() { User = user };
        }
        public UserAuthorizeResult AuthorizeUser(string[] parameters)
        {
            var email = parameters[0];
            var password_md5 = Utills.ConvertPasswordToMD5(parameters[1]);
            var user_in_question = databaseProvider.GetUser(email);
            if (user_in_question != null)
            {
                if (user_in_question.Md5Password == password_md5)
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
        public List<QuestionResult> GetAllQuestionsResults(string[] parameters)
        {
            var auth = JsonConvert.DeserializeObject<AuthorizeData>(parameters[1]);
            if (databaseProvider.IsAuthorizeValid(auth))
            {
                return questionResultsProvider.GetAllQuestionsReultsForUser(auth.UserId);
            }
            return null;
        }
        public QuestionResult AddOrUpdateQuestionResult(string[] parameters)
        {
            //(QuestionResult questionResult, AuthorizeData authorize
            var question_result = JsonConvert.DeserializeObject<QuestionResult>(parameters[0]);
            var auth = JsonConvert.DeserializeObject<AuthorizeData>(parameters[1]);
            if (databaseProvider.IsAuthorizeValid(auth))
            {
                questionResultsProvider.AddOrUpdateQuestionResult(question_result, auth.UserId);
            }
            return question_result;
        }

        public List<EditedQuestionCode> GetAllQuestionsCodes(string[] parameters)
        {
            //User user, AuthorizeData authorize
            var auth = JsonConvert.DeserializeObject<AuthorizeData>(parameters[1]);
            if (databaseProvider.IsAuthorizeValid(auth))
            {
                return questionEditsProvider.GetAllQuestionCodes(auth.UserId);
            }
            return null;
        }
        public EditedQuestionCode AddOrUpdateEditedQuestionCode(string[] parameters)
        {
            //EditedQuestionCode editedQuestionCode, User user, AuthorizeData authorize
            var auth = JsonConvert.DeserializeObject<AuthorizeData>(parameters[2]);
            if (databaseProvider.IsAuthorizeValid(auth))
            {
                var question_code = JsonConvert
                .DeserializeObject<EditedQuestionCode>(parameters[0]);
                questionEditsProvider.AddOrRemoveQuestionCode(question_code, auth.UserId);
                return question_code;
            }
            return null;
        }
        public EditedQuestionCode GetQuestionCode(string[] parameters)
        {
            //string questionGuid, int? examId, int? editedCodeId, bool isTeacherShared, DateTime? lastUpdateTime, AuthorizeData authorize
            var auth = JsonConvert.DeserializeObject<AuthorizeData>(parameters[5]);
            if (databaseProvider.IsAuthorizeValid(auth))
            {
                var question_guid = parameters[0];
                var question_edit = questionEditsProvider.GetQuestionEditByGuid(auth.UserId, question_guid);
                return question_edit;
            }
            return null;
        }
        public User UpdateOrDeleteUser(string[] parameters)
        {
            //User user, AuthorizeData authorize
            var user = JsonConvert.DeserializeObject<User>(parameters[0]);
            var auth = JsonConvert.DeserializeObject<AuthorizeData>(parameters[1]);
            if (databaseProvider.IsAuthorizeValid(auth) && auth.UserId == user.Id)
            {
                databaseProvider.UpdateUser(user);
            }
            return null;
        }
        public GetToyShopDataResult GetUserToysShopInfo(string[] parameters)
        {
            //User user, AuthorizeData authorize
            var auth = JsonConvert.DeserializeObject<AuthorizeData>(parameters[1]);
            if (databaseProvider.IsAuthorizeValid(auth))
            {
                var toyShop = toyShopProvider.GetToyShop(auth.UserId);
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
            return null;
        }
        public ToyShopData AddOrUpdateToyShopInfo(string[] parameters)
        {
            //ToyShopData toyShopData, AuthorizeData authorize
            var auth = JsonConvert.DeserializeObject<AuthorizeData>(parameters[1]);
            var clientToyShop = JsonConvert.DeserializeObject<ToyShopData>(parameters[0]);
            if (databaseProvider.IsAuthorizeValid(auth))
            {
                toyShopProvider.SaveOrUpdateToyShop(clientToyShop, auth.UserId);
            }
            return clientToyShop;
        }
        public List<Notification> GetNotificationForUser(string[] parameters)
        {
            //string LanguageKey, AuthorizeData authorizeData
            var auth = JsonConvert.DeserializeObject<AuthorizeData>(parameters[1]);
            if (databaseProvider.IsAuthorizeValid(auth))
            {
                var user = databaseProvider.GetUser(auth.UserId);
                return new List<Notification>{
                notyficationProvider.GetNotyficationForUser(parameters[0], user) 
                };
            }
            return null;
        }
        public List<Championship> GetActiveChampionshipsInCountry(string[] parameters)
        {
            //int countryId, AuthorizeData authorize
            var auth = JsonConvert.DeserializeObject<AuthorizeData>(parameters[1]);
            if (databaseProvider.IsAuthorizeValid(auth))
            {
                var user = databaseProvider.GetUser(auth.UserId);
                return championshipsProvider.GetAllChampionshipsForUser(Convert.ToInt32(parameters[0]), user);
            }
            return null;
        }

    }

}