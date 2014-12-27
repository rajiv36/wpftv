using System;

namespace NDTV.Interfaces
{
    public interface ILogger
    {
        /// <summary>
        /// Logs the given message
        /// </summary>
        /// <param name="message">Message to be logged</param>
        void Log(string message);

        /// <summary>
        /// Logs the given exception
        /// </summary>
        /// <param name="exception">Exception to be logged</param>
        void Log(Exception exception);
    }
}
