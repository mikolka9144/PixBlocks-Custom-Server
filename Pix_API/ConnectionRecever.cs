using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Pix_API.Utills;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Pix_API.CoreComponents
{
	public class ConnectionRecever
	{
		private readonly string adress;

		private readonly IAPIServerResolver resolver;
        private readonly ILogger logger;

        public ConnectionRecever(string adress, IAPIServerResolver resolver,LogFactory logger)
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
				context.Response.ContentType = "application/json";
				context.Response.ContentEncoding = Encoding.UTF8;
				context.Response.SendChunked = false;
                Task.Run(() =>
                {
                    OnCommand(method_name, parameters, context.Response);
                    context.Response.Close();
                });
            }
        }

        private string[] GetParameters(NameValueCollection queryString)
        {
            var allKeys = queryString.AllKeys.Skip(1);
            var values = new List<String>();
            foreach (var item in allKeys)
            {
                values.Add(queryString[item]);
            }
            return values.ToArray();
        }

        private void OnCommand(string method_name, string[] parameters, HttpListenerResponse response)
		{
			try
			{
				string text = resolver.Execute_API_Method(method_name, parameters);
				
				byte[] bytes = Encoding.UTF8.GetBytes(text);
				response.OutputStream.Write(bytes, 0, bytes.Length);
			}
			catch (TargetInvocationException ex)
			{
				response.StatusCode = 500;
				logger.Error(ex,"Exception occured: ");
				if (Debugger.IsAttached)
				{
					throw ex.InnerException;
				}
			}
			catch (Exception ex2)
			{
				response.StatusCode = 500;
                logger.Error(ex2,"Exception occured: ");
                if (Debugger.IsAttached)
				{
					throw;
				}
			}

		}
	}
}
