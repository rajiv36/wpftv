namespace NDTV.Entities
{
    /// <summary>
    /// Class contains the mail share response
    /// </summary>
    public class MailShareResponse : Response
    {
        /// <summary>
        /// Gets or sets whether the mail share is successful or not
        /// </summary>
        public bool Success
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response message obtained from the api</param>
        public MailShareResponse(string responseMessage)
            : base(responseMessage)
        {
            Success = responseMessage.Contains("Untitled Page");            
        }
    }
}
