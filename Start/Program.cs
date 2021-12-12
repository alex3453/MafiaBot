using System;
using Ninject;
using UserInterface;

namespace Start
{
    internal static class Program
    {
        public static void Main()
        {
            var container = ConfigureContainer();
            var entryPoint = container.Get<EntryPoint>();
            entryPoint.Run();
            Console.ReadLine();
        }

        private static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();
            container.Bind<ITokenProvider>().To<FromEnvVarProvider>();

            return container;
        }
    }
}