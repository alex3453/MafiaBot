using CommonInteraction;

namespace UserInterface
{
    public class DebugGenerator : AnswerGenerator
    {
        public override string GenerateAnswer(Answer answer)
        {
            return answer.AnswerType.ToString();
        }
    }
}