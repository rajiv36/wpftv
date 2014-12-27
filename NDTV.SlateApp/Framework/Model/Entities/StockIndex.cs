
namespace NDTV.Entities
{
    /// <summary>
    /// All the properties required for a ProfitIndexItem is found here
    /// </summary>
    public class StockIndexItem
    {
        /// <summary>
        /// Id of the Stock Index Item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Associated graph indicating performance.
        /// </summary>
        public string Graph { get; set; }

        /// <summary>
        /// Highest value reached during the day
        /// </summary>
        public float High { get; set; }

        /// <summary>
        /// Exchange name
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// The percentage change associated with the exchange (Percentage Difference between open price and current price)
        /// </summary>
        public float PercentageChange { get; set; }

        /// <summary>
        /// Lowest value reached during the day
        /// </summary>
        public float Low { get; set; }

        /// <summary>
        /// Time associated with the response
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Full name of the Stock Item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Open value
        /// </summary>
        public float Open { get; set; }

        /// <summary>
        /// Previous closing value
        /// </summary>
        public float Close { get; set; }

        /// <summary>
        /// Price change.
        /// </summary>
        public float PriceChange { get; set; }

        /// <summary>
        /// Sector associated with the Company.
        /// </summary>
        public string Sector { get; set; }
        
        /// <summary>
        /// Current Stock value 
        /// </summary>
        public float CurrentStockValue
        {
            get
            {
               return (this.Open+this.PriceChange);
            }
        }

        /// <summary>
        /// Stock direction (if positive or negative)
        /// </summary>
        public string StockDirection
        {
            get
            {
                if (PriceChange < 0)
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
