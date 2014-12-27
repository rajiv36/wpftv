
namespace NDTV.Entities
{
    public class LinkShortenRequest : Request
    {
        public LinkShortenRequest(string shortenUrl)
            : base("LinkShortenRequest", shortenUrl, HttpOperation.Get)
        {
        }

        protected override Response BuildResponseObject(string responseString)
        {
            LinkShortenResponse response = new LinkShortenResponse(responseString);
            return response;
        }
    }
}
