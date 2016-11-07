using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HelloGame.Common.Logging
{
    public class Logger : ILogger
    {
        private readonly Type _sourceType;
        private readonly string _extraInfo;

        public Logger(Type sourceType, string extraInfo)
        {
            _sourceType = sourceType;
            _extraInfo = extraInfo;
        }

        public void LogInfo(string text)
        {
            var logDetails = new LogDetails
            {
                Text = text,
                ExtraInfo = _extraInfo,
                Exception = null,
                Type = _sourceType,
                When = DateTime.Now
            };

            Debug.WriteLine(FormatLog(logDetails));
        }

        public void LogError(string text, Exception error = null)
        {
            var logDetails = new LogDetails
            {
                Text = text,
                ExtraInfo = _extraInfo,
                Exception = error,
                Type = _sourceType,
                When = DateTime.Now
            };

            Debug.WriteLine(FormatLog(logDetails));
        }

        public static string FormatLog(LogDetails details)
        {
            string exception = $"{Environment.NewLine}{details.Exception}";
            return $"{details.When:HH:mm:ss} [{details.ExtraInfo}] {details.Type.Name}: {details.Text}{exception}";
        }
    }
}