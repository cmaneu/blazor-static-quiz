using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlazorQuiz.Model
{
    public class Answer
    {

        public string AnswerId => CandidateId + Question.Id;

        [JsonPropertyName("candidateId")]
        public string CandidateId { get; set; }
        
        [JsonPropertyName("question")]
        public Question Question { get; set; }
        
        [JsonPropertyName("choices")]
        public List<AnswerChoice> Choices { get; set; }

        [JsonPropertyName("answeredAt")]
        public DateTime AnsweredAt { get; set; }

        [JsonIgnore]
        public bool IsCorrect
        {
            get
            {
                return Choices != null && Choices.TrueForAll(a => a.IsCorrect);
            }
        }
    }
}