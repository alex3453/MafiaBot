using System;
using System.Threading.Tasks;
using Discord.WebSocket;
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
            
            container.Bind<ICommandsHandler>().To<CommandsHandler>();
            container.Bind<ICommandParser>().To<CommandParser>();
            container.Bind<ILogger>().To<ConsoleLogger>();
            container.Bind<ITokenProvider>().To<FromEnvVarProvider>();
            container.Bind<IAnswerParser>().To<DefaultParser>();

            return container;
        }
    }
}