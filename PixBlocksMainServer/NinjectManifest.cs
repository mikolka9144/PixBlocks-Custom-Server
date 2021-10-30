using Ninject.Modules;
using Pix_API.Base.Utills;
using Pix_API.PixBlocks.MainServer;

namespace PixBlocksMainServer
{
    public class NinjectManifest:NinjectModule
    {
        public override void Load()
        {
            Bind<ICommandRepository>().To<Main_Logic>();
        }
    }
}
