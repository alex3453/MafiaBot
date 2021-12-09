using System;
using Mafia;
using Ninject;
using UserInterface;
using App;

namespace Start
{
    internal static class Program
    {
        public static void Main()
        {
            var container = ConfigureContainer();
            var entryPoint = container.Get<EntryPoint>();
            entryPoint.RegisterMethods();
            entryPoint.View.Run();
            Console.ReadLine();
        }

        private static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();
            container.Bind<IMafia>().To<MafiaGame>();

            return container;
        }
    }
}