namespace BlazorQuiz.Model
{
    public class AnswerChoice : Choice
    {
        public bool IsSelected { get; set; }

        public bool IsCorrect => IsSelected == IsCorrectChoice;
    }
}
