using System.Collections.Generic;
using System.Text;
using CommonInteraction;

namespace UserInterface
{
    public abstract class IAnswerGenerator
    {
        public abstract string GenerateAnswer(Answer answer);

        protected static string GenerateKillList(IReadOnlyList<string> killList)
        {
            var res = new StringBuilder();
            for (var i = 0; i < killList.Count; i += 2)
            {
                res.Append(killList[i] + " - ");
                res.Append(killList[i + 1] + "\n");
            }
            return res.ToString();
        }
    }
}