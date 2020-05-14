using System;
using System.IO;
using System.Threading.Tasks;
using BlazorQuiz.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BlazorQuiz.Functions
{
    public static class ExamGeneration
    {
        [FunctionName("ExamGeneration")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var exam = new Exam();
            exam.Id = Guid.NewGuid().ToString("N");
            exam.ExamTitle = "Elementary Physics";
            exam.AccessCode = "47854";
            
            var q1 = new Question()
            {
                Id = 1,
                Tag = "quantum",
                Title = "How many states can have a qbit?",
                Choices = new []
                {
                    new Choice() {Text = "One", IsCorrectChoice = false}, 
                    new Choice() {Text = "Two", IsCorrectChoice = false}, 
                    new Choice() {Text = "Four", IsCorrectChoice = false}, 
                    new Choice() {Text = "Four at the same time", IsCorrectChoice = true}, 
                }
            };
            exam.Questions.Add(q1);



            var q2 = new Question()
            {
                Id = 2,
                Tag = "physics",
                Title = "When you put a pressure on a perfect gaz, how the volume is affected?",
                Choices = new[]
                {
                    new Choice() {Text = "The volume augments", IsCorrectChoice = true},
                    new Choice() {Text = "The volume reduces", IsCorrectChoice = false},
                    new Choice() {Text = "Nothing", IsCorrectChoice = false},
                }
            };
            exam.Questions.Add(q2);

            return new OkObjectResult(exam);
        }
    }
}
