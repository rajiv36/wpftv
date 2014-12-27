using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using NDTV.Entities;
using NDTV.Interfaces;

namespace NDTV.Utilities
{
    /// <summary>
    /// Logger class
    /// </summary>
    public class Logger : ILogger
    {
        private string logFolderPath;        
        private SortedList<DateTime, string> logFileList;
        private int maxLogCount;
        private DateTime logDateTime;
        private string currentDateTimeString;        

        /// <summary>
        /// Default constructor
        /// </summary>
        public Logger()
        {   
            int output = -1;
            maxLogCount = int.TryParse(Utility.GetLogEntries(Constants.LoggingConstants.MaximumLogCount), out output) ? output : -1;
            logFileList = new SortedList<DateTime, string>();
            logFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Utility.ApplicationFolderPath, Utility.GetLogEntries(Constants.LoggingConstants.LogFolderName));
            if (false == Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);                
            }
            else
            {
                string[] fileNames = Directory.GetFiles(logFolderPath, Constants.LoggingConstants.LogFileSearchString);                
                foreach (string file in fileNames)
                {
                    AddToCollection(file);
                }
            }
        }

        /// <summary>
        /// This method adds the log file name to the collection
        /// </summary>
        /// <param name="fileName">File name</param>
        private void AddToCollection(string fileName)
        {
            string fullFileName = fileName.Replace(logFolderPath + "\\", "");
            fileName = fullFileName.Replace(Constants.LoggingConstants.LogFileExtension, "");
            string[] splitString = fileName.Split(Constants.LoggingConstants.Underscore);
            if (splitString.Length >= 2)
            {
                DateTime fileDateTime;
                bool isValidDate = DateTime.TryParse(splitString[1],out fileDateTime);
                if (isValidDate)
                {
                    fileDateTime = new DateTime(fileDateTime.Year, fileDateTime.Month, fileDateTime.Day, 
                        int.Parse(splitString[2], CultureInfo.InvariantCulture), 
                        int.Parse(splitString[3], CultureInfo.InvariantCulture), 
                        int.Parse(splitString[4], CultureInfo.InvariantCulture));
                    while (logFileList.ContainsKey(fileDateTime))
                    {
                        fileDateTime = fileDateTime.AddMilliseconds(1);
                    }
                    logFileList.Add(fileDateTime, fullFileName);
                }
            }
        }

        /// <summary>
        /// Logs the given message
        /// </summary>
        /// <param name="message">Message</param>
        public void Log(string message)
        {
            lock (this)
            {
                logDateTime = DateTime.Now;
                currentDateTimeString = logDateTime.ToString(Constants.LoggingConstants.DateTimeFormatString, CultureInfo.InvariantCulture);
                StringBuilder logMessage = new StringBuilder();

                logMessage.Append("Log date : " + logDateTime.ToString(Constants.LoggingConstants.DateTimeLogFormatString, CultureInfo.InvariantCulture));
                logMessage.AppendLine("-----------------------------------------------------------");
                logMessage.AppendLine(message);
                logMessage.AppendLine("-----------------------------------------------------------");

                LogData(BuildLogFilePath(), logMessage.ToString(), logDateTime, BuildLogFileName());
            }
        }        

        /// <summary>
        /// Logs the given exception
        /// </summary>
        /// <param name="exception">Exception</param>
        public void Log(Exception exception)
        {
            lock (this)
            {
                logDateTime = DateTime.Now;
                currentDateTimeString = logDateTime.ToString(Constants.LoggingConstants.DateTimeFormatString, CultureInfo.InvariantCulture);
                if (null != exception)
                {
                    StringBuilder logMessage = new StringBuilder();

                    logMessage.Append("Log date : " + logDateTime.ToString(Constants.LoggingConstants.DateTimeLogFormatString, CultureInfo.InvariantCulture));
                    logMessage.AppendLine("-----------------------------------------------------------");
                    logMessage.Append("Exception : ");
                    logMessage.AppendLine(exception.Message);
                    if (null != exception.InnerException)
                    {
                        logMessage.Append("Inner Exception : ");
                        logMessage.AppendLine(exception.InnerException.Message);
                    }
                    logMessage.Append("Stacktrace : ");
                    logMessage.AppendLine(exception.StackTrace);
                    logMessage.AppendLine("-----------------------------------------------------------");

                    LogData(BuildLogFilePath(), logMessage.ToString(), logDateTime, BuildLogFileName());
                }
            }
        }

        /// <summary>
        /// This builds the log file path
        /// </summary>
        /// <returns>Log file path</returns>
        private string BuildLogFilePath()
        {
            string logFilePath = Path.Combine(logFolderPath ,BuildLogFileName());
            return logFilePath;
        }

        /// <summary>
        /// This builds the log file name
        /// </summary>
        /// <returns>Log file name</returns>
        private string BuildLogFileName()
        {
            string logFileName = string.Format(CultureInfo.InvariantCulture, Constants.LoggingConstants.LogFileNameFormat, new object[]{ currentDateTimeString });
            return logFileName;
        }

        /// <summary>
        /// This method logs the information into the log file
        /// </summary>
        /// <param name="logFilePath">Log file path</param>
        /// <param name="logInformation">Information to be logged into</param>
        /// <param name="currentDateTime">Current date time in string</param>
        /// <param name="logFileName"> log file name</param>
        private void LogData(string logFilePath, string logInformation, DateTime currentDateTime, string logFileName)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine(logInformation);
                    writer.Flush();
                    writer.Close();
                }
                UpdateLogFiles(logFileName, currentDateTime);
            }
            catch(IOException)
            {
                // Ignore.
            }          
        }

        /// <summary>
        /// Updates the log files in the Log folder
        /// <param name="logFileName">Log file name</param>
        /// <param name="currentDateTime">Current date time</param>
        /// </summary>
        private void UpdateLogFiles(string logFileName, DateTime currentDateTime)
        {
            while (logFileList.ContainsKey(currentDateTime))
            {
                currentDateTime = currentDateTime.AddMilliseconds(1);
            }
            logFileList.Add(currentDateTime, logFileName);
            while (logFileList.Count > maxLogCount)
            {
                File.Delete(Path.Combine(logFolderPath,logFileList.Values[0]));
                logFileList.RemoveAt(0);                
            }
        }        
    }
}
