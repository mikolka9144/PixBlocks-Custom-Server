using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pix_API.CoreComponents
{
	public class ConnectionRecever
	{
		private readonly string adress;

		private readonly APIServerResolver resolver;

		public ConnectionRecever(string adress, APIServerResolver resolver)
		{
			this.adress = adress;
			this.resolver = resolver;
		}

		public void Start_Lisening_Action()
		{
			HttpListener httpListener = new HttpListener();
			httpListener.Prefixes.Add(adress);
			httpListener.Start();
			while (true)
			{
				HttpListenerContext context = httpListener.GetContext();
				string method_name = context.Request.QueryString["me"];
				string[] parameters =
				{
					context.Request.QueryString["p1"],
					context.Request.QueryString["p2"],
					context.Request.QueryString["p3"],
					context.Request.QueryString["p4"],
					context.Request.QueryString["p5"],
					context.Request.QueryString["p6"],
					context.Request.QueryString["p7"]
				};
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

		private void OnCommand(string method_name, string[] parameters, HttpListenerResponse response)
		{
			try
			{
				string text = resolver.Execute_API_Method(method_name, parameters);
				string s = "\"" + text.Replace("\"", "\\\"") + "\"";
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				response.OutputStream.Write(bytes, 0, bytes.Length);
			}
			catch (TargetInvocationException ex)
			{
				response.StatusCode = 500;
				Console.WriteLine("Exception occured: " + ex.InnerException.Message);
				if (Debugger.IsAttached)
				{
					throw ex.InnerException;
				}
			}
			catch (Exception ex2)
			{
				response.StatusCode = 500;
				Console.WriteLine("Exception occured: " + ex2.Message);
				if (Debugger.IsAttached)
				{
					throw;
				}
			}

		}
	}
}
