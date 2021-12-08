using UserInterface;
using App;

namespace Start
{
    internal class EntryPoint
    {
        public readonly View View;
        public readonly Bot Bot;
        
        public EntryPoint(View view, Bot bot)
        {
            View = view;
            Bot = bot;
            View.Notify += Bot.Register();
            Bot.Notify += View.Register();
        }
    }
}