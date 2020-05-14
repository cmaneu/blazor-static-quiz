using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlazorQuiz.Model
{
    public class Exam
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("examTitle")]
        public string ExamTitle { get; set; }

        [JsonPropertyName("examImage")]
        public string ExamImage { get; set; }

        [JsonPropertyName("accessCode")]
        public string AccessCode { get; set; }

        [JsonPropertyName("questions")]
        public List<Question> Questions { get; set; }
    }
}