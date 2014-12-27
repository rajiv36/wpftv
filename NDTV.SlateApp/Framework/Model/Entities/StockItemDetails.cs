using System.Runtime.Serialization;

namespace NDTV.Entities
{
    /// <summary>
    /// Class to store the individual values associated with Stock Item Details.
    /// </summary>
    [DataContract]
    public class StockItemDetails
    {
        /// <summary>
        /// Name of the concerned stock
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Associated Slug
        /// </summary>
        [DataMember(Name = "slug")]
        public string Slug { get; set; }

        /// <summary>
        /// Highest reached price during the day
        /// </summary>
        [DataMember(Name = "high_price")]
        public float HighPrice { get; set; }

        /// <summary>
        /// Change experienced (Open price - current price)
        /// </summary>
        [DataMember(Name = "change")]
        public float Change { get; set; }

        /// <summary>
        /// Short name of the company
        /// </summary>
        [DataMember(Name = "short_name")]
        public string ShortName { get; set; }

        /// <summary>
        /// The Id of the company.
        /// </summary>
        [DataMember(Name = "company_id")]
        public int Id { get; set; }

        /// <summary>
        /// Current performance chart
        /// </summary>
        [DataMember(Name = "chart")]
        public string Chart { get; set; }

        /// <summary>
        /// Lowest price reached during the day
        /// </summary>
        [DataMember(Name = "low_price")]
        public float LowPrice { get; set; }

        /// <summary>
        /// Volume traded
        /// </summary>
        [DataMember(Name = "total_traded_quantity")]
        public long TotalTradedQuantity { get; set; }

        /// <summary>
        /// Last traded price
        /// </summary>
        [DataMember(Name = "last_traded_price")]
        public float LastTradedPrice { get; set; }

        /// <summary>
        /// Current price difference
        /// </summary>
        [DataMember(Name = "price_diff")]
        public float PriceDifference { get; set; }

        /// <summary>
        /// Stock Direction (Positive or Negative based on the current values)
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
