using NotifyInterfaces;

namespace Start
{
    internal class EntryPoint
    {
        public readonly IView View;
        public readonly IBot Bot;
        
        public EntryPoint(IView view, IBot bot)
        {
            View = view;
            Bot = bot;
            View.Notify += Bot.Register();
        }
    }
}