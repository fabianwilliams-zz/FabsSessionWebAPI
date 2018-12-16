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
using System.Linq;
using Newtonsoft.Json.Linq;

namespace fabsesrev
{
    public static class FabsSessionAndReview
    {
        static List<FabsSession> fs = new List<FabsSession>();
        static List<SessionReview> sr = new List<SessionReview>();

        [FunctionName("CreateSession")]
        public static async Task<IActionResult> CreateSession(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sesrev")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating a new Session that Fabian will or have give");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic input = JsonConvert.DeserializeObject<CreateSession>(requestBody);
            
            var sesrev = new FabsSession()
            {
                SessionName = input.SessionName,
                SessionDate = input.SessionDate,
                SessionCity = input.SessionCity,
                SessionRegionState = input.SessionRegionState,
                SessionCountry = input.SessionCountry,
                SessionReview = input.SessionReview
            };
            fs.Add(sesrev);

            return sesrev != null
                ? (ActionResult)new OkObjectResult(sesrev)
                : new BadRequestObjectResult("Please pass a valid Session JSON Payload in the request body");
        }

        [FunctionName("GetSessions")]
        public static IActionResult GetSessions(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "sesrev")]HttpRequest req, ILogger log)
        {
            log.LogInformation("Getting all of Fabian Sessions");
            return new OkObjectResult(fs);
        }

        [FunctionName("GetSessionById")]
        public static IActionResult GetTodoById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "sesrev/{id}")]HttpRequest req, 
            ILogger log, string id)
        {
            var sesrev = fs.FirstOrDefault(s => s.Id == id);
            if (sesrev == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(sesrev);
        }

        [FunctionName("UpdateSession")]
        public static async Task<IActionResult> UpdateSession(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "sesrev/{id}")]HttpRequest req,
            ILogger log, string id)
        {
            var sesrev = fs.FirstOrDefault(s => s.Id == id);
            if (sesrev == null)
            {
                return new NotFoundResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic updated = JsonConvert.DeserializeObject<FabsSession>(requestBody);

            {
                sesrev.SessionNumber = updated.SessionNumber;
                sesrev.SessionName = updated.SessionName;
                sesrev.SessionDate = updated.SessionDate;
                sesrev.SessionCity = updated.SessionCity;
                sesrev.SessionRegionState = updated.SessionRegionState;
                sesrev.SessionCountry = updated.SessionCountry;
                sesrev.SessionReview = updated.SessionReview;
            }
            return new OkObjectResult(sesrev);
        }
        [FunctionName("CreateReview")]
        public static async Task<IActionResult> CreateReview(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "createreview/{id}")]HttpRequest req,
            ILogger log, string id)
        {
            var sesrev = fs.FirstOrDefault(s => s.Id == id);
            if (sesrev == null)
            {
                return new NotFoundResult();
            }

            SessionReview[] mysr = sesrev.SessionReview;
            int length = mysr.Length;
            int newIndex;

            if (length == 0 || mysr == null)
            {
                newIndex = 0;
            }
            else
            {
                length = mysr.Length;
                newIndex = length - 1;
                //newIndex = length + 1;
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<SessionReview>(requestBody);

            {
                sesrev.SessionReview[newIndex].Id = updated.Id;
                sesrev.SessionReview[newIndex].Name = updated.Name;
                sesrev.SessionReview[newIndex].ZerotofiveRating = updated.ZerotofiveRating;
                sesrev.SessionReview[newIndex].Feedback = updated.Feedback;
                sesrev.SessionReview[newIndex].RequestContact = updated.RequestContact;
                sesrev.SessionReview[newIndex].Email = updated.Email;
            }

            var blankReview = new SessionReview()
            {
                Name = "",
                ZerotofiveRating = 0,
                Feedback = "",
                RequestContact = false,
                Email = ""
            };
            sr.Add(blankReview);

            return new OkObjectResult(sesrev);
        }


        [FunctionName("DeleteSession")]
        public static IActionResult DeleteSession(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "sesrev/{id}")]HttpRequest req,
            ILogger log, string id)
        {
            var todo = fs.FirstOrDefault(s => s.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }
            fs.Remove(todo);
            return new OkResult();
        }

    }
}
