using System;
using System.Globalization;
using System.Windows.Data;

namespace NDTV.SlateApp.Converter
{
    /// <summary>
    /// This class helps to format duration comming as a string in seconds unit.
    /// </summary>
    public sealed class DurationConverter : IValueConverter
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
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string formattedDuration = string.Empty;
            UInt32 input = 0, seconds = 0, minutes = 0, hours = 0;
            string secondsInString = value as string;

            if (false == String.IsNullOrEmpty(secondsInString))
            {
                input = Convert.ToUInt32(secondsInString, CultureInfo.InvariantCulture);

                seconds = input > 0 ? input % 60 : 0;
                input = input > 0 ? (input - seconds) / 60 : 0;
                minutes = input > 0 ? (input % 60) : 0;
                hours = input > 0 ? input / 60 : 0;                               
                if (hours > 0)
                {
                    /* Format: hh:mm:ss */
                    formattedDuration = Convert.ToString(hours, CultureInfo.InvariantCulture) + ":" +
                                        string.Format(CultureInfo.InvariantCulture, "{0:d2}", minutes) + ":" +
                                        string.Format(CultureInfo.InvariantCulture, "{0:d2}", seconds);

                }
                else
                {
                    if (minutes > 0)
                    {
                        /* When hours is 0, Format: m:ss */
                        formattedDuration = Convert.ToString(minutes, CultureInfo.InvariantCulture) + ":" +
                                            string.Format(CultureInfo.InvariantCulture,"{0:d2}", seconds);
                    }
                    else
                    {
                        /* When mins is 0, Format: mm:ss */
                        formattedDuration = string.Format(CultureInfo.InvariantCulture, "{0:d2}", minutes) + ":" +
                                            string.Format(CultureInfo.InvariantCulture, "{0:d2}", seconds);

                    }

                }                
            }
            return formattedDuration;
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
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
