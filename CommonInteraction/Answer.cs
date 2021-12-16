using System;
using System.Collections.Generic;

namespace CommonInteraction
{
    public class Answer
    {
        public readonly bool IsCommon;
        public readonly AnswerType AnswerType;
        public readonly IReadOnlyList<string> Args;

        public Answer(bool isCommon, AnswerType answerType, params string[] args)
        {
            AnswerType = answerType;
            IsCommon = isCommon;
            Args = args;
        }
    }
}