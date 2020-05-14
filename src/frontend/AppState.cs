using System;
using BlazorQuiz.Model;

namespace BlazorQuiz.FrontEnd
{
    public class AppState
    {
        public bool IsExamLoaded { get; set; } = false;

        public string CurrentExamTitle { get; set; }

        public string CurrentExamLogo { get; set; }

        public Candidate CurrentCandidate { get; set; }

        public event Action OnChange;

        public void NotifyStateChanged() => OnChange?.Invoke();

    }
}
