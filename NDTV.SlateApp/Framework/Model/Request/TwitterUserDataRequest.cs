
using NDTV.Controller;

namespace NDTV.Entities
{
    public class TwitterUserDataRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TwitterUserDataRequest() :
            base("TwitterUserDataRequest", GetUserDataRequestLink(), HttpOperation.Get)
        {
        }

        /// <summary>
        /// Gets the user data request link.
        /// </summary>
        /// <returns></returns>
        private static string GetUserDataRequestLink()
        {
            return (ApplicationData.TwitterAccount as TwitterAccount).RequestUserDataLink;
        }

        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            TwitterUserDataResponse response = new TwitterUserDataResponse(responseString);
            return response;
        }
    }
}
