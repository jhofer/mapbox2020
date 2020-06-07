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
using MongoDB.Driver;
using System.Linq;

namespace func_endgame_f2_dev
{
    public static class Functions
    {
        private static readonly FeedOptions DefaultOptions = new FeedOptions { EnableCrossPartitionQuery = true };

        [FunctionName("negotiate")]
        public static async Task<object> GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "chat", UserId = "{headers.x-ms-client-principal-id}")] SignalRConnectionInfo connectionInfo, ILogger log)

        {
            log.LogInformation("Enter negotiate");
            try
            {
                Payload token =  TokenValidator.GetAndValidateToken(req, log).Result;
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


        [FunctionName("SendMessage")]
        public static async Task<IActionResult> SendMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "messages")] HttpRequest req,
            [SignalR(HubName = "chat")] IAsyncCollector<SignalRMessage> signalRMessages,
             ILogger log)
        {
            log.LogDebug("Enter SendMessage");
            Payload token = await TokenValidator.GetAndValidateToken(req, log);
            
            try
            {
               
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation(" signalRMessages.AddAsync");
                await signalRMessages.AddAsync(
                    new SignalRMessage
                    {
                        Target = "newMessage",
                        Arguments = new[] { token.Email + ": " + content }
                    });
                return new OkResult();
            }
            catch(UnauthorizedAccessException e) {
                return new UnauthorizedResult();
            }
        
            catch (Exception e)
            {
                log.LogError("SendMessage Failed "+e.Message);
                    throw e;
            }

        }

        [FunctionName("GetLoggedInUser")]
        public static async Task<IActionResult> GetLoggedInUser(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/me")] HttpRequest req, ILogger logger, [CosmosDB(
                databaseName: "endgame",
                collectionName: "users",
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client)
         
        {
            logger.LogDebug("Enter GetLoggedInUser");
           
            try
            {
                Payload token = await TokenValidator.GetAndValidateToken(req, logger);
                logger.LogDebug("email in token: " + token.Email);
                logger.LogDebug("name  in token: " + token.Name);
                
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri("endgame", "users");
                var user = client.CreateDocumentQuery<User>(collectionUri, DefaultOptions)
                       .Where(f => f.email == token.Email).AsEnumerable().FirstOrDefault();
               
                if (user != null) { 
                    logger.LogDebug("Return User from DB");
                    return new OkObjectResult(user);

                }
                else
                {
                    logger.LogDebug("create new User");
               

                    var newUser = new User
                    {
                        email = token.Email,
                        name = token.Name
                    };
                    await client.UpsertDocumentAsync(collectionUri, newUser);
                    return new OkObjectResult(newUser);

                }


            }
            catch (UnauthorizedAccessException e)
            {
                logger.LogError(e, e.Message);
                return new UnauthorizedResult();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw e;
            }

        }
 

    }
}
