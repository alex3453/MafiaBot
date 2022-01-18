using System.Threading.Tasks;
using Discord;

namespace UserInterface
{
    public interface IDsLogger
    {
        Task Log(LogMessage msg);
    }
}