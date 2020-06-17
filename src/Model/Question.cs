using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlazorQuiz.Model
{
    public partial class Question
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("imageData")]
        public string ImageContents { get; set; }

        [JsonPropertyName("tag")]
        public string Tag { get; set; }

        [JsonPropertyName("choices")]
        public Choice[] Choices { get; set; }

        [JsonPropertyName("canGoBack")] 
        public bool CanGoBack { get; set; } = true;

        [JsonPropertyName("canGoForward")]
        public bool CanGoForward { get; set; } = true;

        [JsonPropertyName("timeLimit")]
        public int TimeLimit { get; set; }

        public IEnumerable<AnswerChoice> GetAnswerChoices()
        {
            int id = 1;
            foreach (Choice choice in Choices)
            {
                yield return AnswerChoice.FromChoice(choice, id);
                id+= 1;
            }
        }
    }
}