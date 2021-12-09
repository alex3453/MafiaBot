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
        }

        public void RegisterMethods()
        {
            View.ExCommand += Bot.Register();
            Bot.ExCommand += View.RegisterSending();
            Bot.DeleteUser += View.RegisterDelUser();
        }
    }
}