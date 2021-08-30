using System;
using System.Collections.Generic;
using System.Linq;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using PixBlocks.Server.DataModels.DataModels.DBModels;
using PixBlocks.Server.DataModels.DataModels.ExamInfo;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using PixBlocks.Server.DataModels.DataModels.Woocommerce;

namespace Pix_API.CoreComponents.ServerCommands
{
	public class Main_Logic : ICommandRepository
	{
		private readonly ICountriesProvider countriesProvider;

		private readonly IUserDatabaseProvider databaseProvider;

		private readonly IQuestionResultsProvider questionResultsProvider;

		private readonly IQuestionEditsProvider questionEditsProvider;

		private readonly IToyShopProvider toyShopProvider;

		private readonly INotyficationProvider notyficationProvider;

		private readonly IChampionshipsMetadataProvider championshipsProvider;

		private readonly IStudentClassProvider studentClassProvider;

		private readonly IStudentClassExamsProvider studentClassExamsProvider;

		private readonly IUserCommentsProvider userCommentsProvider;

		private readonly ISchoolProvider schoolProvider;

		private readonly IBrandingProvider brandingProvider;

		private readonly IParentInfoHolder parentInfoProvider;

		public List<Countrie> GetAllCountries(object _)
		{
			return countriesProvider.GetAllCountries();
		}

		public UserAddingResult RegisterNewUser(User user)
		{
			if (user.Email != null && databaseProvider.ContainsUserWithEmail(user.Email))
			{
				return new UserAddingResult
				{
					IsEmailExist = true
				};
			}
			user.SetupUser();
			databaseProvider.AddUser(user);
			return new UserAddingResult
			{
				User = user
			};
		}

		public UserAuthorizeResult AuthorizeUser(string loginOrEmail, string password, bool md5, LicenseData licenseData)
		{
			User user = databaseProvider.GetUser(loginOrEmail);
			if (user != null)
			{
				string text = Utills.ConvertPasswordToMD5(password);
				bool flag = password == user.Student_explicitPassword && user.Student_isStudent;
				if (user.Md5Password == text || flag)
				{
					user.RegisterLogin();
					databaseProvider.UpdateUser(user);
					return new UserAuthorizeResult
					{
						User = user,
						IsPasswordCorrect = true,
						PixBlocksLicense = new PixBlocksLicense(LicenseType.Free),
						SchoolLogoInBase64 = brandingProvider.GetBase64LogoForUser(user.Id.Value)
					};
				}
			}
			return new UserAuthorizeResult
			{
				IsPasswordCorrect = false
			};
		}

		public bool StudentsLoginsCheckAvaible(string studentLogin)
		{
			return !databaseProvider.ContainsUserWithLogin(studentLogin);
		}

		public User UpdateOrDeleteUser(User user, AuthorizeData authorize)
		{
			if (authorize.UserId == user.Id)
			{
				if (user.IsDeleted)
				{
					databaseProvider.RemoveUser(user.Id.Value);
				}
				else
				{
					databaseProvider.UpdateUser(user);
				}
			}
			else
			{
				User user2 = databaseProvider.GetUser(user.Id.Value);
				if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, user2.Student_studentsClassId.Value))
				{
					if (user.IsDeleted)
					{
						databaseProvider.RemoveUser(user.Id.Value);
					}
					else
					{
						databaseProvider.UpdateUser(user);
					}
				}
			}
			return user;
		}

		public List<Notification> GetNotificationForUser(string LanguageKey, AuthorizeData authorizeData)
		{
			User user = databaseProvider.GetUser(authorizeData.UserId);
			return new List<Notification> { notyficationProvider.GetNotyficationForUser(LanguageKey, user) };
		}

		public List<Championship> GetActiveChampionshipsInCountry(int countryId, AuthorizeData authorize)
		{
			User user = databaseProvider.GetUser(authorize.UserId);
			return championshipsProvider.GetAllChampionshipsForUser(countryId, user);
		}

		public List<Championship> GetAllActiveChampionships(AuthorizeData authorize)
		{
			databaseProvider.GetUser(authorize.UserId);
			return championshipsProvider.GetAllChampionships();
		}

		public Main_Logic(ICountriesProvider countriesProvider, IUserDatabaseProvider databaseProvider, IQuestionResultsProvider questionResultsProvider, IQuestionEditsProvider questionEditsProvider, IToyShopProvider toyShopProvider, INotyficationProvider notyficationProvider, IChampionshipsMetadataProvider championshipsProvider, IStudentClassProvider studentClassProvider, IStudentClassExamsProvider studentClassExamsProvider, IUserCommentsProvider userCommentsProvider, ISchoolProvider schoolProvider, IBrandingProvider brandingProvider, IParentInfoHolder parentInfoProvider)
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
			this.userCommentsProvider = userCommentsProvider;
			this.schoolProvider = schoolProvider;
			this.brandingProvider = brandingProvider;
			this.parentInfoProvider = parentInfoProvider;
		}

		public List<Comment> GetAllCommentsFromStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
		{
			if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, studentsClass.Id.Value))
			{
				List<Comment> list = new List<Comment>();
				{
					foreach (User item in studentClassProvider.GetStudentsInClassForUser(authorize.UserId, studentsClass.Id.Value))
					{
						list.AddRange(userCommentsProvider.GetAllCommentsForUser(item.Id.Value));
					}
					return list;
				}
			}
			return null;
		}

		public Comment AddOrUpdateComment(Comment comment, AuthorizeData authorize)
		{
			User user = databaseProvider.GetUser(comment.UserID);
			if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, user.Student_studentsClassId.Value))
			{
				userCommentsProvider.AddOrUpdateComment(comment, comment.UserID);
			}
			return null;
		}

		public List<Comment> GetAllCommentsForUser(User user, AuthorizeData authorize)
		{
			User user2 = databaseProvider.GetUser(user.Id.Value);
			if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, user2.Student_studentsClassId.Value))
			{
				return userCommentsProvider.GetAllCommentsForUser(user.Id.Value);
			}
			return null;
		}

		public List<Exam> GetAllExamsInClass(StudentsClass studentsClass, AuthorizeData authorize)
		{
			return (from s in studentClassExamsProvider.GetAllExamsInClass(studentsClass.Id.Value)
				select s.Exam_metadata).ToList();
		}

		public List<ExamQuestion> GetAllQuestionsInAllExamsInStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
		{
			if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, studentsClass.Id.Value))
			{
				return studentClassExamsProvider.GetAllExamsInClass(studentsClass.Id.Value).SelectMany((ServerExam s) => s.questions).ToList();
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
			return studentClassExamsProvider.GetExam(exam.Id).questions;
		}

		public bool AddQuestionInExam(ExamQuestion examQuestion, AuthorizeData authorize)
		{
			int studentsClassId = studentClassExamsProvider.GetExam(examQuestion.ExamId).Exam_metadata.StudentsClassId;
			if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, studentsClassId))
			{
				studentClassExamsProvider.AddQuestionInExam(examQuestion, examQuestion.ExamId);
				return true;
			}
			return false;
		}

		public bool DeleteQuestionInExam(ExamQuestion examQuestion, AuthorizeData authorize)
		{
			int studentsClassId = studentClassExamsProvider.GetExam(examQuestion.ExamId).Exam_metadata.StudentsClassId;
			if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, studentsClassId))
			{
				studentClassExamsProvider.RemoveQuestionInExam(examQuestion, examQuestion.ExamId);
				return true;
			}
			return false;
		}

		public List<Exam> GetAllActiveExamsForStudent(User participant, AuthorizeData authorizeData)
		{
			List<Exam> list = new List<Exam>();
			if (participant.Student_studentsClassId.HasValue)
			{
				list.AddRange(from s in studentClassExamsProvider.GetAllExamsInClass(participant.Student_studentsClassId.Value)
					select s.Exam_metadata into s
					where s.IsActive
					select s);
			}
			if (participant.ChampionshipId.HasValue)
			{
				list.AddRange(from s in studentClassExamsProvider.GetChampionshipExams(participant.ChampionshipId.Value)
					select s.Exam_metadata);
			}
			return list;
		}

		public Exam UpdateOrDeleteExam(Exam exam, AuthorizeData authorize)
		{
			if (studentClassProvider.IsExamCreatedByUser(studentClassExamsProvider.GetExam(exam.Id).Exam_metadata, authorize.UserId))
			{
				ServerExam exam2 = studentClassExamsProvider.GetExam(exam.Id);
				exam2.Exam_metadata = exam;
				studentClassExamsProvider.UpdateExam(exam2);
			}
			return null;
		}

		public School AddSchool(School school, AuthorizeData authorize)
		{
			school.CreatorUserID = authorize.UserId;
			schoolProvider.AddSchool(school);
			return school;
		}

		public School GetSchool(User user, AuthorizeData authorize)
		{
			return schoolProvider.GetSchool(authorize.UserId);
		}

		public School UpdateOrDeleteSchool(School school, AuthorizeData authorize)
		{
			schoolProvider.UpdateSchool(school, authorize.UserId);
			return school;
		}

		public GetToyShopDataResult GetUserToysShopInfo(User user, AuthorizeData authorize)
		{
			ToyShopData toyShop = toyShopProvider.GetToyShop(authorize.UserId);
			if (toyShop != null)
			{
				return new GetToyShopDataResult
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

		public List<QuestionResult> GetAllQuestionsResults(User user, AuthorizeData authorize)
		{
			if (user.Id == authorize.UserId)
			{
				return questionResultsProvider.GetAllQuestionsReultsForUser(authorize.UserId);
			}
			if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, user.Student_studentsClassId.Value))
			{
				return questionResultsProvider.GetAllQuestionsReultsForUser(user.Id.Value);
			}
			return null;
		}

		public QuestionResult AddOrUpdateQuestionResult(QuestionResult questionResult, AuthorizeData authorize)
		{
			questionResultsProvider.AddOrUpdateQuestionResult(questionResult, authorize.UserId);
			return questionResult;
		}

		public StudentsClass GetStudentsClassById(int id, AuthorizeData authorize)
		{
			return studentClassProvider.GetStudentsClassByGlobalId(id);
		}

		public User AddUserToStudentsClass(StudentsClass studentsClass, User newStudent, AuthorizeData authorize)
		{
			User user = databaseProvider.GetUser(authorize.UserId);
			user.Student_studentsClassId = studentsClass.Id;
			user.Student_isStudent = true;
			user.Student_isAssignedToStudentsClass = true;
			user.Student_isAcceptedToStudentsClass = false;
			databaseProvider.UpdateUser(user);
			return user;
		}

		public User RemoveUserFromStudentsClass(User userToRemove, AuthorizeData authorize)
		{
			User user = databaseProvider.GetUser(authorize.UserId);
			user.Student_studentsClassId = null;
			user.Student_isStudent = false;
			user.Student_isAssignedToStudentsClass = false;
			user.Student_isAcceptedToStudentsClass = null;
			databaseProvider.UpdateUser(user);
			return user;
		}

		public StudentsClass EditStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
		{
			studentClassProvider.EditClassForUser(studentsClass, authorize.UserId);
			return null;
		}

		public StudentsClass DeleteStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
		{
			foreach (User item in databaseProvider.GetAllUsersBelongingToClass(studentsClass.Id.Value))
			{
				if (item.Email == null)
				{
					databaseProvider.RemoveUser(item.Id.Value);
					continue;
				}
				item.Student_isStudent = false;
				item.Student_studentsClassId = null;
				databaseProvider.UpdateUser(item);
			}
			studentClassExamsProvider.RemoveAllExamsInClass(studentsClass.Id.Value);
			studentClassProvider.RemoveClassForUser(studentsClass, authorize.UserId);
			return null;
		}

		public List<User> GetAllStudentsInClass(StudentsClass studentsClass, AuthorizeData authorize)
		{
			return studentClassProvider.GetStudentsInClassForUser(authorize.UserId, studentsClass.Id.Value);
		}

		public List<QuestionResult> GetAllResultsForStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
		{
			if (studentClassProvider.IsClassBelongsToUser(authorize.UserId, studentsClass.Id.Value))
			{
				List<QuestionResult> list = new List<QuestionResult>();
				{
					foreach (User item in studentClassProvider.GetStudentsInClassForUser(authorize.UserId, studentsClass.Id.Value))
					{
						list.AddRange(questionResultsProvider.GetAllQuestionsReultsForUser(item.Id.Value));
					}
					return list;
				}
			}
			return null;
		}

		public List<StudentsClass> GetAllStudentsClasses(int teacherID, AuthorizeData authorize)
		{
			return studentClassProvider.GetClassesForUser(authorize.UserId);
		}

		public StudentsClass AddStudentsClass(StudentsClass studentsClass, AuthorizeData authorize)
		{
			studentsClass.TeacherID = authorize.UserId;
			studentClassProvider.AddClassForUser(studentsClass, authorize.UserId);
			return studentsClass;
		}

		public AccountActivationStatus IsParentEmailActivated(int childID)
		{
			return new AccountActivationStatus
			{
				IsEmailActivated = true
			};
		}

		public ParentInfo AddOrUpdateParentInfo(ParentInfo parentInfo, AuthorizeData authorize)
		{
			parentInfoProvider.AddOrUpdateParentInfoForUser(parentInfo, authorize.UserId);
			return parentInfo;
		}
	}
}
