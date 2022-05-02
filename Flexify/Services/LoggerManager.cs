using NLog;

namespace Flexify.Services
{
    public class LoggerManager: ILoggerManager
    {
        private ILogger _logger;

        public LoggerManager()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warning(string message)
        {
            _logger.Warn(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }
    }
}