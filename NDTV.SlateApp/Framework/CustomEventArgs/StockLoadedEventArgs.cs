using System;
using System.Collections.Generic;
using NDTV.Entities;

namespace NDTV.SlateApp.Framework.CustomEventArgs
{
    /// <summary>
    /// Stock Loaded Event arguments.
    /// </summary>
    public class StocksLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="stocks"></param>
        public StocksLoadedEventArgs(List<StockItemDetails> stocks)
        {
            this.Stocks = stocks;
        }

        /// <summary>
        /// The List of Stocks.
        /// </summary>
        public IList<StockItemDetails> Stocks { get; private set; }
    }
}
