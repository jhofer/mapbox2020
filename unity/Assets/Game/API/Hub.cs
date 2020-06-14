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
using System.Collections.Generic;
using System.Linq;

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
   private Queue<Tuple<string, Type, Action<string>>> listenersToAdd = new Queue<Tuple<string, Type,Action<string>>>();

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

        this.listenersToAdd.Enqueue(new Tuple<string, Type, Action<string>>(type, typeof(T), (string payload) => {
            Debug.Log($"Socket Message {payload}");
            var obj = JsonConvert.DeserializeObject<T>(payload);
            callback(obj); 
        
        }));


  
   }
   


   async void OnDestroy()
   {
      if (connection != null)
      {
            Debug.Log(" connection.StopAsync()");
           await connection.StopAsync();
            Debug.Log(" connection.StopAsync() done");
        }
      if (client != null)
      {
         client.Dispose();
      }
   }

   void Update()
   {
        if(connection != null && listenersToAdd.Any())
        {
            var el = listenersToAdd.Dequeue();
            var eventName = el.Item1;
            var modelType = el.Item2;
            var callBack = el.Item3;
            Debug.Log($"Add Listener {eventName} {modelType} {callBack.ToString()}");
            connection.On<string>(eventName, callBack);
        }
       
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
