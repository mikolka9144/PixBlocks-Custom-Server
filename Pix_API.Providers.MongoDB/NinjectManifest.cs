using System;
using Ninject;
using Ninject.Modules;
using Pix_API.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using PixBlocks.Server.DataModels.DataModels;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using Pix_API.Providers.SinglePoolProviders;
using Pix_API.Providers.StaticProviders;
using Pix_API.Providers.ContainersProviders;
using Pix_API.Providers.MultiplePoolProviders;
using System.IO;

namespace Pix_API.Providers.MongoDB
{
    public class NinjectManifest:NinjectModule
    {
        private MongoClient mongo;

        public NinjectManifest()
        {
            mongo = new MongoClient(GetMongoAdressString());//TODO
        }

        public override void Load()
        {
            Bind<MongoClient>().ToConstant(mongo);

            var question_result_saver = new DiskDataSaver<List<QuestionResult>>("./QuestionResults/");
            var studentClassesSaver = new DiskDataSaver<List<StudentsClass>>("./Classes/");
            var comments_saver = new DiskDataSaver<List<Comment>>("./Comments/");
            var student_class_exams_saver = new DiskDataSaver<ServerExam>("./Exams/");
            var toyShop_saver = new DiskDataSaver<ToyShopData>("./ToyShops/");
            var schools_saver = new DiskDataSaver<School>("./Schools/");
            var championshipsMetadata_saver = new DiskDataSaver<Championship>("./Championships/");


            Bind<DataSaver<List<QuestionResult>>>().ToConstant(question_result_saver);
            Bind<DataSaver<List<StudentsClass>>>().ToConstant(studentClassesSaver);
            Bind<DataSaver<List<Comment>>>().ToConstant(comments_saver);
            Bind<DataSaver<ServerExam>>().ToConstant(student_class_exams_saver);
            Bind<DataSaver<School>>().ToConstant(schools_saver);
            Bind<DataSaver<ToyShopData>>().ToConstant(toyShop_saver);
            Bind<DataSaver<Championship>>().ToConstant(championshipsMetadata_saver);

            Bind<IUserDatabaseProvider>().To<MongoUserDatabaseProvider>();
            Bind<IQuestionEditsProvider>().To<MongoQuestionEditsProvider>();
            Bind<IQuestionResultsProvider>().To<MongoQuestionResultProvider>();
            Bind<IStudentClassProvider>().To<MongoStudentClassesProvider>();
            Bind<IStudentClassExamsProvider>().To<MongoStudentClassExamsProvider>();
            Bind<IUserCommentsProvider>().To<MongoUserCommentsProvider>();
            Bind<IToyShopProvider>().To<MongoToyShopProvider>();
            Bind<ISchoolProvider>().To<MongoSchoolsProvider>();

            Bind<IChampionshipsMetadataProvider>().To<ChampionshipProvider>();
            Bind<IBrandingProvider>().To<BrandingProvider>();
            Bind<ICountriesProvider>().To<CountriesProvider>();
            Bind<INotyficationProvider>().To<StaticNotyficationProvider>();
        }
        private string GetMongoAdressString()
        {
            if (!File.Exists("mongo_server_adress.cfg"))
            {
                File.WriteAllText("mongo_server_adress.cfg", "mongodb://localhost:27017/");
            }
            return File.ReadAllText("mongo_server_adress.cfg");
        }
    }
}
