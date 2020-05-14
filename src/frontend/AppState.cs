using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorQuiz.FrontEnd
{
    public class AppState
    {
        public bool IsExamLoaded { get; set; } = false;

        public string CurrentExamTitle { get; set; }
        
        public event Action OnChange;

        public void NotifyStateChanged() => OnChange?.Invoke();

    }
}
