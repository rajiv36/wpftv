using System;
using System.Collections.Generic;
using NDTV.Entities;

namespace NDTV.SlateApp.Framework.CustomEventArgs
{
    /// <summary>
    /// Events Arguments class to indicate Search Completion
    /// </summary>
    public class SearchCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Search Kind (Articles,Photos,Videos)
        /// </summary>
        public SearchType SearchKind { get; set; }

        /// <summary>
        /// Search Results
        /// </summary>
        public IList<Item> SearchResults { get; set; }
    }
}
