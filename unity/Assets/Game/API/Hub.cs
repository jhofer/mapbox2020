using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using System;
using Microsoft.Extensions.Logging;
using UnityEngine.UI;
using System.Net.Http;
using Microsoft.AspNetCore.Http.Connections.Client;
using System.Net.Http.Headers;
using System.Text.Json;
using Mapbox.Json;
using Mapbox.Json.Serialization;

public class Hub : BaseSingleton<Hub>
{ 
    [SerializeField]
    public string BaseAdressAzure = "https://func-endgame-f2-dev.azurewebsites.net";
    [SerializeField]
    public string BaseAdressEditor = "http://localhost:7071";

    private string baseAdress;
    private HttpClient client = new HttpClient();
    private HubConnection connection;

    public HttpConnectionFactory factory;
    private float nextTime =0;
    private float interval = 3;
    private bool connected;
    public Auth auth;



    // Start is called before the first frame update
    async void Start()
    {
        if (Application.isEditor)
        {
            baseAdress = (BaseAdressEditor);

        }
        else
        {
            baseAdress = (BaseAdressAzure);

        }

        client.BaseAddress = new Uri(baseAdress);


    }



    private void OnDestroy()
    {
        if (connection != null)
        {
            connection.StopAsync();

        }
        if (client != null)
        {
            client.Dispose();
        }
    }

    async void Update()
    {
        
        if(auth.IsLoggedIn && connection == null)
        {
            Debug.Log("Create HubConnection");
            Debug.Log("1. Get Token");
            var token = await auth.GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Debug.Log("2. Read Profile");
            var response = await client.GetAsync("/api/users/me");
            string content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                



                var user = JsonConvert.DeserializeObject<User>(content);
                var userId = user.id;
                Debug.Log("loaded user profile: " + userId);

                Debug.Log("3. Create Connection");
                connection = new HubConnectionBuilder()
                 .WithUrl(baseAdress+"/api", options =>
                 {
                     options.AccessTokenProvider = () => auth.GetToken();
                     options.Headers.Add("x-ms-client-principal-id", userId);
                 })
                 .ConfigureLogging(logging =>
                 {
                     logging.ClearProviders();
                     logging.SetMinimumLevel(LogLevel.Debug);
                     logging.AddProvider(new UnityLoggerProvider());
                 })
                 .Build();

                connection.On<string>("newMessage", Echo);



                Debug.Log("4. Start Connection");
                await connection.StartAsync();

            }
            else
            {
                Debug.LogWarning("response code from get user was " + response.StatusCode +"\n"+ content);
            }
        }


    }

    // Update is called once per frame
    public void Echo(string message)
    {
        Debug.Log(message);
    }

    public async void Send(string message)
    {   
        var token = await auth.GetToken();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var result = await client.PostAsync("/api/messages", new StringContent(message));
        string resultContent = await result.Content.ReadAsStringAsync();
        Debug.Log("Send Result" + resultContent);
    }
}
