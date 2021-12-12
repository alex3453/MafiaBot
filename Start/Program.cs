using System;
using System.Threading.Tasks;
using Ninject;
using UserInterface;

namespace Start
{
    internal static class Program
    {
        public static async Task Main()
        {
            var container = ConfigureContainer();
            var entryPoint = container.Get<EntryPoint>();
            await entryPoint.Run();
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