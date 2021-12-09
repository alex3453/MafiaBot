using System;
using System.Collections.Generic;

namespace CommonInteraction
{
    public class Answer
    {
        public AnswerType AnswerType { get; }
        public IReadOnlyList<string> Args { get; }

        public Answer(AnswerType answerType, IReadOnlyList<string> args = null)
        {
            AnswerType = answerType;
            Args = args;
        }
    }
}