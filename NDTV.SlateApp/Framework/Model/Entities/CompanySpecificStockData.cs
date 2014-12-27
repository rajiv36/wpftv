namespace NDTV.Entities
{
    /// <summary>
    /// Specific details associated with the necessary company.
    /// </summary>
    public class CompanySpecificStockData
    {
        /// <summary>
        /// The concerned ISIN number of the company.
        /// </summary>
        public string ISIN { get; set; }

        /// <summary>
        /// Net Dividend.
        /// </summary>
        public float Dividend { get; set; }

        /// <summary>
        /// Name of the company.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// PE value associated with the company.
        /// </summary>
        public float PEValue { get; set; }

        /// <summary>
        /// The sector to which the company belongs.
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        /// Net market capital.
        /// </summary>
        public float MarketCapital { get; set; }

        /// <summary>
        /// Id associated with the company.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Short name associated with the company.
        /// </summary>
        public string ShortName { get; set; }
        
        /// <summary>
        /// Specific details of the company associated with the BSE exchange.
        /// </summary>
        public StockExchangeSpecificDetails BombayStockExchangeDetails { get; set; }

        /// <summary>
        /// Specific details of the company associated with the NSE exchange.
        /// </summary>
        public StockExchangeSpecificDetails NationalStockExchangeDetails { get; set; }
    }
}
