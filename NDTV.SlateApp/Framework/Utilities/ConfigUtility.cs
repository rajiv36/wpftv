using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Collections.ObjectModel;

namespace NDTV.Utilities
{
    internal static class ConfigUtility
    {
        /// <summary>
        /// This method gets the value associated with a particular key present in a given section
        /// </summary>
        /// <param name="sectionName">Name of the section where key is present</param>
        /// <param name="key">Name of the key</param>
        /// <returns>value associated with the key</returns>
        internal static string GetConfigValues(string sectionName, string key)
        {
            string value = string.Empty;
            try
            {
                value = ((NameValueCollection)ConfigurationManager.GetSection(sectionName))[key];
            }
            catch (ConfigurationErrorsException)
            {
                value = string.Empty;
            }
            return value;
        }       

        /// <summary>
        /// Gets the app settings value for the given key
        /// </summary>
        /// <param name="appSettingsKey">App settings key</param>
        /// <returns>Value for the given key</returns>
        internal static string GetAppSettingsValue(string appSettingsKey)
        {
            string configValue = string.Empty;
            try
            {
                configValue = ConfigurationManager.AppSettings[appSettingsKey];
            }
            catch (ConfigurationErrorsException)
            {
                configValue = string.Empty;
            }
            return configValue;
        }

        /// <summary>
        /// This method gets keys present in a given section
        /// </summary>
        /// <param name="sectionName">Name of the section where key is present</param>
        /// <returns>collection - keys present in section</returns>
        internal static Collection<string> GetKeysFromSection(string sectionName)
        {
            Collection<string> errorCodes = new Collection<string>();
            string[] keys = null;
            try
            {
                keys = ((NameValueCollection)ConfigurationManager.GetSection(sectionName)).AllKeys;
                foreach (string key in keys)
                {
                    errorCodes.Add(key);
                }
            }
            catch (ConfigurationErrorsException)
            {
                errorCodes = null;
            }
            return errorCodes;
        }
    }
}
