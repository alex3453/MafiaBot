using NotifyInterfaces;

namespace Start
{
    internal class EntryPoint
    {
        public readonly IBot Bot;
        public readonly IApp App;
        
        public EntryPoint(IBot bot, IApp app)
        {
            Bot = bot;
            App = app;
            Bot.Notify += App.Register();
        }
    }
}