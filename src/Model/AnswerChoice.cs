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
            var answerChoice = new AnswerChoice();
            answerChoice.Id = id;
            answerChoice.Text = choice.Text;
            answerChoice.IsCorrectChoice = choice.IsCorrectChoice;
            answerChoice.ImageContents = choice.ImageContents;
            answerChoice.Rationale = choice.Rationale;

            return answerChoice;
        }
    }
}
