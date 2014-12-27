using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDTV.SlateApp.Framework.CustomEventArgs
{
    /// <summary>
    /// Search Started Event Arguments
    /// </summary>
    public class SearchStartedEventArgs : EventArgs
    {
        /// <summary>
        /// Search String
        /// </summary>
        private string searchString;

        /// <summary>
        /// Search String
        /// </summary>
        public string SearchString
        {
            get { return searchString; }
            set { searchString = value; }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SearchStartedEventArgs()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchString">Search String</param>
        public SearchStartedEventArgs(string searchString)
        {
            this.SearchString = searchString;
        }
    }
}
