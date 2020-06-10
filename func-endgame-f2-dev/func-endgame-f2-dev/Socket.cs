using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace func_endgame_f2_dev
{
    public class Socket
    {
        private IAsyncCollector<SignalRMessage> messages;
        
        public Socket(IAsyncCollector<SignalRMessage> signalRMessages)
        {
            this.messages = signalRMessages;
          
        }


        public async Task SendMessageToUser(string target, object payload, string userId)
        {

            var arguments = JsonConvert.SerializeObject(payload);

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
