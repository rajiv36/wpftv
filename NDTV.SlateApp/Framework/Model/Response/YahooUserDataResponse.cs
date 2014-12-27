
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class YahooUserDataResponse : Response
    {
        /// <summary>
        /// Gets the yahoo user.
        /// </summary>
        public YahooUser YahooUser
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseString">Response string.</param>
        public YahooUserDataResponse(string responseString)
            : base(responseString)
        {
            this.Parse();
        }

        /// <summary>
        /// Parses the response string.
        /// </summary>
        private void Parse()
        {
            this.YahooUser = Utility.Deserialize<YahooUser>(responseMessage);            
        }
    }
}
