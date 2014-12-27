using System.Linq;
using System.Xml.Linq;
using System.Globalization;

namespace NDTV.Entities
{
    public class CompanySpecificStockDataResponse : Response
    {
        /// <summary>
        /// Company specific stock data.
        /// </summary>
        public CompanySpecificStockData CompanySpecificData { get; set;}

        public CompanySpecificStockDataResponse(string responseMessage)
            : base(responseMessage)
        {
            Parse();
        }

        /// <summary>
        /// Function to Parse the response Message.
        /// </summary>
        private void Parse()
        {
            XElement element = XElement.Parse(responseMessage);
            float min = 0;
            long longMin = 0;
            var elementList = (from eachItem in element.Elements("element")
                               select new CompanySpecificStockData()
                               {
                                   ISIN = (null!=eachItem.Element("isin").Value) ? eachItem.Element("isin").Value:string.Empty,
                                   Dividend = (null!=eachItem.Element("dividend").Value)?float.Parse(eachItem.Element("dividend").Value,CultureInfo.InvariantCulture):min,
                                   PEValue = (null != eachItem.Element("pe").Value) ? float.Parse(eachItem.Element("pe").Value, CultureInfo.InvariantCulture) : min,
                                   Name = (null!=eachItem.Element("name").Value)?(eachItem.Element("name").Value.ToString()):string.Empty,
                                   MarketCapital = (null != eachItem.Element("market_cap").Value) ? float.Parse(eachItem.Element("market_cap").Value, CultureInfo.InvariantCulture) : min,
                                   ShortName = (null != eachItem.Element("short_name").Value)?(eachItem.Element("short_name").Value.ToString()):string.Empty,
                                   Id = (null != eachItem.Element("id").Value) ? int.Parse(eachItem.Element("id").Value, CultureInfo.InvariantCulture) : int.MinValue,
                                   Sector = (null != eachItem.Element("sector").Value)?eachItem.Element("sector").Value:string.Empty,
                                   NationalStockExchangeDetails = new StockExchangeSpecificDetails()
                                   {
                                       ExchangeName = "NSE",
                                       Change = !string.IsNullOrWhiteSpace(eachItem.Element("nse").Element("change").Value) ? float.Parse(eachItem.Element("nse").Element("change").Value, CultureInfo.InvariantCulture) : min,
                                       Code = !string.IsNullOrWhiteSpace(eachItem.Element("nse").Element("code").Value) ? eachItem.Element("nse").Element("code").Value : string.Empty,
                                       GraphLink = !string.IsNullOrWhiteSpace(eachItem.Element("nse").Element("graph").Value) ? eachItem.Element("nse").Element("graph").Value : string.Empty,
                                       HighPrice = !string.IsNullOrWhiteSpace(eachItem.Element("nse").Element("high_price").Value) ? float.Parse(eachItem.Element("nse").Element("high_price").Value, CultureInfo.InvariantCulture) : min,
                                       LastTradedPrice = !string.IsNullOrWhiteSpace(eachItem.Element("nse").Element("last_traded_price").Value) ? float.Parse(eachItem.Element("nse").Element("last_traded_price").Value, CultureInfo.InvariantCulture) : min,
                                       LowPrice = !string.IsNullOrWhiteSpace(eachItem.Element("nse").Element("low_price").Value) ? float.Parse(eachItem.Element("nse").Element("low_price").Value, CultureInfo.InvariantCulture) : min,
                                       PreviousClose = !string.IsNullOrWhiteSpace(eachItem.Element("nse").Element("prev_close").Value) ? float.Parse(eachItem.Element("nse").Element("prev_close").Value, CultureInfo.InvariantCulture) : min,
                                       PriceDifference = !string.IsNullOrWhiteSpace(eachItem.Element("nse").Element("price_diff").Value) ? float.Parse(eachItem.Element("nse").Element("price_diff").Value, CultureInfo.InvariantCulture) : min,
                                       Time = !string.IsNullOrWhiteSpace(eachItem.Element("nse").Element("time").Value) ? eachItem.Element("nse").Element("time").Value : string.Empty,
                                       Volume = !string.IsNullOrWhiteSpace(eachItem.Element("nse").Element("volume").Value) ? long.Parse(eachItem.Element("nse").Element("volume").Value, CultureInfo.InvariantCulture) : longMin,
                                       YearHigh = !string.IsNullOrWhiteSpace(eachItem.Element("nse").Element("year_high").Value) ? float.Parse(eachItem.Element("nse").Element("year_high").Value, CultureInfo.InvariantCulture) : min,
                                       YearLow = !string.IsNullOrWhiteSpace(eachItem.Element("nse").Element("year_low").Value) ? float.Parse(eachItem.Element("nse").Element("year_low").Value, CultureInfo.InvariantCulture) : min
                                   
                                   },
                                   BombayStockExchangeDetails = new StockExchangeSpecificDetails()
                                   {
                                       ExchangeName = "BSE",
                                       Change = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("change").Value) ? float.Parse(eachItem.Element("bse").Element("change").Value, CultureInfo.InvariantCulture) : min,
                                       Code = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("code").Value) ? eachItem.Element("bse").Element("code").Value : string.Empty,
                                       GraphLink = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("graph").Value) ? eachItem.Element("bse").Element("graph").Value : string.Empty,
                                       HighPrice = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("high_price").Value) ? float.Parse(eachItem.Element("bse").Element("high_price").Value, CultureInfo.InvariantCulture) : min,
                                       LastTradedPrice = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("last_traded_price").Value) ? float.Parse(eachItem.Element("bse").Element("last_traded_price").Value, CultureInfo.InvariantCulture) : min,
                                       LowPrice = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("low_price").Value) ? float.Parse(eachItem.Element("bse").Element("low_price").Value, CultureInfo.InvariantCulture) : min,
                                       PreviousClose = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("prev_close").Value) ? float.Parse(eachItem.Element("bse").Element("prev_close").Value, CultureInfo.InvariantCulture) : min,
                                       PriceDifference = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("price_diff").Value) ? float.Parse(eachItem.Element("bse").Element("price_diff").Value, CultureInfo.InvariantCulture) : min,
                                       Time = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("time").Value) ? eachItem.Element("bse").Element("time").Value : string.Empty,
                                       Volume = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("volume").Value) ? long.Parse(eachItem.Element("bse").Element("volume").Value, CultureInfo.InvariantCulture) : long.MinValue,
                                       YearHigh = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("year_high").Value) ? float.Parse(eachItem.Element("bse").Element("year_high").Value, CultureInfo.InvariantCulture) : min,
                                       YearLow = !string.IsNullOrWhiteSpace(eachItem.Element("bse").Element("year_low").Value) ? float.Parse(eachItem.Element("bse").Element("year_low").Value, CultureInfo.InvariantCulture) : min
                                   }
                               }).ToList();

            if (elementList.Count != 1)
            {
                this.CompanySpecificData = null;
            }
            else
            {
                this.CompanySpecificData = elementList[0]; 
            }

        }
    }
}
