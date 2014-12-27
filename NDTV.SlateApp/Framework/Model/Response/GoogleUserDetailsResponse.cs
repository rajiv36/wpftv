
using NDTV.Utilities;
namespace NDTV.Entities
{
    public class GoogleUserDetailsResponse : Response
    {
        /// <summary>
        /// Gets or sets the google user data.
        /// </summary>
        public GoogleUser User
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseString">Response string</param>
        public GoogleUserDetailsResponse(string responseString)
            : base(responseString)
        {
            this.Parse();
        }

        /// <summary>
        /// Parses the response data.
        /// </summary>
        private void Parse()
        {
            this.User = Utility.Deserialize<GoogleUser>(responseMessage);
        }
    }
}
