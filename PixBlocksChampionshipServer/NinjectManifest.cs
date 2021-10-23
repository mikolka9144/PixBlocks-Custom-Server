﻿using System;
using Ninject.Modules;
using Pix_API.CoreComponents;
using Pix_API.Interfaces;
namespace PixBlocksChampionshipServer
{
    public class NinjectManifest:NinjectModule
    {
        public NinjectManifest()
        {
        }

        public override void Load()
        {
            Bind<ICommandRepository>().To<ChampionshipAPICommands>();
        }
    }
}
