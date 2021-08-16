using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Pix_API.Providers;
using System.Text;
using PixBlocks.Server.DataModels.DataModels;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels.UserProfileInfo;
using Pix_API.Providers.StaticProviders;
using Pix_API.Providers.ContainersProviders;
using Pix_API.Providers.MultiplePoolProviders;
using Pix_API.CoreComponents.ServerCommands;
using Pix_API.Providers.SinglePoolProviders;
using PixBlocks.Server.DataModels.DataModels.Championsships;
using System.Reflection;
using Pix_API.CoreComponents;
using Pix_API.Interfaces;

namespace Pix_API
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var raw_countries_csv = File.ReadAllText("./countries.csv");

            var users_saver = new DiskDataSaver<User>("./Users/");
            var question_result_saver = new DiskDataSaver<List<QuestionResult>>("./QuestionResults/");
            var studentClassesSaver = new DiskDataSaver<List<StudentsClass>>("./Classes/");
            var question_edits_saver = new DiskDataSaver<List<EditedQuestionCode>>("./QuestionsCodes/");
            var comments_saver = new DiskDataSaver<List<Comment>>("./Comments/");
            var student_class_exams_saver = new DiskDataSaver<ServerExam>("./Exams/");
            var toyShop_saver = new DiskDataSaver<ToyShopData>("./ToyShops/");
            var schools_saver = new DiskDataSaver<School>("./Schools/");
            var championshipsMetadata_saver = new DiskDataSaver<Championship>("./Championships/");

            var usersProvider = new UserDatabaseProvider(users_saver);
            var questionResultProvider = new QuestionResultProvider(question_result_saver);
            var editQuestionProvider = new QuestionEditsProvider(question_edits_saver);
            var toyShopProvider = new ToyShopProvider(toyShop_saver);
            var studentClassesProvider = new StudentClassesProvider(studentClassesSaver,usersProvider);
            var studentClassExamsProvider = new StudentClassExamsProvider(student_class_exams_saver);
            var userCommentsProvider = new UserCommentsProvider(comments_saver);
            var SchoolsProvider = new SchoolsProvider(schools_saver);
            var countriesProvider = new CountriesProvider();
            var staticNotyficationProvider = new StaticNotyficationProvider();//TODO
            var staticChampionshipsProvider = new ChampionshipProvider(championshipsMetadata_saver);
            var brandingProvider = new BrandingProvider();

            var server = new ConnectionRecever();
            var resolver = new APIServerResolver(new Main_Logic(countriesProvider,
                usersProvider, questionResultProvider, editQuestionProvider,
                toyShopProvider,staticNotyficationProvider,
                staticChampionshipsProvider,studentClassesProvider,
                studentClassExamsProvider,userCommentsProvider,SchoolsProvider,brandingProvider),usersProvider);

            server.OnCommandReceved += (a, b, c) => OnCommand(a, b, c, resolver);
            server.Start_Lisening("http://*:8080/");
        }

        private static void OnCommand(string method_name, string[] parameters,HttpListenerResponse response,APIServerResolver resolver)
        {
            try
            {
                var result = resolver.Execute_API_Method(method_name, parameters);
                var formatted_result = $"\"{result.Replace("\"", "\\\"")}\"";
                var encoded_result = Encoding.UTF8.GetBytes(formatted_result);

                response.OutputStream.Write(encoded_result, 0, encoded_result.Length);
            }
            catch (TargetInvocationException ex)
            {
                response.StatusCode = 500;
                Console.WriteLine($"Exception occured: {ex.InnerException.Message}");
                if (System.Diagnostics.Debugger.IsAttached) throw ex.InnerException;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                Console.WriteLine($"Exception occured: {ex.Message}");
                if (System.Diagnostics.Debugger.IsAttached) throw;
            }
        }
    }
}
