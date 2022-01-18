using UserInterface;
using App;

namespace Start
{
    internal class  EntryPoint
    {
        private readonly ViewController _controller;
        private readonly Bot _bot;
        
        public EntryPoint(Bot bot, ViewController controller)
        {
            _bot = bot;
            _controller = controller;
        }

        public void Run()
        {
            _controller.SubscribeOn(_bot.Register());
            _bot.SendMassage += _controller.RegisterSending();
            _controller.Start();
        }
    }
}