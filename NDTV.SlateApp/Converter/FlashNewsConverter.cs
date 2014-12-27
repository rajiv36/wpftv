using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using NDTV.Entities;

namespace NDTV.SlateApp.Converter
{
    public class FlashNewsConverter:IValueConverter
    {
        /// <summary>
        /// Bind the data associated with the flash news object based on the flag
        /// </summary>
        /// <param name="value">latest news/ any object as bound in the UI</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">
        /// These numbers should be reflected in the xaml appropriately, 
        /// 0 is used for visibilty of the flash news section,
        /// 1 is used for returning type 1 data as shown in the case statement
        /// Remember it should allways be an integer
        /// </param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LatestNews latestNews = value as LatestNews;
            switch (int.Parse((string)parameter, CultureInfo.InvariantCulture))
            {
                case 0:
                    return ((null != value && (latestNews).IsFlashNewsExist) ? Visibility.Visible : Visibility.Collapsed);
                case 1:
                    return ((null != value && (latestNews).IsFlashNewsExist) ? (latestNews).latestNewsCollection[0].FlashNews : "");
                default:
                    return "";
            }
        }

        /// <summary>
        /// This method would not be called in our case as the flash news cannot be altered back from the user interface.
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
