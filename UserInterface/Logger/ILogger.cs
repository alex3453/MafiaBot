using System.Threading.Tasks;
using Discord;

namespace UserInterface
{
    public interface ILogger
    {
        Task Log(LogMessage msg);
    }
}