using CommonInteraction;

namespace UserInterface
{
    public class DebugParser : IAnswerParser
    {
        public override string ParseAnswer(Answer answer)
        {
            return answer.AnswerType.ToString();
        }
    }
}