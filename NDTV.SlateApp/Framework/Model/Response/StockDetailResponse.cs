using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace NDTV.Entities
{
    public class StockDetailResponse :Response
    {
        /// <summary>
        /// Stock Item Details.
        /// </summary>
        private StockItem selectedItemStockDetails;

        /// <summary>
        /// Stock Item Details.
        /// </summary>
        public ObservableCollection<StockItemDetails> SelectedItemStockDetails
        {
            get 
            {
                if (null != this.selectedItemStockDetails.Items && this.selectedItemStockDetails.Items.Count > 0)
                {
                    return (new ObservableCollection<StockItemDetails>(this.selectedItemStockDetails.Items));
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseMessage">Response Message</param>
        public StockDetailResponse(string responseMessage)
            : base(responseMessage)
        {
            this.selectedItemStockDetails = new StockItem();
            this.selectedItemStockDetails.Items = new List<StockItemDetails>();

            XElement element;
            bool isXMLParse = Utilities.Utility.XElementTryParse(responseMessage,out element);
            if (isXMLParse)
            {
                XMLParse();
            }
            else
            {
                JSONParse();
            }
        }

        /// <summary>
        /// JSON Parser for the received JSON.
        /// </summary>
        private void JSONParse()
        {
            if (!string.IsNullOrEmpty(responseMessage))
            {
                this.selectedItemStockDetails = Utilities.Utility.Deserialize<StockItem>(responseMessage);
            }
        }

        /// <summary>
        /// XML Parser for the received XML.
        /// </summary>
        private void XMLParse()
        {
            XElement element = XElement.Parse(responseMessage);
            float min = 0;

            var elementList = (from eachItem in element.Elements("element")
                               select new StockItemDetails()
                               {
                                   Name = (null != eachItem.Element("name").Value) ? (eachItem.Element("name").Value.ToString()) : string.Empty,
                                   ShortName = (null != eachItem.Element("short_name").Value) ? (eachItem.Element("short_name").Value.ToString()) : string.Empty,
                                   Id = (null != eachItem.Element("id").Value) ? int.Parse(eachItem.Element("id").Value,CultureInfo.InvariantCulture) : int.MinValue,
                                   Change = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("change").Value) ? float.Parse(eachItem.Element("bse").Element("change").Value, CultureInfo.InvariantCulture) : min,
                                   Chart = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("graph").Value) ? eachItem.Element("bse").Element("graph").Value : string.Empty,
                                   HighPrice = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("high_price").Value) ? float.Parse(eachItem.Element("bse").Element("high_price").Value, CultureInfo.InvariantCulture) : min,
                                   LastTradedPrice = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("last_traded_price").Value) ? float.Parse(eachItem.Element("bse").Element("last_traded_price").Value, CultureInfo.InvariantCulture) : min,
                                   LowPrice = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("low_price").Value) ? float.Parse(eachItem.Element("bse").Element("low_price").Value, CultureInfo.InvariantCulture) : min,
                                   PriceDifference = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("price_diff").Value) ? float.Parse(eachItem.Element("bse").Element("price_diff").Value, CultureInfo.InvariantCulture) : min,
                                   TotalTradedQuantity = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("volume").Value) ? long.Parse(eachItem.Element("bse").Element("volume").Value,CultureInfo.InvariantCulture) : long.MinValue,
                               }).ToList();

            for (int elementIndex = 0; elementIndex < elementList.Count; elementIndex++)
            {
                this.selectedItemStockDetails.Items.Add(elementList.ElementAt(elementIndex));
            }
        }
    }
}
