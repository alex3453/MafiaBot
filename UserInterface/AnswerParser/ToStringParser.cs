using CommonInteraction;

namespace UserInterface
{
    public class ToStringParser : IAnswerParser
    {
        public string ParseAnswer(Answer answer)
        {
            return answer.AnswerType.ToString();
        }
    }
}