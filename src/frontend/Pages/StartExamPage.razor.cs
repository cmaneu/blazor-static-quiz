using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorQuiz.Model;
using BlazorQuiz.FrontEnd.Services;
using System.Net.Http;

namespace BlazorQuiz.FrontEnd.Pages
{
    public partial class StartExamPage
    {
        private CurrentPageState PageState { get; set; } = CurrentPageState.Identity;

        [Parameter]
        public string ExamId { get; set; }

        public bool ExamLoadFailed { get; set; } = false;

        public string AccessCode { get; set; }

        public Candidate CurrentCandidate { get; set; } = new Candidate();

        [Inject]
        protected ExamService ExamService { get; set; } 

        [Inject]
        protected HttpClient Http { get; set; }

        [Inject]
        protected AppState AppState { get; set; }

        [Inject]
        protected NavigationManager NavManager { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (!firstRender)
                return;

            try
            {
                await ExamService.LoadExam(ExamId);

                StateHasChanged();
            }
            catch (Exception e)
            {
                ExamLoadFailed = true;
            }
        }

        private void KeyPressed()
        {
            if (AccessCode == ExamService.CurrentExam.AccessCode)
            {
                StartExam();
            }
        }

        private void StartExam()
        {
            AppState.CurrentCandidate = CurrentCandidate;
            NavManager.NavigateTo("/exam");
        }

        protected void NextPageState()
        {
            switch (PageState)
            {
                case CurrentPageState.Identity:
                    PageState = CurrentPageState.Picture;
                    break;
                case CurrentPageState.Picture:
                    PageState = CurrentPageState.Contract;
                    break;
                case CurrentPageState.Contract:
                    PageState = CurrentPageState.StartExam;
                    break;
                default:
                    break;
            }
        }

        private enum CurrentPageState
        {
            Identity,
            Picture,
            Contract,
            StartExam
        }
    }


}
