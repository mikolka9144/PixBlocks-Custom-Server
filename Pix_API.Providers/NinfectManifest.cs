using System;
using Ninject.Modules;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using Ninject.Activation;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using Pix_API.Providers.ContainersProviders;
using Pix_API.Providers.MultiplePoolProviders;
using Pix_API.Providers.SinglePoolProviders;
using Pix_API.Providers.StaticProviders;

namespace Pix_API.Providers
{
    public class NinfectManifest:NinjectModule
    {

        public override void Load()
        {
            var users_saver = new DiskDataSaver<User>("./Users/");
            var question_result_saver = new DiskDataSaver<List<QuestionResult>>("./QuestionResults/");
            var studentClassesSaver = new DiskDataSaver<List<StudentsClass>>("./Classes/");
            var question_edits_saver = new DiskDataSaver<List<EditedQuestionCode>>("./QuestionsCodes/");
            var comments_saver = new DiskDataSaver<List<Comment>>("./Comments/");
            var student_class_exams_saver = new DiskDataSaver<ServerExam>("./Exams/");
            var toyShop_saver = new DiskDataSaver<ToyShopData>("./ToyShops/");
            var schools_saver = new DiskDataSaver<School>("./Schools/");
            var championshipsMetadata_saver = new DiskDataSaver<Championship>("./Championships/");

            Bind<DataSaver<User>>().ToConstant(users_saver);
            Bind<DataSaver<List<QuestionResult>>>().ToConstant(question_result_saver);
            Bind<DataSaver<List<StudentsClass>>>().ToConstant(studentClassesSaver);
            Bind<DataSaver<List<EditedQuestionCode>>>().ToConstant(question_edits_saver);
            Bind<DataSaver<List<Comment>>>().ToConstant(comments_saver);
            Bind<DataSaver<ServerExam>>().ToConstant(student_class_exams_saver);
            Bind<DataSaver<School>>().ToConstant(schools_saver);
            Bind<DataSaver<ToyShopData>>().ToConstant(toyShop_saver);
            Bind<DataSaver<Championship>>().ToConstant(championshipsMetadata_saver);
            Bind<IUserDatabaseProvider>().To<UserDatabaseProvider>();
            Bind<IQuestionEditsProvider>().To<QuestionEditsProvider>();
            Bind<IQuestionResultsProvider>().To<QuestionResultProvider>();
            Bind<IStudentClassProvider>().To<StudentClassesProvider>();
            Bind<IStudentClassExamsProvider>().To<StudentClassExamsProvider>();
            Bind<IUserCommentsProvider>().To<UserCommentsProvider>();
            Bind<IToyShopProvider>().To<ToyShopProvider>();
            Bind<ISchoolProvider>().To<SchoolsProvider>();
            Bind<IChampionshipsMetadataProvider>().To<ChampionshipProvider>();
            Bind<IBrandingProvider>().To<BrandingProvider>();
            Bind<ICountriesProvider>().To<CountriesProvider>();
            Bind<INotyficationProvider>().To<StaticNotyficationProvider>();
        }
    }
}
