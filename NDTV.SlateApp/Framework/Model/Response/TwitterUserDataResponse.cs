
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class TwitterUserDataResponse : Response
    {
        /// <summary>
        /// Gets the twitter user object.
        /// </summary>
        private TwitterUser twitterUser;
        public TwitterUser TwitterUser
        {
            get
            {
                return this.twitterUser;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseValue">Response string</param>
        public TwitterUserDataResponse(string responseValue)
            : base(responseValue)
        {
            Parse();
        }

        /// <summary>
        /// Parses the response to obtain data.
        /// </summary>
        private void Parse()
        {
            TwitterUser user = Utility.Deserialize<TwitterUser>(responseMessage);
            this.twitterUser = user;
        }
    }
}
