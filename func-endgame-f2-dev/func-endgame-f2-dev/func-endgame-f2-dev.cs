using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

using System.Web.Http;
using System.Net;
using Google.Apis.Auth;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace func_endgame_f2_dev
{
    public static class Functions
    {
        private const string AUTH_HEADER_NAME = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";

        [FunctionName("negotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "chat")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }


        [FunctionName("SendMessage")]
        public static async Task SendMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalR(HubName = "chat")] IAsyncCollector<SignalRMessage> signalRMessages,
             ILogger log)
        {
            log.LogDebug("Enter function");
            try
            {
             
                Payload token;
                // Get the token from the header
                if (req.Headers.ContainsKey(AUTH_HEADER_NAME) &&
                   req.Headers[AUTH_HEADER_NAME].ToString().StartsWith(BEARER_PREFIX))
                {
                    var authorizationHeader = req.Headers["Authorization"].ToString();
                    log.LogInformation(authorizationHeader);
                    var jwt = req.Headers["Authorization"].ToString().Substring(BEARER_PREFIX.Length);
                    token = await GoogleJsonWebSignature.ValidateAsync(jwt);
                    log.LogInformation(JsonConvert.SerializeObject(token));

                }
                else
                {
                    log.LogInformation("no token added");
                    throw new Exception("no token");
                }
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation(" signalRMessages.AddAsync");
                await signalRMessages.AddAsync(
                    new SignalRMessage
                    {
                        Target = "newMessage",
                        Arguments = new[] { token.Email + ": " + content }
                    });
            }catch(Exception e)
            {
                log.LogError(e.Message);
                    throw e;
            }

        }
    }
}
