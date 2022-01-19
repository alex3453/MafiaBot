using System;
using System.Net.Http;
using App;
using App.CommandHandler;
using CommonInteraction;
using Discord.WebSocket;
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
            
            container.Bind<IMafia>().To<MafiaGame>();
            container.Bind<IRoleDistribution>().To<SimpleRoleDist>();
            
            container.Bind<IVisitor<BaseCommandHandler>>().To<Visitor>();
            container.Bind<IMafiaFactory>().ToFactory();
            
            container.Bind(c => c.FromAssemblyContaining<BaseCommandHandler>()
                .SelectAllClasses().InheritedFrom<BaseCommandHandler>().BindAllBaseClasses());
            
            container.Bind(c => c.FromAssemblyContaining<CommandMessage>()
                .SelectAllClasses().InheritedFrom<CommandMessage>().BindAllBaseClasses());
            
            container.Bind(c => c.FromAssemblyContaining<AbstractViewComMessage>()
                .SelectAllClasses().InheritedFrom<AbstractViewComMessage>().BindAllBaseClasses());
            
            container.Bind<IParserFactory>().ToFactory();
            
            container.Bind<MessageParser>().To<MessageParser>().InSingletonScope();
            
            container.Bind(c => c.FromAssemblyContaining<IView>()
                .SelectAllClasses().InheritedFrom<IView>().BindAllInterfaces());
            
            container.Bind<ITgErrorHandler>().To<TgConsoleErrorHandler>();
            container.Bind<string>().ToMethod(ctx => ctx.Kernel
                .Get<TgEnvVarTokenProvider>().GetToken()).WhenInjectedInto<TelegramBotClient>();
            container.Bind<HttpClient>().ToSelf().WhenInjectedInto<TelegramBotClient>();
            container.Bind<TelegramBotClient>().ToSelf().InSingletonScope()
                .WithConstructorArgument("baseUrl", "https://api.telegram.org");
            container.Bind<TgSender>().ToSelf().InSingletonScope();
            container.Bind<IMessageSender>().ToMethod(ctx => ctx.Kernel.Get<TgSender>());

            container.Bind<IDsLogger>().To<ConsoleDsLogger>();
            container.Bind<DiscordSocketClient>().ToSelf().InSingletonScope();
            container.Bind<DsSender>().ToSelf().InSingletonScope();
            container.Bind<IMessageSender>().ToMethod(ctx => ctx.Kernel.Get<DsSender>());

            return container;
        }
    }
}