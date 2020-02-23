using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using TMPro;
using System;
using Microsoft.Extensions.Logging;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Net.Http;

public class HubStuff : MonoBehaviour
{
    private HttpClient client;
    private HubConnection connection;

    public HttpConnectionFactory factory;
    public Text Text;
    private float nextTime =0;
    private float interval = 10;
    private bool connected;

    public class UnityLogger : ILoggerProvider
    {
        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return new UnityLog();
        }
        public class UnityLog : Microsoft.Extensions.Logging.ILogger
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                var id = Guid.NewGuid();
                Debug.Log($"BeginScope ({id}): {state}");
                return new Scope<TState>(state, id);
            }
            struct Scope<TState> : IDisposable
            {
                public Scope(TState state, Guid id)
                {
                    State = state;
                    Id = id;
                }

                public TState State { get; }
                public Guid Id { get; }

                public void Dispose() => Debug.Log($"EndScope ({Id}): {State}");
            }

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                switch (logLevel)
                {
                    case LogLevel.Trace:
                        Debug.Log($"{logLevel}, {eventId}, {state}, {exception}");
                        break;
                    case LogLevel.Debug:
                        Debug.Log($"{logLevel}, {eventId}, {state}, {exception}");
                        break;
                    case LogLevel.Information:
                        Debug.Log($"{logLevel}, {eventId}, {state}, {exception}");
                        break;
                    case LogLevel.Warning:
                        Debug.LogWarning($"{logLevel}, {eventId}, {state}, {exception}");
                        break;
                    case LogLevel.Error:
                        Debug.LogError($"{logLevel}, {eventId}, {state}, {exception}");
                        break;
                    case LogLevel.Critical:
                        Debug.LogError($"{logLevel}, {eventId}, {state}, {exception}");
                        break;
                    case LogLevel.None: break;
                }
            }
        }

        public void Dispose() { }
    }

    // Start is called before the first frame update
    async Task Start()
    {
        try
        {
            Text.text += "Starting\n";
            client = new HttpClient();
            connection = new HubConnectionBuilder()
                .WithUrl("https://func-endgame-f2-dev.azurewebsites.net/api")
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Debug);
                    logging.AddProvider(new UnityLogger());
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
        
        connection.StopAsync();
        client.Dispose();
    }

    async Task Update()
    {
        if (Time.time >= nextTime)
        {
            nextTime += interval;
            if (connected)
            {
                Debug.Log("create httpclient");
               
                client.BaseAddress = new Uri("https://func-endgame-f2-dev.azurewebsites.net");
                Debug.Log("pre client.PostAsyn");
                var result = await client.PostAsync("/api/SendMessage", new StringContent("hallo unity") );
                string resultContent = await result.Content.ReadAsStringAsync();
                Debug.Log("post client.PostAsyn"+ resultContent);
            }

           

        }
    }

    // Update is called once per frame
    public void Echo(string message)
    {
        Text.text += $"{message} {DateTime.Now}\n";
    }
}
