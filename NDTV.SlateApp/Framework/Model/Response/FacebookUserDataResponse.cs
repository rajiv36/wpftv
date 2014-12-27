
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class FacebookUserDataResponse : Response
    {
        /// <summary>
        /// Gets the facebook user.
        /// </summary>
        private FacebookUser facebookUser;
        public FacebookUser FacebookUser
        {
            get
            {
                return this.facebookUser;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseMessage">Response message</param>
        public FacebookUserDataResponse(string responseMessage)
            : base(responseMessage)
        {
            Parse();
        }

        /// <summary>
        /// Parses the response message data.
        /// </summary>
        private void Parse()
        {
           this.facebookUser = Utility.Deserialize<FacebookUser>(responseMessage);
        }
    }
}
