using System;
using System.Reflection;
using System.Threading.Tasks;
using App;
using App.CommandHandler;
using CommonInteraction;
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
            container.Bind<IMessageHandler>().To<MessageHandler>().InSingletonScope();
            container.Bind<IMessageParser>().To<MessageParser>().InSingletonScope();
            container.Bind<IMessageSender>().To<MessageSender>().InSingletonScope();

            container.Bind(c => c.FromAssemblyContaining<CommandMessage>()
                .SelectAllClasses().InheritedFrom<CommandMessage>().BindAllBaseClasses());

            container.Bind(c => c.FromAssemblyContaining<ViewCommandMessage>()
                .SelectAllClasses().InheritedFrom<ViewCommandMessage>().BindAllBaseClasses());

            container.Bind(c => c.FromAssemblyContaining<ICommandHandler>()
                .SelectAllClasses().InheritedFrom<ICommandHandler>().BindAllBaseClasses());

            container.Bind<IDictionaryProvider>().To<GameTeamProvider>();
            container.Bind<IVisitor<ICommandHandler>>().To<Visitor>();

            container.Bind<IMafiaFactory>().ToFactory();
            container.Bind<IParserFactory>().ToFactory();
            
            container.Bind<IMafia>().To<MafiaGame>();
            container.Bind<IRoleDistribution>().To<SimpleRoleDist>();
            
            container.Bind<ILogger>().To<ConsoleLogger>();
            container.Bind<ITokenProvider>().To<FromEnvVarProvider>();
            container.Bind<IAnswerParser>().To<BalabobaParser>();
            container.Bind<IAnswerParser>().To<DefaultParser>(); 


            return container;
        }
    }
}