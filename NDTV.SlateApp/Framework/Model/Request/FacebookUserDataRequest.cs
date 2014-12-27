using NDTV.Controller;

namespace NDTV.Entities
{
    public class FacebookUserDataRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public FacebookUserDataRequest(string credentialLink) :
            base("FacebookUserDataResponse", credentialLink, HttpOperation.Get)
        {
        }       

        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            FacebookUserDataResponse response = new FacebookUserDataResponse(responseString);
            return response;
        }
    }
}
