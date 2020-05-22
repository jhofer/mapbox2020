using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using System;
using Microsoft.Extensions.Logging;
using UnityEngine.UI;
using System.Net.Http;
using Microsoft.AspNetCore.Http.Connections.Client;
using System.Net.Http.Headers;


public class Hub : MonoBehaviour
{
    private HttpClient client;
    private HubConnection connection;

    public HttpConnectionFactory factory;
    public Text Text;
    private float nextTime =0;
    private float interval = 3;
    private bool connected;
    public Auth auth;

    

    // Start is called before the first frame update
    async void Start()
    {
        try
        {
            Debug.Log("Start HubConnection");
            client = new HttpClient();
           
            connection = new HubConnectionBuilder()
                .WithUrl("https://func-endgame-f2-dev.azurewebsites.net/api")
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Debug);
                    logging.AddProvider(new UnityLoggerProvider());
                })
                .Build();


            connection.On<string>("newMessage", Echo);
           


            Text.text += "Connecting\n";
            await connection.StartAsync();
            this.connected = true;
            Text.text += "Connected\n";
        }
        catch (Exception ex)
        {
            Text.text += ex.ToString();
            Debug.LogException(ex);
        }



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
        if (Time.time >= nextTime)
        {
            nextTime += interval;
           
                try
                {
                    Debug.Log("create httpclient");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.GetToken());


                    client.BaseAddress = new Uri("https://func-endgame-f2-dev.azurewebsites.net");
                    Debug.Log("pre client.PostAsyn");
                    var result = await client.PostAsync("/api/SendMessage", new StringContent("hallo unity"));
                    string resultContent = await result.Content.ReadAsStringAsync();
                    Debug.Log("post client.PostAsyn" + resultContent);
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                }
        }
    }

    // Update is called once per frame
    public void Echo(string message)
    {
        Text.text += $"{message} {DateTime.Now}\n";
        Debug.Log(message);
    }
}
