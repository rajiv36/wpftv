using System.Globalization;

namespace NDTV.Entities
{
    public class SearchRequest : Request
    {
        /// <summary>
        /// Variable to store the search type(Images,videos or news articles).
        /// </summary>
        private SearchType searchType;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="searchLink">Search URL</param>
        /// <param name="type">Search Type</param>
        public SearchRequest(string searchLink,string searchKeyword,int pageNumber,SearchType type)
            : base("SearchRequest", GetCompleteSearchLink(searchLink, searchKeyword,pageNumber), HttpOperation.Get)
        {
            this.searchType = type;
        }

        /// <summary>
        /// Method to build the response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response Object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            SearchResponse response = new SearchResponse(responseString,searchType);
            return response;
        }

        private static string GetCompleteSearchLink(string searchLink,string searchKeyword,int pageNumber)
        {
            string completeLink = string.Empty;
            completeLink = string.Format(CultureInfo.InvariantCulture,searchLink, searchKeyword,pageNumber);
            return completeLink;
        }
    }
}
