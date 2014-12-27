using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Details associated with the Stock Index Request required by the Stock Ticker
    /// </summary>
    public class StockIndexRequest :  Request
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public StockIndexRequest()
            : base("StockIndexRequest", Utility.GetLink(Constants.LinkNames.StockIndexesLink), HttpOperation.Get, null, Utilities.Utility.GetStockIndexCredentials(Constants.Constant.Username), Utilities.Utility.GetStockIndexCredentials(Constants.Constant.Password), HttpAuthentication.Digest)
        {

        }

        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string.</param>
        /// <returns>Response object.</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            StockIndexResponse response = new StockIndexResponse(responseString);
            return response;
        }
       
    }
}
