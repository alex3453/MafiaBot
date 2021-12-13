using CommonInteraction;

namespace UserInterface
{
    public interface ICommandParser
    {
        public CommandType Parse(string command);
    }
}