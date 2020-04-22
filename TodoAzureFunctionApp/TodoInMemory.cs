using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using System.Linq;

namespace TodoAzureFunctionApp
{
    public static class TodoInMemory
    {

        private static List<Todo> Items = new List<Todo>();

        [FunctionName("Get_TodoInMemory")]
        public static IActionResult GetTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Hämtar todo list items");

            return new OkObjectResult(Items);
        }

        [FunctionName("Get_TodoInMemoryById")]
        public static IActionResult GetTodoById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id}")] HttpRequest req,
            ILogger log, string id)
        {

            var todo = Items.FirstOrDefault(t => t.Id == id);
            if (todo == null) 
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(Items);
        }

        [FunctionName("Create_TodoInMemory")]

        public static async Task<IActionResult> CreateTodo([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")] HttpRequest req, ILogger logger)
        {
            logger.LogInformation("Skapar en ny Todo!");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // Plockar ut task description ur bodyn
            var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

            var todo = new Todo() { TaskDescription = input.TaskDescription};
            Items.Add(todo);
            return new OkObjectResult(todo);
        }

        [FunctionName("Update_TodoInMemory")]

        public static async Task<IActionResult> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo/{id}")]HttpRequest req,
            ILogger logger, string id) 
        {
            var todo = Items.FirstOrDefault(t => t.Id == id);

            if (todo == null) 
            {
                return new NotFoundResult();
            }

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TodoUpdateModel>(requestBody);

            todo.IsCompleted = updated.IsCompleted;
            return new OkObjectResult(todo);
        }

        [FunctionName("Delete_TodoInMemory")]

        public static IActionResult DeleteTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo/{id}")]HttpRequest req,
            ILogger logger, string id)
        {
            var todo = Items.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }
            Items.Remove(todo);
            return new OkResult();
        }


    }
}
