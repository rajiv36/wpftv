
using NDTV.SlateApp.Properties;
namespace NDTV.Entities
{
    public class PostCommentResponse : Response
    {
        /// <summary>
        /// Gets or sets the post message.
        /// </summary>
        public string PostMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseValue">Response string.</param>
        public PostCommentResponse(string responseValue)
            : base(responseValue)
        {
            this.Parse();
        }

        /// <summary>
        /// Parses the response message.
        /// </summary>
        private void Parse()
        {
            if (responseMessage.Contains("success"))
            {
                this.PostMessage = Resources.SuccessfulPostMessage;
            }
            else if (responseMessage.Contains("You have already posted this comment"))
            {
                this.PostMessage = Resources.CommentsPostRepeatError;
            }
            else
            {
                this.PostMessage = Resources.CommentsPostError;
            }
        }
    }
}
