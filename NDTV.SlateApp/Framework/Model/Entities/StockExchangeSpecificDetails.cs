using NDTV.Entities;

namespace NDTV.Entities
{
    /// <summary>
    /// Specific details of the company associated with the desired exchange.
    /// </summary>
    public class StockExchangeSpecificDetails
    {
        /// <summary>
        /// Name of the exchange
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Lowest price reached during the day
        /// </summary>
        public float LowPrice { get; set; }

        /// <summary>
        /// Change in the stock value
        /// </summary>
        public float Change { get; set; }

        /// <summary>
        /// The last traded price
        /// </summary>
        public float LastTradedPrice { get; set; }

        /// <summary>
        /// 52 week low
        /// </summary>
        public float YearLow { get; set; }

        /// <summary>
        /// Highest price reached during the day
        /// </summary>
        public float HighPrice { get; set; }

        /// <summary>
        /// Associated time
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Performance graph.
        /// </summary>
        public string GraphLink { get; set; }

        /// <summary>
        /// Code associated with the performance
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 52 week high
        /// </summary>
        public float YearHigh { get; set; }

        /// <summary>
        /// previous close value
        /// </summary>
        public float PreviousClose { get; set; }

        /// <summary>
        /// Last traded quantity
        /// </summary>
        public long Volume { get; set; }

        /// <summary>
        /// Price difference
        /// </summary>
        public float PriceDifference { get; set; }

        /// <summary>
        /// Current stock value
        /// </summary>
        public float CurrentStockValue
        {
            get
            {
                return PreviousClose + PriceDifference;
            }
        }

        /// <summary>
        /// Stock Direction (Positive or Negative)
        /// </summary>
        public string StockDirection
        {
            get
            {
                if (Change < 0)
                {
                    return Direction.Down.ToString();
                }
                else
                {
                    return Direction.Up.ToString();
                }
            }
        }
    }
}
