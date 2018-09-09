using Cake.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using System;

namespace TIKSN.Cake.Addin
{
    public class LoggerAdapter : ILogger
    {
        private readonly ICakeLog _cakeLog;

        public LoggerAdapter(ICakeLog cakeLog)
        {
            _cakeLog = cakeLog ?? throw new ArgumentNullException(nameof(cakeLog));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new EmptyDisposable();
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return _cakeLog.Verbosity >= ConvertToCakeLogVerbosity(logLevel);
        }

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _cakeLog.Write(ConvertToCakeLogVerbosity(logLevel), ConvertToCakeLogLevel(logLevel), formatter(state, exception));
        }

        private global::Cake.Core.Diagnostics.LogLevel ConvertToCakeLogLevel(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                    return global::Cake.Core.Diagnostics.LogLevel.Verbose;

                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    return global::Cake.Core.Diagnostics.LogLevel.Debug;

                case Microsoft.Extensions.Logging.LogLevel.Information:
                    return global::Cake.Core.Diagnostics.LogLevel.Information;

                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    return global::Cake.Core.Diagnostics.LogLevel.Warning;

                case Microsoft.Extensions.Logging.LogLevel.Error:
                    return global::Cake.Core.Diagnostics.LogLevel.Error;

                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    return global::Cake.Core.Diagnostics.LogLevel.Fatal;

                case Microsoft.Extensions.Logging.LogLevel.None:
                    return global::Cake.Core.Diagnostics.LogLevel.Fatal;

                default:
                    return global::Cake.Core.Diagnostics.LogLevel.Fatal;
            }
        }

        private Verbosity ConvertToCakeLogVerbosity(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                    return Verbosity.Diagnostic;

                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    return Verbosity.Verbose;

                case Microsoft.Extensions.Logging.LogLevel.Information:
                    return Verbosity.Normal;

                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    return Verbosity.Normal;

                case Microsoft.Extensions.Logging.LogLevel.Error:
                    return Verbosity.Minimal;

                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    return Verbosity.Minimal;

                case Microsoft.Extensions.Logging.LogLevel.None:
                    return Verbosity.Quiet;

                default:
                    return Verbosity.Quiet;
            }
        }

        public class EmptyDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}