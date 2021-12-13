using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Factory;
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

            container.Bind<DiscordSocketClient>().To<DiscordSocketClient>().InSingletonScope();
            container.Bind<ICommandsHandler>().To<CommandsHandler>();
            container.Bind<ICommandParser>().To<CommandParser>();
            // container.Bind(c => c.FromThisAssembly().SelectAllClasses()
            //     .InheritedFrom<Command>().BindAllBaseClasses());
            container.Bind<Command>().To<HelpCommand>();
            container.Bind<ILogger>().To<ConsoleLogger>();
            container.Bind<ITokenProvider>().To<FromEnvVarProvider>();
            container.Bind<IMessageSender>().To<MessageSender>();
            container.Bind<IAnswerParser>().To<DefaultParser>();

            return container;
        }
    }
}