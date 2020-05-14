using System.Text.Json.Serialization;

namespace BlazorQuiz.Model
{
    public partial class Choice
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("imageData")]
        public string ImageContents { get; set; }

        [JsonPropertyName("correct")]
        public bool IsCorrectChoice { get; set; }

        [JsonPropertyName("rationale")]
        public string Rationale { get; set; }
    }
}