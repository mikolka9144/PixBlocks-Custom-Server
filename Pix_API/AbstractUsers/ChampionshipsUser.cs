using System.Collections.Generic;
using System.Linq;
using Pix_API.CoreComponents.ServerCommands;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using PixBlocks.Server.DataModels.DataModels.DBModels;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API
{
    internal class ChampionshipsUser : IAbstractUser
    {
        private readonly ICountriesProvider countriesProvider;
        private readonly IUserDatabaseProvider databaseProvider;
        private readonly IQuestionResultsProvider questionResultsProvider;
        private readonly IStudentClassExamsProvider studentClassExamsProvider;
        private readonly IChampionshipsMetadataProvider championships;
        private readonly ServerConfiguration configuration;

        public ChampionshipsUser(ICountriesProvider countriesProvider, IUserDatabaseProvider databaseProvider,IQuestionResultsProvider questionResultsProvider,IStudentClassExamsProvider studentClassExamsProvider,IChampionshipsMetadataProvider championships,ServerConfiguration configuration)
        {
            this.countriesProvider = countriesProvider;
            this.databaseProvider = databaseProvider;
            this.questionResultsProvider = questionResultsProvider;
            this.studentClassExamsProvider = studentClassExamsProvider;
            this.championships = championships;
            this.configuration = configuration;
        }
        public string password => configuration.ChampionshipUser_Password;

        public string login => "CHAMPIONSHIP";

        public List<Championship> GetActiveChampionshipsInCountry(int countryId, AuthorizeData authorize) => new List<Championship>();
        public List<Comment> GetAllCommentsFromStudentsClass(StudentsClass studentsClass, AuthorizeData authorize) => new List<Comment>();
        public List<QuestionResult> GetAllQuestionsResults(User user, AuthorizeData authorize) => new List<QuestionResult>();
        public List<Notification> GetNotificationForUser(string LanguageKey, AuthorizeData authorizeData) => new List<Notification>();
        public GetToyShopDataResult GetUserToysShopInfo(User user, AuthorizeData authorize) => new GetToyShopDataResult() { IsToyShopExist = false};
        public User GenerateUser(int Id)
        {
            return new User()
            {
                Id = Id,
                Name = login,
                Md5Password = password,
                IsEmailActivated = true,
                Admin_isAdmin = true,
                Teacher_isTeacher = true
            };
        }
        public List<StudentsClass> GetAllStudentsClasses(int teacherID, AuthorizeData authorize)
        {
            var countries = countriesProvider.GetAllCountries();
            var classes = new List<StudentsClass>();
            foreach (var item in countries)
            {
                var user_class = new StudentsClass()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Yearbook = item.Id,
                    Description = $"Abstract class for users in {item.Code}"
                };
                classes.Add(user_class);
            }
            return classes;
        }
        public List<User> GetAllStudentsInClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            var list = databaseProvider.GetAllUsersInCountry(studentsClass.Id.Value);
            list.ForEach(s => s.Student_isAcceptedToStudentsClass = true); // Spoof student entry
            return list;
        }

        public List<QuestionResult> GetAllResultsForStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            var users = GetAllStudentsInClass(studentsClass, authorize);
            var results_pool = new List<QuestionResult>();
            foreach (var user in users)
            {
                var results = questionResultsProvider.GetAllQuestionsReultsForUser(user.Id.Value);
                results_pool.AddRange(results);
            }
            return results_pool;
        }
        public StudentsClass GetStudentsClassById(int id, AuthorizeData authorize)
        {
            var item = countriesProvider.GetAllCountries().Find(s => s.Id == id);
            return new StudentsClass()
            {
                Id = item.Id,
                Name = item.Name,
                Yearbook = item.Id,
                Description = $"Abstract class for users in {item.Code}"
            };
        }
        //Exams
        
        public List<ExamQuestion> GetAllQuestionsInAllExamsInStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            return studentClassExamsProvider.GetAllExamsInClass(-1).SelectMany((s) => s.questions).ToList();
        }

        public Exam AddNewExam(Exam exam, AuthorizeData authorize)
        {
            exam.SetupExam();
            exam.StudentsClassId = -1;
            studentClassExamsProvider.AddExam(new ServerExam(exam));
            return exam;
        }

        public List<ExamQuestion> GetAllQuestionsInExam(Exam exam, AuthorizeData authorize)
        {
            return studentClassExamsProvider.GetExam(exam.Id).questions;
        }
        public List<Exam> GetAllExamsInClass(StudentsClass studentsClass, AuthorizeData authorize)
        {
            return (from s in studentClassExamsProvider.GetAllExamsInClass(-1)
                    select s.Exam_metadata).ToList();
        }

        public bool AddQuestionInExam(ExamQuestion examQuestion, AuthorizeData authorize)
        {
            int studentsClassId = studentClassExamsProvider.GetExam(examQuestion.ExamId).Exam_metadata.StudentsClassId;

            studentClassExamsProvider.AddQuestionInExam(examQuestion, examQuestion.ExamId);
            return true;
        }

        public bool DeleteQuestionInExam(ExamQuestion examQuestion, AuthorizeData authorize)
        {
            int studentsClassId = studentClassExamsProvider.GetExam(examQuestion.ExamId).Exam_metadata.StudentsClassId;

            studentClassExamsProvider.RemoveQuestionInExam(examQuestion, examQuestion.ExamId);
            return true;
        }

        public Exam UpdateOrDeleteExam(Exam exam, AuthorizeData authorize)
        {
            ServerExam exam2 = studentClassExamsProvider.GetExam(exam.Id);
            exam.StudentsClassId = -1;
            exam2.Exam_metadata = exam;
            studentClassExamsProvider.UpdateExam(exam2);
            return null;
        }
        public List<Championship> GetAllActiveChampionships(AuthorizeData authorize) => championships.GetAllChampionships();
    }
}