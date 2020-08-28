using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Endgame.Backend
{
    public class Socket
    {
        private IAsyncCollector<SignalRMessage> messages;

        public Socket(IAsyncCollector<SignalRMessage> signalRMessages)
        {
            this.messages = signalRMessages;

        }


        public async Task SendMessageToUser(string target, object payload, params string[] userIds)
        {
            var arguments = JsonConvert.SerializeObject(payload);

            foreach (var userId in userIds)
            {
                await messages.AddAsync(
                  new SignalRMessage
                  {
                      UserId = userId,
                      Target = target,
                      Arguments = new[] { arguments }
                  }
                );
            }
        }
    }
}
