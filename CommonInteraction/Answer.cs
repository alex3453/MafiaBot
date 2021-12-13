using System;
using System.Collections.Generic;

namespace CommonInteraction
{
    public class Answer
    {
        public readonly AnswerType AnswerType;
        public IReadOnlyList<string> Args { get; }

        public Answer(AnswerType answerType, IReadOnlyList<string> args = null)
        {
            AnswerType = answerType;
            Args = args;
        }
    }
}