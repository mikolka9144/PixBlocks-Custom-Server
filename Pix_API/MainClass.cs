using System;
using System.Threading.Tasks;
using Ninject;
using Ninject.Parameters;
using Pix_API.CoreComponents;
using Pix_API.CoreComponents.ServerCommands;
using Pix_API.Interfaces;

namespace Pix_API
{
	internal class MainClass
	{
		public static void Main(string[] args)
		{
           
			ServerConfiguration serverConfiguration = ServerConfiguration.Read_configuration();
			StandardKernel kernel = new StandardKernel();
			kernel.Load(serverConfiguration.Providers_lib);

            var security = kernel.Get<SecurityChecks>();

            var resolver = new APIServerResolver(kernel.Get<Main_Logic>(), kernel.Get<IUserDatabaseProvider>(),security);
			var connectionRecever = new ConnectionRecever(serverConfiguration.server_host_ip, resolver);

			var championship_resolver = new APIServerResolver(kernel.Get<Main_Logic>(), kernel.Get<IUserDatabaseProvider>(),security);
			ConnectionRecever championship_server = new ConnectionRecever(serverConfiguration.championship_server_host_ip, resolver);

			Task.Run(() => championship_server.Start_Lisening_Action());
			connectionRecever.Start_Lisening_Action();
		}
	}
}
