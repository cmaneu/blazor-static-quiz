using System.Collections.Generic;

namespace BlazorQuiz.Model
{
    public class Answer
    {
        public Question Question { get; set; }
        public List<AnswerChoice> Choices { get; set; }
        public bool IsCorrect => Choices.TrueForAll(a => a.IsCorrect);
    }
}