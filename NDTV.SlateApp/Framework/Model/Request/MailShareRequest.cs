using System.Globalization;
using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Class which contains the mail share request
    /// </summary>
    public class MailShareRequest : Request
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fromEmailAddress">From email address</param>
        /// <param name="toEmailAddress">To Email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="mailContent">Content within the email</param>
        public MailShareRequest(string fromEmailAddress, string toEmailAddress, string subject, string mailContent)
            : base("MailShareRequest", BuildRequestLink(fromEmailAddress,toEmailAddress,subject,mailContent),HttpOperation.Get)
        {            
        }

        /// <summary>
        /// Overridden method to build response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            MailShareResponse response = new MailShareResponse(responseString);
            return response;
        }

        /// <summary>
        /// Static method to build the link
        /// </summary>
        /// <param name="fromEmailAddress">From email address</param>
        /// <param name="toEmailAddress">To Email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="mailContent">Content within the email</param>
        /// <returns>Fully formed URL</returns>
        private static string BuildRequestLink(string fromEmailAddress, string toEmailAddress, string subject, string mailContent)
        {
            string requestLink = string.Empty;
            requestLink = string.Format(CultureInfo.InvariantCulture, Utility.GetLink(Constants.LinkNames.MailShareLink), toEmailAddress, fromEmailAddress, mailContent, subject, CultureInfo.InvariantCulture);
            return requestLink;
        }
    }
}
