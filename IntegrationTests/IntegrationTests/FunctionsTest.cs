using Endgame.DTOs;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EndgameTest
{
    [TestFixture]
    public class FunctionsTest
    {
        private HttpClient client;

        [SetUp]
        public void Setup()
        {
            this.client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:7071")
              
             };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "rofl.orly@gmail.com");
        }

        [Test]
        public async Task GetUser() 
        {
            var response = await this.client.GetAsync("/api/users/me");
            string content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserDto>(content);
          
            Assert.AreEqual(user.email,"rofl.orly@gmail.com");
        }

    }
}