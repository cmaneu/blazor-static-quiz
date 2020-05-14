using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlazorQuiz.Model
{
    public class AnswerSheet
    {
        [JsonPropertyName("candidate")]
        public Candidate Candidate { get; set; }

        [JsonPropertyName("answers")]
        public List<Answer> Answers { get; set; }

        [JsonPropertyName("startedAt")]
        public DateTime StartedAt { get; set; }

        [JsonPropertyName("endedAt")]
        public DateTime EndedAt { get; set; }

        public double Score { get; set; }

        public void ComputeScore()
        {

        }
    }
}