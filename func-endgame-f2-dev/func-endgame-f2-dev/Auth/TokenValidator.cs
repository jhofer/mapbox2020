using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Google.Apis.Auth;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace func_endgame_f2_dev.Auth
{
    public class TokenValidator
    {
        private const string AUTH_HEADER_NAME = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";

        public static async Task<Payload> GetAndValidateToken(HttpRequest req, ILogger log)
        {
            Payload token;
            // Get the token from the header
            if (req.Headers.ContainsKey(AUTH_HEADER_NAME) &&
               req.Headers[AUTH_HEADER_NAME].ToString().StartsWith(BEARER_PREFIX))
            {
                var authorizationHeader = req.Headers["Authorization"].ToString();
                log.LogInformation("TokenValidator Extracted Token: "+authorizationHeader);
                var jwt = authorizationHeader.Substring(BEARER_PREFIX.Length);

                try
                {
                    token = await GoogleJsonWebSignature.ValidateAsync(jwt);
                    log.LogInformation(JsonConvert.SerializeObject(token));
                }catch(Exception e)
                {
                    throw new UnauthorizedAccessException(e.Message);
                }

            }
            else
            {
                log.LogInformation("no token added");
                throw new UnauthorizedAccessException("no token provided");
            }

            return token;
        }
    }
}
