using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domicile.Plugin;

namespace Domicile.Core.Logging
{
    class NullLog : ILog
    {
        public bool IsDebugEnabled { get; }
        public bool IsVerboseEnabled { get; }
        public bool IsInformationalEnabled { get; }
        public bool IsWarningEnabled { get; }
        public bool IsFatalEnabled { get; }
        public bool IsErrorEnabled { get; }
        public string DateTimeFormat { get; set; }
        public void Write(string message, EventSeverity severity)
        {
        }

        public void Write(string message, Exception exception, EventSeverity severity)
        {
        }
    }
}
