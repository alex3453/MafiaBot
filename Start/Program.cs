using System;
using Mafia;
using Ninject;
using NotifyInterfaces;

namespace Start
{
    internal static class Program
    {
        private static void Main()
        {
            var container = ConfigureContainer();
            var entryPoint = container.Get<EntryPoint>();
            entryPoint.Bot.Run();
        }
        
        public static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();
            container.Bind<IBot>().To<Bot.Bot>();
            container.Bind<IApp>().To<App.App>();
            container.Bind<IMafia>().To<MafiaGame>();

            return container;
        }
    }
}