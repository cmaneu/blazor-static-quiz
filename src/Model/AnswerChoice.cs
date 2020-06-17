using System.Text.Json.Serialization;

namespace BlazorQuiz.Model
{
    public class AnswerChoice : Choice
    {
        [JsonIgnore]
        public int Id { get; set; }

        public bool IsSelected { get; set; }

        public bool IsCorrect => IsSelected == IsCorrectChoice;

        public static AnswerChoice FromChoice(Choice choice, int id)
        {
            var answerChoice = new AnswerChoice
            {
                Id = id,
                Text = choice.Text,
                IsCorrectChoice = choice.IsCorrectChoice,
                ImageContents = choice.ImageContents,
                Rationale = choice.Rationale
            };

            return answerChoice;
        }
    }
}
