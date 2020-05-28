using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlazorQuiz.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;

namespace BlazorQuiz.Functions
{
    public static class ExamFunctions
    {
        private static readonly string AnswerCollectionId = "AnswerLog";
        private static readonly string AnswerSheetCollectionId = "AnswerSheet";

        [FunctionName("GetSampleAnswer")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            var question = new Question()
            {
                Id=1,
                Tag = "Physics",
                Title = "What is the letter of the Oxygen element?",
                Choices = new []
                {
                    new Choice() {Text = "N", IsCorrectChoice = false}, 
                    new Choice() {Text = "O", IsCorrectChoice = true}, 
                    new Choice() {Text = "C", IsCorrectChoice = false}, 
                }
            };

            var answer = new Answer();
            answer.CandidateId = Guid.NewGuid().ToString("N");
            answer.Question = question;
            answer.Choices = question.GetAnswerChoices().ToList();
            answer.AnsweredAt = DateTime.UtcNow;

            return new OkObjectResult(answer);
        }

        [FunctionName("setup")]
        public static async Task<IActionResult> Setup(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var dbName = config["COSMOSDB_DBNAME"];

            using (var client = new DocumentClient(new Uri(config["COSMOSDB_URI"]), config["COSMOSDB_KEY"]))
            {
                var response = await client.CreateDatabaseIfNotExistsAsync(new Database { Id = dbName });
                var db = response.Resource;

                DocumentCollection collectionDefinition = new DocumentCollection();
                collectionDefinition.Id = AnswerCollectionId;
                collectionDefinition.PartitionKey.Paths.Add("/AnswerId");

                DocumentCollection answerLogCollection = await client.CreateDocumentCollectionIfNotExistsAsync(
                    UriFactory.CreateDatabaseUri(dbName),
                    collectionDefinition);

                DocumentCollection collection2Definition = new DocumentCollection();
                collection2Definition.Id = AnswerSheetCollectionId;
                collection2Definition.PartitionKey.Paths.Add("/ExamId");

                DocumentCollection answer2LogCollection = await client.CreateDocumentCollectionIfNotExistsAsync(
                    UriFactory.CreateDatabaseUri(dbName),
                    collection2Definition);
            }

            return new OkObjectResult(new { Status = "Ok" });
        }

        [FunctionName("answer")]
        public static async Task<IActionResult> CollectAnswer(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "answer/{candidateId}")] Answer req,
                string candidateId,
                ILogger log,
                ExecutionContext context)
        {
            log.LogInformation($"New answer for {candidateId} : {req?.Question?.Title}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var dbName = config["COSMOSDB_DBNAME"];

            using (var client = new DocumentClient(new Uri(config["COSMOSDB_URI"]), config["COSMOSDB_KEY"]))
            {
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(dbName, AnswerCollectionId);
                await client.CreateDocumentAsync(collectionUri, req);
            }

            return new OkObjectResult(new {Status = "Ok"});
        }

        [FunctionName("endExam")]
        public static async Task<IActionResult> EndExam(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "exam/{examId}/end")] AnswerSheet req,
            string examId,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation($"End of exam for {req.Candidate.FirstName}.");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var dbName = config["COSMOSDB_DBNAME"];

            using (var client = new DocumentClient(new Uri(config["COSMOSDB_URI"]), config["COSMOSDB_KEY"]))
            {
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(dbName, AnswerSheetCollectionId);
                await client.CreateDocumentAsync(collectionUri, req);
            }

            return new OkObjectResult(new { Status = "Ok" });
        }


        [FunctionName("GetAnswers")]
        public static async Task<IActionResult> GetAnswers(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "exam/{examId}/_admin/answers")] HttpRequest req,
            string examId,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation("Request exam answers");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var dbName = config["COSMOSDB_DBNAME"];

            using (var client = new DocumentClient(new Uri(config["COSMOSDB_URI"]), config["COSMOSDB_KEY"]))
            {
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(dbName, AnswerSheetCollectionId);
                var query = $"SELECT * FROM c WHERE c.ExamId = \"{examId}\"";
                log.LogInformation(query);

                var crossPartition = new FeedOptions { EnableCrossPartitionQuery = true };
                var documentsList = client.CreateDocumentQuery(collectionUri, query, crossPartition).ToList();

                return new OkObjectResult(documentsList);
            }

            return new BadRequestResult();
        }
    }
}
