using System.Threading.Tasks;
using UserInterface;
using App;

namespace Start
{
    internal class EntryPoint
    {
        private readonly IView view;
        private readonly Bot bot;
        
        public EntryPoint(IView view, Bot bot)
        {
            this.view = view;
            this.bot = bot;
        }

        public async Task Run()
        {
            view.SubscribeOn(bot.Register());
            bot.SendMassage += view.RegisterSending();
            await view.StartAsync();
        }
    }
}