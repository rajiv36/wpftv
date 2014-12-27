
using NDTV.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NDTV.Entities
{
    public class GetCommentsResponse : Response
    {
        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        public Comments Comments
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseValue">Response string.</param>
        public GetCommentsResponse(string responseValue)
            : base(responseValue)
        {
            this.Parse();
        }

        /// <summary>
        /// Parses the response stream.
        /// </summary>
        private void Parse()
        {            
            if (!string.IsNullOrEmpty(responseMessage))
            {
                if (string.Equals(responseMessage, "[]"))
                {
                    this.Comments = new Comments();
                }
                else
                {
                    this.Comments = Utility.Deserialize<Comments>(responseMessage);                   
                }
            }
        }
    }
}
