using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;

namespace NDTV.SlateApp.Converter
{
    public class ImageAlbumDispalyConverter : IValueConverter
    {
        /// <summary>
        /// Convert the Text value into an nappropriate text that suits Album Collection
        /// </summary>
        /// <param name="value">object sent</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">parameters if any sent</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string returnValue = string.Empty;
            if (null != value && value.GetType() == typeof(int))
            {
                returnValue = string.Format(CultureInfo.InvariantCulture,"{0} Images", int.Parse(value.ToString(),CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture));
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
