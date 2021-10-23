using System;
using Ninject.Modules;
using Pix_API.Interfaces;
using Pix_API.CoreComponents.ServerCommands;
namespace PixBlocksMainServer
{
    public class NinjectManifest:NinjectModule
    {
        public NinjectManifest()
        {
        }

        public override void Load()
        {
            Bind<ICommandRepository>().To<Main_Logic>();
        }
    }
}
