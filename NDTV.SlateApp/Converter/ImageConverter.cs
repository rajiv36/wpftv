using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.IO;
using NDTV.Entities;


namespace NDTV.SlateApp.Converter
{
    /// <summary>
    /// Converter needed to convert the Image URI into an Bitmap image ,
    /// We infact can use the same method to 
    /// convert URI to String and viceversa
    /// convert URI to any object and viceversa etc.,
    /// </summary>
    public class ImageConverter:IValueConverter
    {
        /// <summary>
        /// Converting the URI into a bitmapImage object which gets easily bound to the user interface..
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">a flag which tells which default image path is to be sent back</param>
        /// <param name="culture">Globalisation - CultureInfo</param>
        /// <returns>object - image path</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null != value && value.GetType() == typeof(string))
            {
                value = new Uri(value.ToString());
            }
            if (null != value && false == string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new BitmapImage((Uri)value);
            }
            else
            {
                if (null != parameter && parameter.ToString().Equals(Constants.Constant.DefaultImage))
                {
                    return Utilities.Utility.FetchDefaultNdtvImageSource(Constants.Constant.DefaultImage);
                }
                else if (null != parameter && parameter.ToString().Equals(Constants.Constant.DefaultImageForRelatedNews))
                {
                    return Utilities.Utility.FetchDefaultNdtvImageSource(Constants.Constant.DefaultImageForRelatedNews);
                }
                else
                {
                    return new BitmapImage();
                }
            }
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
