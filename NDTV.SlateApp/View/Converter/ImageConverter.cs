using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;
using System.Windows.Media.Imaging;


namespace NDTV.SlateApp
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
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return ((value != null) ? new BitmapImage((Uri)value) : new BitmapImage());
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
