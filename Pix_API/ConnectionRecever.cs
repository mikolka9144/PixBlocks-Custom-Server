using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using Pix_API.Base.Utills;

namespace Pix_API.PixBlocks
{
    public class ConnectionRecever
    {
        private readonly string adress;

        private readonly IAPIServerResolver resolver;
        private readonly ILogger logger;

        public ConnectionRecever(string adress, IAPIServerResolver resolver, LogFactory logger)
        {
            this.adress = adress;
            this.resolver = resolver;
            this.logger = logger.GetLogger(LogsNames.CONNECTION_RECEVER);
        }

        public void Start_Lisening_Action()
        {
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add(adress);
            httpListener.Start();
            logger.Info($"Lisning on {adress}");
            while (true)
            {
                HttpListenerContext context = httpListener.GetContext();
                string method_name = context.Request.QueryString[0];
                string[] parameters = GetParameters(context.Request.QueryString);
                //var body = ReadBody(context.Request.InputStream, Convert.ToInt32(context.Request.ContentLength64));
                var body = new StreamReader(context.Request.InputStream).ReadToEnd();
                context.Response.ContentType = "application/json";
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.SendChunked = false;
                Task.Run(() =>
                {
                    OnCommand(method_name, parameters, body, context.Response);
                    context.Response.Close();
                });
            }
        }



        private string[] GetParameters(NameValueCollection queryString)
        {
            var allKeys = queryString.AllKeys.Skip(1);
            var values = new List<string>();
            foreach (var item in allKeys)
            {
                values.Add(queryString[item]);
            }
            return values.ToArray();
        }

        private void OnCommand(string method_name, string[] parameters, string body, HttpListenerResponse response)
        {
            try
            {
                string text = resolver.Execute_API_Method(method_name, parameters, body);

                byte[] bytes = Encoding.UTF8.GetBytes(text);
                response.OutputStream.Write(bytes, 0, bytes.Length);
            }
            catch (TargetInvocationException ex)
            {
                response.StatusCode = 500;
                logger.Error($"Exception occured: {ex.InnerException.Message}");
            }
            catch (Exception ex2)
            {
                response.StatusCode = 500;
                logger.Error($"Exception occured: {ex2.Message}");
            }
        }
    }
}
