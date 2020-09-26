using System;
using Microsoft.Extensions.Logging;
using UnityEngine;

public class UnityLoggerProvider : ILoggerProvider
{
    public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
    {
        return new UnityLogger();
    }

    public void Dispose()
    {
       //nothing todo
    }

    public class UnityLogger : Microsoft.Extensions.Logging.ILogger
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
}