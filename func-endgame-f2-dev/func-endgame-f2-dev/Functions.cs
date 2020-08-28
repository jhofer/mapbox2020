
using Endgame.Backend.Auth;
using Endgame.Backend.Domain;
using Endgame.Backend.DTO;
using Endgame.Domain;
using Endgame.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Endgame.Backend
{
    public class Functions : IFunctionExceptionFilter, IFunctionInvocationFilter
    {
        private readonly CosmosContext context;
        private readonly DBAccess dbA;
        private readonly ILogger logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private Payload token;

        public Functions(CosmosContext context, DBAccess dbAccess, ILogger logger, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.dbA = dbAccess;
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
        }

        [FunctionName("negotiate")]
        public async Task<object> GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "endgame", UserId = "{headers.x-ms-client-principal-id}")] SignalRConnectionInfo connectionInfo)

        {
            return connectionInfo;
        }

        [FunctionName("ConquerBuilding")]
        public async Task<IActionResult> ConquerBuilding(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "buildings")] HttpRequest req,
           [SignalR(HubName = "endgame")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var socket = new Socket(signalRMessages);
            var userId = dbA.GetUserByEmail(token.Email).Id;
            Building building = await GetPayload<Building, BuildingDto>(req);
            building.UserId = userId;
            var conquredBuilding = await dbA.Create(building);
            await socket.SendMessageToUser("ConquerBuilding", conquredBuilding, userId);

            return new OkResult();
        }

        private async Task<T> GetPayload<T, S>(HttpRequest req)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            this.logger.LogDebug("RequestData: " + content);
            var playloadDto = JsonConvert.DeserializeObject<S>(content);
            var payload = Mapper.Map<T, S>(playloadDto);
            return payload;
        }

        [FunctionName("SendMessage")]
        public async Task<IActionResult> SendMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "messages")] HttpRequest req,
            [SignalR(HubName = "endgame")] IAsyncCollector<SignalRMessage> signalRMessages
          )
        {
            var socket = new Socket(signalRMessages);
            var userId = dbA.GetUserByEmail(token.Email).Id;
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            await socket.SendMessageToUser("newMessage", content, userId);

            return new OkResult();
        }


        [FunctionName("GetLoggedInUser")]
        public async Task<IActionResult> GetLoggedInUser(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/me")] HttpRequest req, ILogger logger)
        {
            var user = dbA.GetUserByEmail(this.token.Email);
            // when entity framework is supported context.Users.FirstOrDefault(u => u.Email.Equals(token.Email));

            if (user != null)
            {
                return new OkObjectResult(user);
            }
            else
            {
                user = await dbA.Create(new User
                {
                    Email = this.token.Email,
                    Name = token.Name
                });

            }
            return new OkObjectResult(user);
        }

        public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
        {

            logger.LogDebug($"I should be executed at last");
            return Task.CompletedTask;
        }

        public Task OnExceptionAsync(FunctionExceptionContext exceptionContext, CancellationToken cancellationToken)
        {
            if (exceptionContext.Exception is UnauthorizedAccessException)
            {
                logger.LogError(exceptionContext.Exception, exceptionContext.Exception.Message);
                var r = new UnauthorizedResult();
                httpContextAccessor.HttpContext.Response.StatusCode = r.StatusCode;


            }
            return Task.CompletedTask;
        }

        public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            var request = executingContext.Arguments.First().Value as HttpRequest;
            try
            {
                Payload token = TokenValidator.GetAndValidateToken(request, logger).Result;
                this.token = token;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("could not validate token ", ex);
            }
            return Task.CompletedTask;

        }
    }
}
