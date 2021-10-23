using System;
using Ninject.Modules;
using Pix_API.CoreComponents;
using Pix_API;
using Pix_API.Interfaces;
namespace PixBlocksCommandResolver
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
