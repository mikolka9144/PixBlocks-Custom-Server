
using Ninject.Modules;
using Pix_API.Base.Utills;
using Pix_API.PixBlocks.Interfaces;

namespace Pix_API.PixBlocks.CommandResolver
{
    public class NinjectManifest:NinjectModule
    {

        public override void Load()
        {
            Bind<IAPIServerResolver>().To<APIServerResolver>();
            Bind<IAbstractUser>().To<ChampionshipsUser>().InSingletonScope();
        }
    }
}
