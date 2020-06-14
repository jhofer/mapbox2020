using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using System;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.AspNetCore.Http.Connections.Client;
using System.Net.Http.Headers;
using Mapbox.Json;
using Assets.Game.Domain;
using System.Threading.Tasks;
using Endgame.Domain;

public class Hub : BaseSingleton<Hub>
{
   [SerializeField]
   public string BaseAdressAzure = "https://func-endgame-f2-dev.azurewebsites.net";


   [SerializeField]
   public string BaseAdressEditor = "http://localhost:7071";
   private static object m_Lock = new object();
   private string baseAdress;
   private HttpClient client = new HttpClient();
   private HubConnection connection;

   public HttpConnectionFactory factory;
   private float nextTime = 0;
   private float interval = 3;
   private bool connected;
   public Auth auth;


    public string UserId { get; private set; }

    public async void ConquerBuilding(BuildingDto building)
   {
        await SetHeader();
        var message = JsonConvert.SerializeObject(building);
        var result = await client.PostAsync("/api/buildings", new StringContent(message));
        string resultContent = await result.Content.ReadAsStringAsync();
        Debug.Log("Send Result" + resultContent);
    }


   // Start is called before the first frame update
   async void Start()
   {
      baseAdress = Application.isEditor ? BaseAdressEditor : BaseAdressAzure;

      client.BaseAddress = new Uri(baseAdress);

      Debug.Log("Create HubConnection");
      Debug.Log("1. Get Token");
      var token = await auth.GetToken();
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      Debug.Log("2. Read Profile");
      var response = await client.GetAsync("/api/users/me");
      string content = await response.Content.ReadAsStringAsync();

      if (response.IsSuccessStatusCode)
      {
         var user= JsonConvert.DeserializeObject<UserDto>(content);
         this.UserId = user.id;
         Debug.Log("loaded user profile: " + UserId);

         Debug.Log("3. Create Connection");
         connection = new HubConnectionBuilder()
          .WithUrl(baseAdress + "/api", options =>
          {
             options.AccessTokenProvider = () => auth.GetToken();
             options.Headers.Add("x-ms-client-principal-id", UserId);
          })
          .ConfigureLogging(logging =>
          {
             logging.ClearProviders();
             logging.SetMinimumLevel(LogLevel.Debug);
             logging.AddProvider(new UnityLoggerProvider());
          })
          .Build();

            Debug.Log("4. Start Connection");
            await connection.StartAsync();

        }
      else
      {
         Debug.LogWarning("response code from get user was " + response.StatusCode + "\n" + content);
      }




   }


   public void On<T>(string type, Action<T> callback)
   {
      connection.On<T>(type, callback);
   }



   private void OnDestroy()
   {
      if (connection != null)
      {
         connection.StopAsync().GetAwaiter().GetResult();

      }
      if (client != null)
      {
         client.Dispose();
      }
   }

   async void Update()
   {


   }



   public async void Send(object payload)
   {
      await SetHeader();
      var message = JsonConvert.SerializeObject(payload);
      var result = await client.PostAsync("/api/messages", new StringContent(message));
      string resultContent = await result.Content.ReadAsStringAsync();
      Debug.Log("Send Result" + resultContent);
   }

   private async Task SetHeader()
   {
      var token = await auth.GetToken();
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
   }
}
