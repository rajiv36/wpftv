
namespace NDTV.Entities
{
    public class RegisterUserRequest : Request
    {
        public RegisterUserRequest(string postData) :
            base("RegisterUserRequest", Constants.RegisterUserConstants.RegisterUserLink, HttpOperation.Post)
        {
            this.rawRequest = postData;
        }

        /// <summary>
        /// Overridden method to build response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            RegisterUserResponse response = new RegisterUserResponse(responseString);
            return response;
        }
    }
}
