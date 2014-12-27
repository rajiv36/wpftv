using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace NDTV.SlateApp.Converter
{

    /// <summary>
    /// Make sure to add a new class for each type of converter, in this case it is a TopStories List converter..
    /// </summary>
    public class ListRangeTrimmingConverter : IValueConverter
    {
        private const char SplitDelimiter = '|';

        /// <summary>
        /// Trimming a list as per your parametric requirement and make it one way..
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Contains the number of items that should be binded to the user interface..</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null != value)
            {
                List<int> range = (parameter.ToString()).Split(SplitDelimiter).ToList().ConvertAll<int>(x => int.Parse(x, CultureInfo.InvariantCulture));
                return ((range.Count > 1) ? ((IEnumerable<object>)value).Skip(range[0] - 1).Take(range[1] - range[0] + 1).ToList()
                                                                : ((IEnumerable<object>)value).Take(range[0]).ToList());
            }
            else
                return new List<object>();
        }

        /// <summary>
        /// It is needed only when we have to track back an object to the view model based on a user event invoked..
        /// Example: say category item has to be sent back on click of a category combobox
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
