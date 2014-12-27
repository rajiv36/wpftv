using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace NDTV.Entities
{
    /// <summary>
    /// Details associated with the Stock Index Response required by the Stock Ticker
    /// </summary>
    public class StockIndexResponse : Response
    {
        /// <summary>
        /// Collection to store the details of the Stock Market.
        /// </summary>
        private ObservableCollection<StockIndexItem> stockIndexList;

        /// <summary>
        /// Collection to store the details of the Stock Market.
        /// </summary>
        public ObservableCollection<StockIndexItem> StockIndexList
        {
            get 
            { 
                return stockIndexList; 
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response</param>
        public StockIndexResponse(string responseMessage)
            :base(responseMessage)
        {
            this.stockIndexList = new ObservableCollection<StockIndexItem>();
            Parse();
        }

        /// <summary>
        /// Parse the response and fill the appropriate model.
        /// </summary>
        private void Parse()
        {
            int result;
            float floatResult;

            XElement element = XElement.Parse(responseMessage);
            if (null != element && null != element.Elements())
            {
                var stockIndices = (from eachItem in element.Elements("element")
                                    select new StockIndexItem()
                                    {
                                        Id = (null != eachItem.Element("id")) ? (int.TryParse(eachItem.Element("id").Value, NumberStyles.None, CultureInfo.InvariantCulture,out result) ? result : -1) : -1,
                                        High = (null != eachItem.Element("high")) ? (float.TryParse(eachItem.Element("high").Value, out floatResult) ? floatResult : -1) : -1,
                                        Low = (null != eachItem.Element("low")) ? (float.TryParse(eachItem.Element("low").Value, out floatResult) ? floatResult : -1) : -1,
                                        Open = (null != eachItem.Element("open")) ? (float.TryParse(eachItem.Element("open").Value, out floatResult) ? floatResult : -1) : -1,
                                        Close = (null != eachItem.Element("close")) ? (float.TryParse(eachItem.Element("close").Value, out floatResult) ? floatResult : -1) : -1,
                                        PercentageChange = (null != eachItem.Element("percent_change")) ? (float.TryParse(eachItem.Element("percent_change").Value, out floatResult) ? floatResult : -1) : -1,
                                        PriceChange = (null != eachItem.Element("price_change")) ? (float.TryParse(eachItem.Element("price_change").Value, out floatResult) ? floatResult : -1) : -1,
                                        Exchange = (null != eachItem.Element("exchange")) ? eachItem.Element("exchange").Value.ToString() : string.Empty,
                                        Name = (null != eachItem.Element("name")) ? eachItem.Element("name").Value.ToString() : string.Empty,
                                        Graph = (null != eachItem.Element("graph")) ? eachItem.Element("graph").Value.ToString() : string.Empty,
                                        Time = (null != eachItem.Element("time")) ? (eachItem.Element("time").Value.ToString()) : string.Empty,
                                        Sector = (null != eachItem.Element("sector")) ? eachItem.Element("sector").Value.ToString() : string.Empty
                                    }).ToList();
                for (int elementIndex = 0; elementIndex < stockIndices.Count; elementIndex++)
                {
                    var currentElement = stockIndices[elementIndex];
                    this.stockIndexList.Add(currentElement);
                }
            }
        }
    }
}
