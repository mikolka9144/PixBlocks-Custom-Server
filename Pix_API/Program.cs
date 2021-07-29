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

namespace Pix_API
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var countriesProvider = new CountriesProvider();

            var users_saver = new DiskDataSaver<User>("./Users/");
            var question_result_saver = new DiskDataSaver<List<QuestionResult>>("./QuestionResults/");
            var question_edits_saver = new DiskDataSaver<List<EditedQuestionCode>>("./QuestionsCodes/");
            var toyShop_saver = new DiskDataSaver<ToyShopData>("./ToyShops/");

            var usersProvider = new UserDatabaseProvider(users_saver);
            var questionResultProvider = new QuestionResultProvider(question_result_saver);
            var editQuestionProvider = new QuestionEditsProvider(question_edits_saver);
            var toyShopProvider = new ToyShopProvider(toyShop_saver);
            var staticNotyficationProvider = new StaticNotyficationProvider();//TODO
            var staticChampionshipsProvider = new StaticChampionshipProvider();//TODO

            var resolver = new APIServerResolver(new Main_Logic(countriesProvider,
                usersProvider, questionResultProvider, editQuestionProvider,
                toyShopProvider,staticNotyficationProvider,staticChampionshipsProvider));

            Start_Lisening(resolver);
        }

        private static void Start_Lisening(APIServerResolver resolver)
        {
            var lisner = new HttpListener();
            lisner.Prefixes.Add("http://*:8080/");
            lisner.Start();
            while (true)
            {
                var request = lisner.GetContext();
                var server_fasade_method_name = request.Request.QueryString["me"];
                var parameters = new string[]{
                    request.Request.QueryString["p1"].CleanPixString(),
                    request.Request.QueryString["p2"].CleanPixString(),
                    request.Request.QueryString["p3"].CleanPixString(),
                    request.Request.QueryString["p4"].CleanPixString(),
                    request.Request.QueryString["p5"].CleanPixString(),
                    request.Request.QueryString["p6"].CleanPixString(),
                    request.Request.QueryString["p7"].CleanPixString()
                    };
                request.Response.ContentType = "application/json";
                request.Response.ContentEncoding = Encoding.UTF8;
                request.Response.SendChunked = false;
                try
                {
                    var result = resolver.Execute_API_Method(server_fasade_method_name, parameters);
                    var formatted_result = $"\"{result.Replace("\"", "\\\"")}\"";
                    var encoded_result = Encoding.UTF8.GetBytes(formatted_result);

                    request.Response.OutputStream.Write(encoded_result, 0, encoded_result.Length);
                    request.Response.Close();
                }
                catch (Exception)
                {
                    request.Response.StatusCode = 500;
                    request.Response.Close();
                }

            }
        }
    }
}
