
using NDTV.Utilities;
namespace NDTV.Entities
{
    public class LinkShortenResponse : Response
    {
        public string bitLYLink;
        public string BitLYLink
        {
            get
            {
                return this.bitLYLink;
            }
        }

        public LinkShortenResponse(string responseString) :
            base(responseString)
        {
            BitLyData linkData = Utility.Deserialize<BitLyData>(responseString);
            if (linkData != null && linkData.LinkDetail != null && !string.IsNullOrEmpty(linkData.LinkDetail.ShortenedLink))
            {
                this.bitLYLink = linkData.LinkDetail.ShortenedLink;
            }
        }
    }
}
