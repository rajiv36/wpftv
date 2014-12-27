
namespace NDTV.Entities
{
    public class PostCommentRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="postData">Post data.</param>
        public PostCommentRequest(string postData)
            : base("PostCommentRequest", Constants.CommentConstants.PostCommentsUrl, HttpOperation.Post)
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
            PostCommentResponse response = new PostCommentResponse(responseString);
            return response;
        }
    }
}
