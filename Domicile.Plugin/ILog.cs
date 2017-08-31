using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domicile.Common
{
    public interface ILog
    {
        bool IsDebugEnabled { get; }
        bool IsVerboseEnabled { get; }
        bool IsInformationalEnabled { get; }
        bool IsWarningEnabled { get; }
        bool IsFatalEnabled { get; }
        bool IsErrorEnabled { get; }

        string DateTimeFormat { get; set; }

        void Write(string message, EventSeverity severity);
        void Write(string message, Exception exception, EventSeverity severity);
    }
}
