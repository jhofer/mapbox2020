using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Endgame.Backend.Auth
{
    public class TokenValidator
    {
        private const string AUTH_HEADER_NAME = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";

        public static async Task<Payload> GetAndValidateToken(HttpRequest req, ILogger log)
        {

            // Get the token from the header
            if (req.Headers.ContainsKey(AUTH_HEADER_NAME))
            {
                var tokenString = req.Headers[AUTH_HEADER_NAME].ToString();
                Payload token;
                if (tokenString.StartsWith(BEARER_PREFIX))
                {
                    var jwt = tokenString.Substring(BEARER_PREFIX.Length);
                    log.LogInformation("TokenValidator Extracted Token: " + jwt);
                    if (jwt.Contains("@gmail.com"))
                    {
                        // For Testing propose
                        token = new Payload()
                        {
                            Email = jwt,
                            Name = jwt.Substring(0, tokenString.IndexOf("@"))
                        };
                    }
                    else
                    {
                        try
                        {
                            token = await GoogleJsonWebSignature.ValidateAsync(jwt);

                        }
                        catch (Exception e)
                        {
                            throw new UnauthorizedAccessException(e.Message);
                        }
                    }


                }

                else
                {
                    throw new UnauthorizedAccessException(tokenString + "is no valid token");
                }

                log.LogInformation(JsonConvert.SerializeObject(token));
                return token;
            }
            else
            {
                log.LogInformation("no token added");
                throw new UnauthorizedAccessException("no token provided");
            }

        }
    }
}