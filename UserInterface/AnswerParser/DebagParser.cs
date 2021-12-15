using CommonInteraction;

namespace UserInterface
{
    public class DebagParser : IAnswerParser
    {
        public string ParseAnswer(Answer answer)
        {
            return answer.AnswerType.ToString();
        }
    }
}