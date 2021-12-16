using System;
using System.Threading.Tasks;
using App;
using App.CommandHandler;
using Discord.WebSocket;
using Mafia;
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

            container.Bind<ICommandHandler>().To<ResetGameCommand>();
            container.Bind<ICommandHandler>().To<RegPlayerCommand>();
            container.Bind<ICommandHandler>().To<StartCommand>();
            container.Bind<ICommandHandler>().To<VoteCommand>();
            container.Bind<ICommandHandler>().To<KillCommand>();

            container.Bind<IMafiaFactory>().ToFactory();
            container.Bind<IMafia>().To<MafiaGame>();
            container.Bind<IRoleDistribution>().To<SimpleRoleDist>();
            
            container.Bind<ILogger>().To<ConsoleLogger>();
            container.Bind<ITokenProvider>().To<FromEnvVarProvider>();
            container.Bind<IMessageSender>().To<MessageSender>();
            container.Bind<IAnswerParser>().To<DefaultParser>();

            return container;
        }
    }
}