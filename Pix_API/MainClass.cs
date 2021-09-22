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
            kernel.Bind<ServerConfiguration>().ToConstant(serverConfiguration).InSingletonScope();
            kernel.Bind<ICommandRepository>().To<Main_Logic>();
            kernel.Bind<IAbstractUser>().To<ChampionshipsUser>();

            var resolver = kernel.Get<APIServerResolver>();
            var connectionRecever = new ConnectionRecever(serverConfiguration.server_host_ip, resolver);

            kernel.Unbind<ICommandRepository>();
            kernel.Bind<ICommandRepository>().To<ChampionshipAPICommands>();

            var championship_resolver = kernel.Get<APIServerResolver>();
			ConnectionRecever championship_server = new ConnectionRecever(serverConfiguration.championship_server_host_ip, championship_resolver);

			Task.Run(() => championship_server.Start_Lisening_Action());
			connectionRecever.Start_Lisening_Action();
		}
	}
}
