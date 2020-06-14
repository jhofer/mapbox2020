using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using func_endgame_f2_dev.Auth;
using Microsoft.Azure.Documents.Client;
using Assets.Game.Domain;
using Newtonsoft.Json;
using Endgame.Domain;

namespace func_endgame_f2_dev
{
   public static class Functions
   {

      [FunctionName("negotiate")]
      public static async Task<object> GetSignalRInfo(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
          [SignalRConnectionInfo(HubName = "endgame", UserId = "{headers.x-ms-client-principal-id}")] SignalRConnectionInfo connectionInfo, ILogger log)

      {

         try
         {
            Payload token = TokenValidator.GetAndValidateToken(req, log).Result;
            return connectionInfo;
         }
         catch (UnauthorizedAccessException e)
         {
            return new UnauthorizedResult();
         }
         catch (Exception e)
         {
            log.LogError(e, "Negotiate Faild");
            throw e;
         }

      }

      [FunctionName("ConquerBuilding")]
      public static async Task<IActionResult> ConquerBuilding(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "buildings")] HttpRequest req,
         [SignalR(HubName = "endgame")] IAsyncCollector<SignalRMessage> signalRMessages,
         [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client, ILogger log)
      {

         try
         {
            Payload token = await TokenValidator.GetAndValidateToken(req, log);

            var dbA = new DBAccess(client);
            var socket = new Socket(signalRMessages);

            var userId = dbA.GetUserByEmail(token.Email).id;
            var content = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogDebug("ConquerBuilding: "+content);
            var building = JsonConvert.DeserializeObject<BuildingDto>(content);
                building.userId = userId;
            var conquredBuilding = await dbA.Create(building);

            await socket.SendMessageToUser("ConquerBuilding", conquredBuilding, userId);

            return new OkResult();
         }
         catch (UnauthorizedAccessException e)
         {
            return new UnauthorizedResult();
         }

      }



      [FunctionName("SendMessage")]
      public static async Task<IActionResult> SendMessage(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "messages")] HttpRequest req,
          [SignalR(HubName = "endgame")] IAsyncCollector<SignalRMessage> signalRMessages,
          [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client, ILogger log)
      {

         try
         {
            Payload token = await TokenValidator.GetAndValidateToken(req, log);

            var dbA = new DBAccess(client);
            var socket = new Socket(signalRMessages);

            var userId = dbA.GetUserByEmail(token.Email).id;
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            await socket.SendMessageToUser("newMessage", content, userId);

            return new OkResult();
         }
         catch (UnauthorizedAccessException e)
         {
            return new UnauthorizedResult();
         }

      }


      [FunctionName("GetLoggedInUser")]
      public static async Task<IActionResult> GetLoggedInUser(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/me")] HttpRequest req, ILogger logger, [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client)
      {
         try
         {
            Payload token = await TokenValidator.GetAndValidateToken(req, logger);
            var dbA = new DBAccess(client);

            var user = dbA.GetUserByEmail(token.Email);
            if (user != null)
            {
               return new OkObjectResult(user);
            }
            else
            {

               user = await dbA.Create(new UserDto
               {
                  email = token.Email,
                  name = token.Name
               });
             
            }
                return new OkObjectResult(user);

            }
         catch (UnauthorizedAccessException e)
         {
            logger.LogError(e, e.Message);
            return new UnauthorizedResult();
         }

      }



   }
}
