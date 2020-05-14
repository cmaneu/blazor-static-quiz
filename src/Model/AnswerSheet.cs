using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BlazorQuiz.Model
{
    public class AnswerSheet
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("examId")]
        public string ExamId { get; set; }

        [JsonPropertyName("candidate")]
        public Candidate Candidate { get; set; }

        [JsonPropertyName("answers")] 
        public List<Answer> Answers { get; set; } = new List<Answer>();

        [JsonPropertyName("startedAt")]
        public DateTime StartedAt { get; set; }

        [JsonPropertyName("endedAt")]
        public DateTime EndedAt { get; set; }

        [JsonPropertyName("log")]
        public List<string> Log { get; set; } = new List<string>();

        public float Score { get; set; }

        public void ComputeScore()
        {
            Score = Answers.Count(a => a.IsCorrect) * 100 / Answers.Count;
        }
    }
}