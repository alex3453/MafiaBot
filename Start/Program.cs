using Mafia;
using Ninject;
using NotifyInterfaces;

namespace Start
{
    internal static class Program
    {
        public static void Main()
        {
            var container = ConfigureContainer();
            var entryPoint = container.Get<EntryPoint>();
            entryPoint.View.Run();
        }

        private static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();
            container.Bind<IView>().To<View.View>();
            container.Bind<IBot>().To<Bot.Bot>();
            container.Bind<IMafia>().To<MafiaGame>();

            return container;
        }
    }
}