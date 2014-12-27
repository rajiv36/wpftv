using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDTV.Entities
{
    /// <summary>
    /// Entity to hold the details of the Market.
    /// </summary>
    public class StockMarketData
    {
        /// <summary>
        /// Id associated with the concerned Market Entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title/Name of the concerned Market Entity.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Link to obtain the details with regard to the Market Entity.
        /// </summary>
        public string Link { get; set; }
    }
}
