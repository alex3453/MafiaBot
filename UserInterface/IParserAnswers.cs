using CommonInteraction;

namespace UserInterface
{
    public interface IParserAnswers
    {
        string Help { get; }
        string ParseAnswer(Answer answer);
    }
}