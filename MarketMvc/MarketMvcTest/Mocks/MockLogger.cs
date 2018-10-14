using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using MarketMvc.Controllers;

namespace MarketMvcTest.Mocks
{
    class MockLogger : ILogger<HomeController>
    {
        private string _log;

        // Register calls to Log
        public MockLogger()
        {
            _log = string.Empty;
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //if (!IsEnabled(logLevel))
            //{
            //    return;
            //}

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            if (exception != null)
            {
                message += "\n" + exception.ToString();
            }

            _log = message;
        }

        //search if log entry matches something we logged
        public bool DidLog(string logEntry)
        {
            return (_log.Contains(logEntry));
        }
    }
}
