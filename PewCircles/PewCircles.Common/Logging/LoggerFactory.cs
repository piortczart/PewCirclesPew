using System;
using System.Collections.Generic;

namespace HelloGame.Common.Logging
{
    public class LoggerFactory : ILoggerFactory
    {
        private readonly string _extraInfo;

        public LoggerFactory(string extraInfo)
        {
            _extraInfo = extraInfo;
        }

        public ILogger CreateLogger(Type sourceType)
        {
            return new Logger(sourceType, _extraInfo);
        }
    }
}