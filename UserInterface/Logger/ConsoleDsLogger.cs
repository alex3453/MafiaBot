using System;
using System.Threading.Tasks;
using Discord;

namespace UserInterface
{
    public class ConsoleDsLogger : IDsLogger
    {
        public Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}