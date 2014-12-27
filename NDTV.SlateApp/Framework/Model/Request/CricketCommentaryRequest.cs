using System.Text;
using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Request for cricket commentary
    /// </summary>
    public class CricketCommentaryRequest : Request
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CricketCommentaryRequest(string link)
            : base("CricketCommentaryRequest", BuildCommentaryLink(link), HttpOperation.Get)
        {
            enableCache = false;
        }

        /// <summary>
        /// Build the commentary link
        /// </summary>
        /// <param name="linkString">URL</param>
        /// <returns>complete URL</returns>
        private static string BuildCommentaryLink(string commentaryLink)
        {
            StringBuilder baseUrl = new StringBuilder(Utility.GetLink(Constants.LinkNames.CricketCommentary));
            baseUrl.Append(commentaryLink);
            baseUrl.Append(Constants.Constant.TextExtension);
            return baseUrl.ToString();
        }


        /// <summary>
        /// Overridden method to build response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            CricketCommentaryResponse response = new CricketCommentaryResponse(responseString);
            return response;
        }
    }
}
