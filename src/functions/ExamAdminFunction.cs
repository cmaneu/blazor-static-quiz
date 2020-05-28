using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BlazorQuiz.Functions
{
    public static class ExamAdminFunction
    {
        private static readonly string AnswerSheetCollectionId = "AnswerSheet";

    }
}
