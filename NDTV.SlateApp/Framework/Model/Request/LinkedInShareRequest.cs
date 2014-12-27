
namespace NDTV.Entities
{
    public class LinkedInShareRequest : Request
    {
        public LinkedInShareRequest(string postData, string header)
            : base("LinkedInShareRequest", Constants.LinkedInConstants.ShareUrl, HttpOperation.Post, header, string.Empty, string.Empty, HttpAuthentication.None)
        {
            this.rawRequest = postData;
            this.requestContentType = Constants.HttpRequestContentType.TextXml;
        }

        protected override Response BuildResponseObject(string responseString)
        {
            LinkedInShareResponse response = new LinkedInShareResponse(responseString);
            return response;
        }
    }
}
