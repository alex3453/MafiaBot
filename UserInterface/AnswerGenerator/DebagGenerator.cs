using CommonInteraction;

namespace UserInterface
{
    public class DebugGenerator : IAnswerGenerator
    {
        public override string GenerateAnswer(Answer answer)
        {
            return answer.AnswerType.ToString();
        }
    }
}