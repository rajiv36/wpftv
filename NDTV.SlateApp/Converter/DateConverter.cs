using System;
using System.Globalization;
using System.Windows.Data;

namespace NDTV.SlateApp.Converter
{
    /// <summary>
    /// This converter helps in converting string format date to MMM dd yyyy format. 
    /// </summary>
    public class DateConverter: IValueConverter
    {
        #region PUBLIC METHODS

        /// <summary>
        ///  Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target</param>
        /// <param name="targetType">The System.Type of data expected by the target dependency property</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic</param>
        /// <param name="culture">The culture of the conversion</param>
        /// <returns>The value to be passed to the target dependency property</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           
            if (null != parameter && false == string.IsNullOrWhiteSpace(parameter.ToString()))
            {
                string converterParameter = parameter.ToString();
                DateTime searchResultsDate;
                string dateStringValue = value.ToString();
                string returnValue = string.Empty;
                if (converterParameter == "SearchResultsDateFormat" && false==string.IsNullOrWhiteSpace(dateStringValue))
                {
                    if (dateStringValue.Contains("IST"))
                    {
                        dateStringValue = dateStringValue.Remove(dateStringValue.IndexOf("IST",StringComparison.OrdinalIgnoreCase), 3);
                    }
                    bool isParseSuccessful = DateTime.TryParse(dateStringValue, out searchResultsDate);
                    if (isParseSuccessful)
                    {
                        returnValue = searchResultsDate.ToString("MMM dd,yyyy hh:mm IST",CultureInfo.InvariantCulture);
                    }
                }
                return returnValue;
            }
            else
            {
                if (null != value)
                {
                    return (System.Convert.ToDateTime(value.ToString().Substring(4, 13), CultureInfo.InvariantCulture).ToString("MMM dd yyyy", CultureInfo.InvariantCulture));
                }
                return null;
            }
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method
        /// is called only in System.Windows.Data.BindingMode.TwoWay bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source</param>
        /// <param name="targetType">The System.Type of data expected by the source object</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic</param>
        /// <param name="culture">The culture of the conversion</param>
        /// <returns>The value to be passed to the source object</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
