using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domicile.Common;

namespace Domicile.Common.Extentions
{
    public static class LogExtensions
    {
        public static void Informational(this ILog logger, string message)
        {
            if (logger.IsInformationalEnabled)
                logger.Write(message, EventSeverity.Informational);
        }

        public static void Informational(this ILog logger, string format, params object[] args)
        {
            if (logger.IsInformationalEnabled)
                logger.Write(string.Format(format, args), EventSeverity.Informational);
        }

        public static void Warning(this ILog logger, string message)
        {
            if (logger.IsWarningEnabled)
                logger.Write(message, EventSeverity.Warning);
        }

        public static void Error(this ILog logger, string message)
        {
            if (logger.IsErrorEnabled)
                logger.Write(message, EventSeverity.Error);
        }

        public static void Fatal(this ILog logger, string message)
        {
            if (logger.IsFatalEnabled)
                logger.Write(message, EventSeverity.Fatal);
        }

        public static void Verbose(this ILog logger, string message)
        {
            if (logger.IsVerboseEnabled)
                logger.Write(message, EventSeverity.Verbose);
        }
    }
}
