using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorQuiz.Model;

namespace BlazorQuiz.FrontEnd.Services
{
    public class ExamService
    {
        private HttpClient _httpClient;
        private ClientAppSettings _appSettings;
        private AppState _appState;

        public AnswerSheet AnswerSheet { get; protected set; }

        public Exam CurrentExam { get; set; }

        public Question CurrentQuestion { get; set; }

        public ExamService(HttpClient httpClient, ClientAppSettings appSettings, AppState appState)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;
            _appState = appState;
        }

        public async Task LoadExam(string examId)
        {
            CurrentExam = await _httpClient.GetFromJsonAsync<Exam>($"exam-data/{examId}.json");
            _appState.CurrentExamTitle = CurrentExam.ExamTitle;
            _appState.CurrentExamLogo = CurrentExam.ExamImage;
            _appState.NotifyStateChanged();
            AnswerSheet = new AnswerSheet();
        }

        public async Task<List<AnswerChoice>> LoadNextQuestionChoices()
        {
            CurrentQuestion = CurrentQuestion == null 
                ? CurrentExam.Questions.FirstOrDefault() 
                : CurrentExam.Questions.ElementAtOrDefault(CurrentExam.Questions.IndexOf(CurrentQuestion) + 1);

            return CurrentQuestion?.GetAnswerChoices().ToList();
        }

        public async void SubmitQuestionAnswer(Answer answer)
        {
            if (AnswerSheet.Candidate == null)
            {
                AnswerSheet.Candidate = _appState.CurrentCandidate;
            }

            if (AnswerSheet.StartedAt == DateTime.MinValue)
            {
                AnswerSheet.StartedAt = answer.AnsweredAt;
            }
            
            AnswerSheet.Answers.Add(answer);
            Task.Run(async () =>
            {
                await _httpClient.PostAsJsonAsync($"{_appSettings.ApiBaseUrl}answer/{_appState.CurrentCandidate.Email}",
                    answer);
            } );
        }

        public async Task EndExam()
        {
            AnswerSheet.EndedAt = DateTime.UtcNow;
            AnswerSheet.ComputeScore();
        }
    }
}
