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
			ServerConfiguration serverConfiguration = ServerConfiguration.read_configuration();
			StandardKernel standardKernel = new StandardKernel();
			standardKernel.Load(serverConfiguration.Providers_lib);

			var resolver = new APIServerResolver(standardKernel.Get<Main_Logic>(), standardKernel.Get<IUserDatabaseProvider>());
			var connectionRecever = new ConnectionRecever(serverConfiguration.server_host_ip, resolver);

			var championship_resolver = new APIServerResolver(standardKernel.Get<Main_Logic>(), standardKernel.Get<IUserDatabaseProvider>());
			ConnectionRecever championship_server = new ConnectionRecever(serverConfiguration.championship_server_host_ip, resolver);

			Task.Run(() => championship_server.Start_Lisening_Action());
			connectionRecever.Start_Lisening_Action();
		}
	}
}
