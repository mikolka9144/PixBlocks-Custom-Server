using System;
using System.Collections.Generic;
using Ninject.Modules;
using Pix_API.Interfaces;
using Pix_API.Providers.ContainersProviders;
using Pix_API.Providers.MultiplePoolProviders;
using Pix_API.Providers.SinglePoolProviders;
using Pix_API.Providers.StaticProviders;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;

namespace Pix_API.Providers
{
    public class NinfectManifest : NinjectModule
    {
        public override void Load()
        {
            DiskDataSaver<User> value = new DiskDataSaver<User>("./Users/");
            DiskDataSaver<List<QuestionResult>> value2 = new DiskDataSaver<List<QuestionResult>>("./QuestionResults/");
            DiskDataSaver<List<StudentsClass>> value3 = new DiskDataSaver<List<StudentsClass>>("./Classes/");
            DiskDataSaver<List<EditedQuestionCode>> value4 = new DiskDataSaver<List<EditedQuestionCode>>("./QuestionsCodes/");
            DiskDataSaver<List<Comment>> value5 = new DiskDataSaver<List<Comment>>("./Comments/");
            DiskDataSaver<ServerExam> value6 = new DiskDataSaver<ServerExam>("./Exams/");
            DiskDataSaver<ToyShopData> value7 = new DiskDataSaver<ToyShopData>("./ToyShops/");
            DiskDataSaver<School> value8 = new DiskDataSaver<School>("./Schools/");
            DiskDataSaver<Championship> value9 = new DiskDataSaver<Championship>("./Championships/");
            DiskDataSaver<ParentInfo> value10 = new DiskDataSaver<ParentInfo>("./ParentInfos/");
            Bind<DataSaver<User>>().ToConstant(value);
            Bind<DataSaver<List<QuestionResult>>>().ToConstant(value2);
            Bind<DataSaver<List<StudentsClass>>>().ToConstant(value3);
            Bind<DataSaver<List<EditedQuestionCode>>>().ToConstant(value4);
            Bind<DataSaver<List<Comment>>>().ToConstant(value5);
            Bind<DataSaver<ServerExam>>().ToConstant(value6);
            Bind<DataSaver<School>>().ToConstant(value8);
            Bind<DataSaver<ToyShopData>>().ToConstant(value7);
            Bind<DataSaver<Championship>>().ToConstant(value9);
            Bind<DataSaver<ParentInfo>>().ToConstant(value10);
            Bind<IUserDatabaseProvider>().To<UserDatabaseProvider>().InSingletonScope();
            Bind<IQuestionEditsProvider>().To<QuestionEditsProvider>().InSingletonScope();
            Bind<IQuestionResultsProvider>().To<QuestionResultProvider>().InSingletonScope();
            Bind<IStudentClassProvider>().To<StudentClassesProvider>().InSingletonScope();
            Bind<IStudentClassExamsProvider>().To<StudentClassExamsProvider>().InSingletonScope();
            Bind<IUserCommentsProvider>().To<UserCommentsProvider>().InSingletonScope();
            Bind<IToyShopProvider>().To<ToyShopProvider>().InSingletonScope();
            Bind<ISchoolProvider>().To<SchoolsProvider>().InSingletonScope();
            Bind<IChampionshipsMetadataProvider>().To<ChampionshipProvider>().InSingletonScope();
            Bind<IBrandingProvider>().To<BrandingProvider>().InSingletonScope();
            Bind<ICountriesProvider>().To<CountriesProvider>().InSingletonScope();
            Bind<INotyficationProvider>().To<StaticNotyficationProvider>().InSingletonScope();
            Bind<IParentInfoHolder>().To<DiskParentInfoProvider>().InSingletonScope();
        }
    }
}
