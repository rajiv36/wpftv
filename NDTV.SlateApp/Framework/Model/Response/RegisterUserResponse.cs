
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class RegisterUserResponse : Response
    {
        /// <summary>
        /// NDTV Slate user instance.
        /// </summary>
        public NDTVSlateUser UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseValue">Response string.</param>
        public RegisterUserResponse(string responseValue)
            : base(responseValue)
        {
            this.Parse();
        }

        /// <summary>
        /// Parses the response data.
        /// </summary>
        private void Parse()
        {
            NDTVSlateUser user = Utility.Deserialize<NDTVSlateUser>(responseMessage);
            this.UserData = user;
        }
    }
}
