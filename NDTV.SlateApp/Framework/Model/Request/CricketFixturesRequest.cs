using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Request for cricket fixtures
    /// </summary>
    public class CricketFixturesRequest : Request
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CricketFixturesRequest()
            : base("CricketFixturesRequest", Utility.GetLink(Constants.LinkNames.CricketFixtures), HttpOperation.Get)
        {

        }

        /// <summary>
        /// Overridden method to build response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            CricketFixturesResponse response = new CricketFixturesResponse(responseString);
            return response;
        }
    }
}
