using System;
using System.Threading.Tasks;
using Discord;

namespace UserInterface
{
    public class ConsoleLogger : ILogger
    {
        public Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}