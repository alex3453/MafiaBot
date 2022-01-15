using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using App;
using App.CommandHandler;
using CommonInteraction;
using Mafia;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Factory;
using Telegram.Bot;
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
            
            container.Bind<IMafia>().To<MafiaGame>();
            container.Bind<IRoleDistribution>().To<SimpleRoleDist>();
            
            container.Bind<IVisitor<BaseCommandHandler>>().To<Visitor>();
            container.Bind<IDictionaryProvider>().To<GameTeamProvider>();
            container.Bind<IMafiaFactory>().ToFactory();
            
            container.Bind(c => c.FromAssemblyContaining<BaseCommandHandler>()
                .SelectAllClasses().InheritedFrom<BaseCommandHandler>().BindAllBaseClasses());
            
            container.Bind(c => c.FromAssemblyContaining<CommandMessage>()
                .SelectAllClasses().InheritedFrom<CommandMessage>().BindAllBaseClasses());
            
            container.Bind(c => c.FromAssemblyContaining<ViewCommandMessage>()
                .SelectAllClasses().InheritedFrom<ViewCommandMessage>().BindAllBaseClasses());
            container.Bind<IParserFactory>().ToFactory();
            
            container.Bind<MessageParser>().To<MessageParser>().InSingletonScope();
            
            container.Bind<IMessageSender>().To<TgSender>().InSingletonScope();
            container.Bind<ITgErrorHandler>().To<TgConsoleErrorHandler>();
            container.Bind<IView>().To<TgView>().InSingletonScope();


            // container.Bind<IMessageSender>().To<DsSender>().InSingletonScope();
            // container.Bind<IDsLogger>().To<ConsoleDsLogger>();
            // container.Bind<IView>().To<DsView>().InSingletonScope();
            // container.Bind<DiscordSocketClient>().To<DiscordSocketClient>().InSingletonScope();

            container.Bind<string>().ToMethod(ctx => ctx.Kernel
                .Get<TgEnvVarTokenProvider>().GetToken()).WhenInjectedInto<TelegramBotClient>();
            container.Bind<HttpClient>().ToSelf().WhenInjectedInto<TelegramBotClient>();
            container.Bind<TelegramBotClient>().ToSelf().InSingletonScope()
                .WithConstructorArgument("baseUrl", "https://api.telegram.org");
            
            
            return container;
        }
    }
}