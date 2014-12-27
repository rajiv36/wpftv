using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NDTV.Entities
{
    /// <summary>
    /// Class to store a list of Stock Item Details.
    /// </summary>
    [DataContract]
    public class StockItem
    {
        /// <summary>
        /// List of Stock Item details
        /// </summary>
        [DataMember(Name = "values")]
        public List<StockItemDetails> Items { get; set; }
    }

    
}
