
namespace NDTV.Entities
{
    public class YahooTokenRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="link">Request link.</param>
        public YahooTokenRequest(string link)
            : base("YahooTokenRequest", link, HttpOperation.Get)
        {
        }

        /// <summary>
        /// Call back method of the Response 
        /// </summary>
        /// <param name="responseMessage">Response message</param>
        /// <returns> Yahoo token Response</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            YahooTokenResponse response = new YahooTokenResponse(responseString);
            return response;
        }
    }
}
