using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace NDTV.SlateApp.Converter
{
    public class ChartUrlConverter : IValueConverter
    {
        /// <summary>
        /// Converter to parse the obtained Chart Url(to remove the unnecessary characters)
        /// </summary>
        /// <param name="value">The required parameter</param>
        /// <returns>Parsed URL</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parsedUrl = string.Empty;
            int tempIndex = 0;
            const string backgroundColor = "&chf=bg,s,D5D4DA";
            
            if (false==(null != parameter && false == string.IsNullOrWhiteSpace(parameter.ToString())))
            {
                if (value != null && value.ToString().Length > 0)
                {
                    StringBuilder finalString;
                    string temp = value.ToString();

                    //Look for the source property.
                    if (temp.Contains("src"))
                    {
                        finalString = new StringBuilder();

                        //Find index of the source property.
                        tempIndex = temp.IndexOf("src=", StringComparison.OrdinalIgnoreCase);

                        //Move till the " character.
                        while (temp.Substring(tempIndex, 1) != "\"")
                        {
                            tempIndex++;
                        }

                        //Start extracting.
                        tempIndex = tempIndex + 1;

                        //Extract till the next " character.
                        while (temp.Substring(tempIndex, 1) != "\"")
                        {
                            finalString.Append(temp.Substring(tempIndex, 1));
                            tempIndex = tempIndex + 1;
                        }
                        //Final parsed Url.
                        finalString = finalString.Replace("&amp;", "&");
                        parsedUrl = finalString.Append(backgroundColor).ToString();
                    }
                    else if (temp.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    {
                        string basicUrl = "http://charts.profit.ndtv.com/chart?cht=ls&chs=50x20";

                        finalString = new StringBuilder(basicUrl);

                        string[] splitStrings = temp.Split('&');
                        foreach (var item in splitStrings)
                        {
                            string[] keyValuePairs = item.Split('=');
                            if (false == string.IsNullOrWhiteSpace(keyValuePairs[0]))
                            {
                                if (keyValuePairs[0].Contains("chco") || keyValuePairs[0].Contains("chd"))
                                {
                                    finalString.Append("&");
                                    finalString.Append(item);
                                }
                            }
                        }

                        finalString.Append(backgroundColor);
                        parsedUrl = finalString.ToString();
                    }
                }
            }
            else
            {
                string temp = value.ToString();
                const string backColor = "&chf=bg,s,151515";

                tempIndex = temp.IndexOf("chco=",StringComparison.OrdinalIgnoreCase);
                tempIndex = tempIndex + 1;
                temp = temp + backColor;
                parsedUrl = temp;
            }
            return parsedUrl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
