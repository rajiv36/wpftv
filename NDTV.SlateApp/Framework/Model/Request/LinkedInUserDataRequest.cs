
namespace NDTV.Entities
{
    public class LinkedInUserDataRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LinkedInUserDataRequest(string requestLink) :
            base("LinkedInUserDataRequest", requestLink, HttpOperation.Get)
        {
        }

        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            LinkedInUserDataResponse response = new LinkedInUserDataResponse(responseString);
            return response;
        }
    }
}
