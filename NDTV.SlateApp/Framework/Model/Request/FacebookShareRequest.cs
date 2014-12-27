
namespace NDTV.Entities
{
    public class FacebookShareRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="shareLink">Facebook share link</param>
        /// <param name="postData">Post data</param>
        public FacebookShareRequest(string shareLink, string postData) :
            base("FacebookShareRequest", shareLink, HttpOperation.Post)
        {
            this.rawRequest = postData;
        }

        /// <summary>
        /// Function that builds the response object.
        /// </summary>
        /// <param name="responseString">Response String</param>
        /// <returns>Response</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            FacebookShareResponse response = new FacebookShareResponse(responseString);
            return response;
        }
    }
}
