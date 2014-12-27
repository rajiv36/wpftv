using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using NDTV.Entities;

namespace NDTV.Utilities
{
    public static class Utility
    {
        /// <summary>
        /// Error code collection
        /// </summary>
        private static Collection<string> errorCodes;

        /// <summary>
        /// Returns the URL, given the link
        /// </summary>
        /// <param name="linkName">Url name</param>
        /// <returns>URL</returns>
        public static string GetLink(string linkName)
        {
            string link = string.Empty;
            link = ConfigUtility.GetConfigValues("UrlList", linkName);
            return link;
        }

        /// <summary>
        /// Returns the URL, given the link
        /// </summary>
        /// <param name="linkName">Url name</param>
        /// <returns>URL</returns>
        public static string GetFeedback(string linkName)
        {
            string link = string.Empty;
            link = ConfigUtility.GetConfigValues("FeedbackMessage", linkName);
            return link;
        }

        /// <summary>
        /// Returns the social data app settings value
        /// based on the key.
        /// </summary>
        /// <param name="key">Settings key</param>
        /// <returns>string</returns>
        public static string GetSocialData(string key)
        {
            string socialData = string.Empty;
            socialData = ConfigUtility.GetConfigValues("SocialData", key);
            return socialData;
        }

        /// <summary>
        /// Gets the application folder path from app.config
        /// </summary>
        public static string ApplicationFolderPath
        {
            get
            {
                return ConfigUtility.GetAppSettingsValue("ApplicationFolderName");
            }
        }

        /// <summary>
        /// Gets the application settings file name
        /// </summary>
        public static string ApplicationSettingsFileName
        {
            get
            {
                return ConfigUtility.GetAppSettingsValue("ApplicationSettingsFileName");
            }
        }

        /// <summary>
        /// Gets log entries from the configurations for the given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public static string GetLogEntries(string key)
        {
            string configValue = string.Empty;
            configValue = ConfigUtility.GetConfigValues("LoggingEntries", key);
            return configValue;
        }

        /// <summary>
        /// Gets Report error entries from the configuration for the given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public static string GetReportErrorEntries(string key)
        {
            string configValue = string.Empty;
            configValue = ConfigUtility.GetConfigValues("ReportError", key);
            return configValue;
        }

        /// <summary>
        /// Gets the credentials associated with the Stock Index
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public static string GetStockIndexCredentials(string key)
        {
            string configValue = string.Empty;
            configValue = ConfigUtility.GetConfigValues("StockIndexCredentials", key);
            return configValue;
        }
        
        /// <summary>
        /// Gets the Team Name associated with the key id.
        /// </summary>
        /// <param name="key">Team Id.</param>
        /// <returns>Associated Short form of the Team.</returns>
        public static string GetTeamName(string key)
        {
            string teamName = string.Empty;
            teamName = ConfigUtility.GetConfigValues("TeamIdList", key);
            return teamName;
        }

        /// <summary>
        /// Deserialize the json string into corresponding class object.
        /// </summary>
        /// <typeparam name="T">Type "T" of the class object used to represent the deserialized string.</typeparam>
        /// <param name="jsonData">The json string to be deserialized.</param>
        /// <returns>The corresponding type "T" object.</returns>
        public static T Deserialize<T>(string jsonData)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonData)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }

        /// <summary>
        /// Deserialize the json string into corresponding class object.
        /// </summary>
        /// <typeparam name="T">Type "T" of the class object used to represent the deserialized string.</typeparam>
        /// <param name="jsonString">The json string to be deserialized.</param>
        /// <returns>The corresponding type "T" object.</returns>
        public static T Deserialize<T>(Stream stream)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(stream);
        }

        /// <summary>
        /// Get the path for static image
        /// </summary>
        /// <returns>path of the image</returns>
        public static string FetchImagePath()
        {
            return (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Constant.Resources, Constants.Constant.Images));
        }

        /// <summary>
        /// Gets the relative path for this default NDTV image
        /// </summary>
        /// <param name="sourcePage">The page requesting the image</param>
        /// <returns>path of the default image</returns>
        public static string FetchDefaultNdtvImageSource(string sourcePage)
        {
            if (sourcePage.Equals(Constants.Constant.DefaultImage))
            {
                return (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Constant.Resources, Constants.Constant.Images, Constants.Constant.DefaultImage +Constants.Constant.JpgExtension ));
            }
            else if (sourcePage.Equals(Constants.Constant.DefaultImageForRelatedNews))
            {
                return (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Constant.Resources, Constants.Constant.Images, Constants.Constant.DefaultImageForRelatedNews + Constants.Constant.JpgExtension));
            }
            return (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Constant.Resources, Constants.Constant.Images, Constants.Constant.DefaultImage +Constants.Constant.JpgExtension ));
        }

        /// <summary>
        /// Read the Config Value
        /// </summary>
        /// <returns>path of the image</returns>
        public static string GetTimerInterval(string key)
        {
            return ConfigUtility.GetConfigValues("RefreshInterval", key);
        }

        /// <summary>
        /// converts XAML to HTML
        /// </summary>
        /// <param name="html">html</param>
        /// <returns>xaml section understandable to RichTextBox</returns>
        public static string ConvertHtmlToXaml(string html)
        {
            return Helper.HtmlToXamlConverter(html);
        }

        /// <summary>
        /// Retrieve the Number of items to be retrieved..
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>a number which specifies the number of items to be returned</returns>
        public static int GetNumberOfItemsToRetrieve(string key)
        {
            return Convert.ToInt32(ConfigUtility.GetConfigValues("ItemsRetrieved", key),CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// The serializes the object into xml
        /// </summary>
        /// <typeparam name="T">Type of object which is to be serialized to xml</typeparam>
        /// <param name="inputObject">Input object</param>
        /// <param name="filePath">File to which serialization is to be done</param>
        public static void SerializeToXml<T>(T inputObject, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter textWriter = new StreamWriter(filePath))
            {
                serializer.Serialize(textWriter, inputObject);
                textWriter.Close();
            }
        }

        /// <summary>
        /// This deserializes the xml into the object
        /// </summary>
        /// <typeparam name="T">Type of object to which the xml is to be deserialized to</typeparam>
        /// <param name="filePath">File path of the xml</param>
        /// <returns>The instance of object</returns>
        public static T DeserializeFromXml<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader textReader = new StreamReader(filePath))
            {
                var resultObject = (T)serializer.Deserialize(textReader);
                textReader.Close();
                return resultObject;
            }
        }


        /// <summary>
        /// This method loads the settings from the application settings file
        /// </summary>
        /// <returns>Application settings object</returns>
        public static ApplicationSettings LoadSettings()
        {
            ApplicationSettings applicationSettings = new ApplicationSettings();
            try
            {
                string settingsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationFolderPath);
                if (Directory.Exists(settingsFolderPath))
                {
                    string settingsFilePath = Path.Combine(settingsFolderPath, ApplicationSettingsFileName);
                    if (File.Exists(settingsFilePath))
                    {
                        string fileString = File.ReadAllText(settingsFilePath);
                        XElement settingsFileXml;
                        if (XElementTryParse(fileString,out settingsFileXml))
                        {
                            applicationSettings = DeserializeFromXml<ApplicationSettings>(settingsFilePath);
                        }
                    }
                }
            }
            catch (IOException)
            {
                //Ignore exception
            }
            return applicationSettings;
        }

        /// <summary>
        /// This method saves the settings into application settings file
        /// </summary>
        /// <param name="applicationSettings">Application settings instance</param>
        public static void SaveSettings(ApplicationSettings applicationSettings)
        {
            try
            {
                string settingsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationFolderPath);
                if (false == Directory.Exists(settingsFolderPath))
                {
                    Directory.CreateDirectory(settingsFolderPath);
                }
                string settingsFilePath = Path.Combine(settingsFolderPath, ApplicationSettingsFileName);
                SerializeToXml<ApplicationSettings>(applicationSettings, settingsFilePath);
            }
            catch (IOException)
            {
                //Ignore exception
            }
        }

        /// <summary>
        /// Custom TryParse for the XElement to indicate if the file being parsed is a XML file or a JSON file
        /// </summary>
        /// <param name="response">Concerned Response</param>
        /// <param name="xmlOutput">The XML output if no exception</param>
        /// <returns>True if XML file else false</returns>
        public static bool XElementTryParse(string response, out XElement xmlOutput)
        {
            bool isParseSuccessful = true;
            XElement temp= null;
            try
            {
               temp = XElement.Parse(response);
            }
            catch (System.Xml.XmlException exception)
            {
                isParseSuccessful = false;
            }

            if (!isParseSuccessful)
            {
                temp = null;
            }
            xmlOutput = temp;
            return isParseSuccessful;
        }

        /// <summary>
        /// A property which returns the query string that should be appended to read any article related for slate device
        /// </summary>
        public static string QueryStringForSlateDevice
        {
            get
            {
                return ConfigUtility.GetAppSettingsValue("QueryStringForArticle");
            }
        }

        /// <summary>
        /// It is used to retrieve QueryParameters from the query string.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns>Dictionary of query parameters retireved from the input string in key value format.</returns>
        public static Dictionary<string, string> GetQueryParameters(string queryString)
        {
            Dictionary<string, string> result = null;

            if (!string.IsNullOrEmpty(queryString))
            {
                result = new Dictionary<string, string>();

                if (queryString.Contains("?"))
                {
                    queryString = queryString.Remove(0, queryString.IndexOf("?", 0, StringComparison.OrdinalIgnoreCase) + 1);
                }

                string[] queryParams = queryString.Split('&');
                foreach (string qryParam in queryParams)
                {
                    if (!string.IsNullOrEmpty(qryParam))
                    {
                        if (qryParam.IndexOf("=", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            string[] temp = qryParam.Split('=');
                            result.Add(temp[0], temp[1]);
                        }
                        else
                        {
                            result.Add(qryParam, string.Empty);
                        }
                    }
                }
            }

            return result;
        }

        
        /// <summary>
        /// Check wether error needs to be suppressed
        /// </summary>
        /// <param name="errorMessage">error message</param>
        /// <returns>bool - wether error needs to be suppressed</returns>
        public static bool SuppressError(string errorMessage)
        {
            errorCodes = (null == errorCodes) ? ConfigUtility.GetKeysFromSection("HttpErrorMessage") : errorCodes;
            return (errorCodes.Any(keys => errorMessage.Contains(keys))) ? true : false;
        }

        public static string RetrieveLinkForArticleType(ArticleType articleType)
        {
            string link = string.Empty;

            switch(articleType)
            {
                case ArticleType.MostCommented:
                    link = ConfigUtility.GetConfigValues("ArticleType", "MostCommented");
                    break;
                case ArticleType.MostRead:
                    link = ConfigUtility.GetConfigValues("ArticleType", "MostRead");
                    break;
                default:
                    link = ConfigUtility.GetConfigValues("ArticleType", "MostRead");
                    break;
            }

            return link;
        }

        /// <summary>
        /// Truncate string with respect to the parameter 'limit'.
        /// </summary>
        /// <param name="title"> Title </param>
        /// <param name="limit"> Number of characters</param>
        /// <returns>Truncated string</returns>
        public static string TruncateString(string title, int limit)
        {
            const string tailingString = "...";
            string trucatedTitle = string.Empty;

            if (false == string.IsNullOrEmpty(title) && limit > -1 && title.Length > limit)
            {
                trucatedTitle = string.Concat(title.Substring(0, limit - 1), tailingString);
            }
            else
            {
                trucatedTitle = title;
            }
            return trucatedTitle;
        }
    }
}
