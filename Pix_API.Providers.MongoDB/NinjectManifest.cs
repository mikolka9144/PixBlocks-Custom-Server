using System;
using System.IO;
using MongoDB.Driver;
using Ninject.Modules;
using Pix_API.Interfaces;
using Pix_API.Providers.MongoDB.Providers;
using Pix_API.Providers.StaticProviders;

namespace Pix_API.Providers.MongoDB
{
	public class NinjectManifest : NinjectModule
	{
		private static MongoClient mongo = new MongoClient(GetMongoAdressString());


		public override void Load()
		{
			Bind<IMongoDatabase>().ToConstant(mongo.GetDatabase("Pix"));
			Bind<IMongoCollection<LastIndexHolder>>().ToConstant(mongo.GetDatabase("Pix").GetCollection<LastIndexHolder>("indexes"));
			Bind<IUserDatabaseProvider>().To<MongoUserDatabaseProvider>().InSingletonScope();
			Bind<IQuestionEditsProvider>().To<MongoQuestionEditsProvider>().InSingletonScope();
			Bind<IQuestionResultsProvider>().To<MongoQuestionResultProvider>().InSingletonScope();
			Bind<IStudentClassProvider>().To<MongoStudentClassesProvider>().InSingletonScope();
			Bind<IStudentClassExamsProvider>().To<MongoStudentClassExamsProvider>().InSingletonScope();
			Bind<IUserCommentsProvider>().To<MongoUserCommentsProvider>().InSingletonScope();
			Bind<IToyShopProvider>().To<MongoToyShopProvider>().InSingletonScope();
			Bind<ISchoolProvider>().To<MongoSchoolsProvider>().InSingletonScope();
			Bind<IParentInfoHolder>().To<MongoParentInfoProvider>().InSingletonScope();
			Bind<IChampionshipsMetadataProvider>().To<MongoChampionshipProvider>();
			Bind<IBrandingProvider>().To<BrandingProvider>();
			Bind<ICountriesProvider>().To<CountriesProvider>();
			Bind<INotyficationProvider>().To<StaticNotyficationProvider>();
		}

		private static string GetMongoAdressString()
		{
			if (!File.Exists("mongo_server_adress.cfg"))
			{
				File.WriteAllText("mongo_server_adress.cfg", "mongodb://localhost:27017/");
                Console.WriteLine("New MongoDB adress file has been created. Make sure that adress is correct and restart server.");
                Environment.Exit(0);
            }
			return File.ReadAllText("mongo_server_adress.cfg");
		}
	}
}
