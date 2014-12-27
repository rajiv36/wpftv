using NDTV.Utilities;

namespace NDTV.Entities
{
    public class CricketShortScorecardRequest :Request
    {
         /// <summary>
        /// Default constructor
        /// </summary>
        public CricketShortScorecardRequest()
            : base("CricketShortScorecardRequest", Utility.GetLink(Constants.LinkNames.CricketShortScorecardLink), HttpOperation.Get)
        {
            this.enableCache = false;
        }

        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string.</param>
        /// <returns>Response object.</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            CricketShortScorecardResponse response = new CricketShortScorecardResponse(responseString);
            return response;
        }
    }
}
