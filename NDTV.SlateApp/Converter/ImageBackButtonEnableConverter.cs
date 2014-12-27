using System.Windows;
using System.Windows.Data;

namespace NDTV.SlateApp.Converter
{
    /// <summary>
    /// Image Back Button Enable Converter class.
    /// </summary>
    public class ImageBackButtonEnableConverter : IValueConverter
    {
        /// <summary>
        /// Takes care of enabling and disabling of the Back arrow image gallery button.
        /// </summary>
        /// <param name="value">The value. </param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Contains the number of items that should be bind to the user interface</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Returns the converted object.</returns>
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object isEnabled = Visibility.Hidden;
            if (value != null)
            {
                int currentItem;
                int.TryParse(value.ToString(), out currentItem);
                if (currentItem == 0)
                {
                    isEnabled = Visibility.Hidden;
                }
                else
                {
                    isEnabled = Visibility.Visible;
                }
            }
            return isEnabled;
        }

        /// <summary>
        /// Convert Back.
        /// </summary>
        /// <param name="value">the value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Contains the number of items that should be bind to the user interface</param>
        /// <param name="culture">The Culture.</param>
        /// <returns>Converts back the converted object.</returns>
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
