using System.Collections.Generic;

namespace BlazorQuiz.Model
{
    public class AnswerSheet
    {
        public Candidate Candidate { get; set; }

        public List<Answer> Answers { get; set; }

        public double Score { get; set; }

        public void ComputeScore()
        {

        }
    }
}