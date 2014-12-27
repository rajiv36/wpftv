
namespace NDTV.Entities
{
    public class TwitterShareRequest : Request
    {
        public TwitterShareRequest(string postData, string header)
            : base("TwitterShareRequest", Constants.TwitterConstants.ShareUrl, HttpOperation.Post, header, string.Empty, string.Empty, HttpAuthentication.None)
        {
            this.rawRequest = postData;
        }

        protected override Response BuildResponseObject(string responseString)
        {
            TwitterShareResponse response = new TwitterShareResponse(responseString);
            return response;
        }
    }
}
