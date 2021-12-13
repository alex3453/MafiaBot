using System.Collections.Generic;
using CommonInteraction;

namespace App
{
    public abstract class Process
    {
        public bool isCommonChat { get; }
        public AnswerType type { get; }
        public List<string> answerArgs { get; }

        public (bool, Answer) ProcessAnswer()
        {
            return (isCommonChat, new Answer(type, answerArgs));
        }
    }
}