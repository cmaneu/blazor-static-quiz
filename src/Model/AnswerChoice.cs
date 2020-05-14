namespace BlazorQuiz.Model
{
    public class AnswerChoice : Choice
    {
        public bool IsSelected { get; set; }

        public bool IsCorrect => IsSelected == IsCorrectChoice;

        public static AnswerChoice FromChoice(Choice choice)
        {
            var answerChoice = new AnswerChoice();
            answerChoice.Text = choice.Text;
            answerChoice.IsCorrectChoice = choice.IsCorrectChoice;
            answerChoice.ImageContents = choice.ImageContents;
            answerChoice.Rationale = choice.Rationale;

            return answerChoice;
        }
    }
}
