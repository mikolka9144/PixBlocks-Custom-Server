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
using Ninject;

namespace Pix_API
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var kerner = new StandardKernel();
            kerner.Load("./Pix_API.Providers.dll");

            var server = new ConnectionRecever();
            var resolver = kerner.Get<APIServerResolver>();

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
