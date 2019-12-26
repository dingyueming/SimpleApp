using System;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;
using LogLevel = NLog.LogLevel;

namespace Simple.Common
{
    public static class LogHelper
    {
        private static Logger _logger;

        public static Logger Logger
        {
            get
            {
                if (!LogOn) return null;

                if (_logger == null)
                {
                    LogManager.Configuration = LogConfig;
                    _logger = LogManager.GetCurrentClassLogger();
                }
                return _logger;
            }
        }

        private static LoggingConfiguration _logConfig;
        private static LoggingConfiguration LogConfig
        {
            get
            {
                if (_logConfig == null)
                {
                    LoggingConfiguration config = new LoggingConfiguration();
                    FileTarget fileTarget = new FileTarget()
                    {
                        FileName = "${basedir}/Logs/${shortdate}.log",
                        Encoding = Encoding.UTF8,
                        Layout = "${longdate}|${level:uppercase=true}|${message}${onexception:inner=${newline} ${exception:format=toString,Data}${newline} ${stacktrace}${newline}"
                    };
                    config.AddTarget("file", fileTarget);
                    LoggingRule ruleFile = new LoggingRule("*", LogLevel, fileTarget);
                    config.LoggingRules.Add(ruleFile);
                    _logConfig = config;
                }
                return _logConfig;
            }
        }

        public static bool LogOn { get; set; }
        public static LogLevel LogLevel { get; set; }

        /// <summary>
        /// 将Nlog日志添加到LogFactory中
        /// </summary>
        /// <param name="logFactory"></param>
        public static ILoggerFactory AddLogHelper(this ILoggerFactory logFactory, bool logOn = false, string logLevel = "warn")
        {
            if (logFactory == null)
                throw new ArgumentNullException(nameof(logFactory));
            LogOn = logOn;
            switch (logLevel?.ToLower())
            {
                case "debug":
                    LogLevel = LogLevel.Debug;
                    break;
                case "error":
                    LogLevel = LogLevel.Error;
                    break;
                case "fatal":
                    LogLevel = LogLevel.Fatal;
                    break;
                case "info":
                    LogLevel = LogLevel.Info;
                    break;
                case "off":
                    LogLevel = LogLevel.Off;
                    break;
                case "trace":
                    LogLevel = LogLevel.Trace;
                    break;
                case "warn":
                    LogLevel = LogLevel.Warn;
                    break;
                default:
                    LogLevel = LogLevel.Warn;
                    break;
            }
            LogFactory factory = new LogFactory(LogConfig);
            return logFactory.AddNLog(factory);
        }


        public static void Debug(string message)
        {
            Logger?.Debug(message);
        }

        public static void Debug(string message, Exception exception)
        {
            Logger?.Debug(exception, message);
        }

        public static void Error(string message)
        {
            Logger?.Error(message);
        }

        public static void Error(string message, Exception exception)
        {
            Logger?.Error(exception, message);
        }

        public static void Error(Exception exception, string message){
            Logger?.Error(exception, message);
        }

        public static void Fatal(string message)
        {
            Logger?.Fatal(message);
        }

        public static void Fatal(string message, Exception exception)
        {
            Logger?.Fatal(exception, message);
        }

        public static void Info(string message)
        {
            Logger?.Info(message);
        }

        public static void Info(string message, Exception exception)
        {
            Logger?.Info(exception, message);
        }

        public static void Warn(string message)
        {
            Logger?.Warn(message);
        }

        public static void Warn(string message, Exception exception)
        {
            Logger?.Warn(exception, message);
        }
    }
}
