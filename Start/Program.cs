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
            container.Bind<IMessageHandler>().To<MessageHandler>();
            container.Bind<IMessageParser>().To<MessageParser>();
            
            container.Bind<CommandMessage>().To<HelpMessage>();
            container.Bind<CommandMessage>().To<RegMessage>();
            container.Bind<CommandMessage>().To<ResetGameMessage>();
            container.Bind<CommandMessage>().To<StartMessageMessage>();
            container.Bind<CommandMessage>().To<KillMessage>();
            container.Bind<CommandMessage>().To<VoteMessage>();
            
            container.Bind<ILogger>().To<ConsoleLogger>();
            container.Bind<ITokenProvider>().To<FromEnvVarProvider>();
            container.Bind<IMessageSender>().To<MessageSender>();
            container.Bind<IAnswerParser>().To<DefaultParser>();

            return container;
        }
    }
}