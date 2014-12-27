using System.Windows;
using System.Windows.Data;

namespace NDTV.SlateApp.Converter
{
    public class ImageForwardButtonEnableConverter : IMultiValueConverter
    {
        /// <summary>
        /// Takes care of enabling and disabling of the forward arrow image gallery button.
        /// </summary>
        /// <param name="value">The set of values.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Contains the number of items that should be bind to the user interface</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Returns the converted object.</returns>
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object isEnabled = Visibility.Hidden;
            if (values != null && values.Length > 0 && values[0] != null && values[1] != null)
            {
                int currentItem;
                int totalNumberOfItems;

                int.TryParse(values[0].ToString(), out totalNumberOfItems);
                int.TryParse(values[1].ToString(), out currentItem);

                if (totalNumberOfItems == currentItem + 1)
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
        /// <param name="value">The set of values.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Contains the number of items that should be bind to the user interface</param>
        /// <param name="culture">The Culture.</param>
        /// <returns>Converts back the converted object.</returns>
        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
