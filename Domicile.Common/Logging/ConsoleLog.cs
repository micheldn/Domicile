using System;
using Domicile.Common;

namespace Domicile.Common.Logging
{
    public class ConsoleLog : ILog
    {
        public bool IsDebugEnabled { get; set; }
        public bool IsVerboseEnabled { get; set; }
        public bool IsInformationalEnabled { get; set; }
        public bool IsWarningEnabled { get; set; }
        public bool IsFatalEnabled { get; set; }
        public bool IsErrorEnabled { get; set; }

        public string DateTimeFormat { get; set; }

        public ConsoleLog()
        {
            IsDebugEnabled = true;
            IsVerboseEnabled = true;
            IsInformationalEnabled = true;
            IsWarningEnabled = true;
            IsFatalEnabled = true;
            IsErrorEnabled = true;
        }

        public void Write(string message, EventSeverity severity)
        {
            var dateTimeFormat = string.IsNullOrWhiteSpace(DateTimeFormat) ? $"yyyy-MM-dd HH:mm:ss" : DateTimeFormat;
            Console.WriteLine($"[{DateTime.Now.ToString(dateTimeFormat)}][{GetSeverityAsString(severity)}] {message}");
        }

        public void Write(string message, Exception exception, EventSeverity severity)
        {
            var dateTimeFormat = string.IsNullOrWhiteSpace(DateTimeFormat) ? $"yyyy-MM-dd HH:mm:ss" : DateTimeFormat;
            Write(message, severity);
            Console.WriteLine($"[{DateTime.Now.ToString(dateTimeFormat)}][{GetSeverityAsString(severity)}] {exception}");
        }

        private string GetSeverityAsString(EventSeverity eventSeverity)
        {
            switch (eventSeverity)
            {
                case EventSeverity.Informational:
                    return "INFO";
                case EventSeverity.Warning:
                    return "WARNING";
                case EventSeverity.Error:
                    return "ERROR";
                case EventSeverity.Fatal:
                    return "FATAL";
                case EventSeverity.Debug:
                    return "DEBUG";
                case EventSeverity.Verbose:
                    return "VERBOSE";
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventSeverity), eventSeverity, null);
            }
        }
    }
}