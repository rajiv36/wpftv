
namespace NDTV.Entities
{
    public class GetCommentsRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="commentLink">Comments link</param>
        public GetCommentsRequest(string commentLink)
            : base("GetCommentsRequest", commentLink, HttpOperation.Post)
        {
        }

        /// <summary>
        /// Overridden method to build response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            GetCommentsResponse response = new GetCommentsResponse(responseString);
            return response;
        }
    }
}
