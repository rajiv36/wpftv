using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;

namespace NDTV.SlateApp.Converter
{
    public class TimeConverter : IValueConverter
    {
        /// <summary>
        /// Converting the URI into a bitmapImage object which gets easily bound to the user interface..
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string returnValue = string.Empty;
            if (null != value)
            {
                int sec = 0;
                int.TryParse(value.ToString(),out sec);
                TimeSpan videoSpan = new TimeSpan(0, 0, 0, sec);
                returnValue = videoSpan.ToString();
            }
            return returnValue;
        }

        /// <summary>
        /// This method would not be called in our case as the image cannot be altered back from the user interface.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
