using System;
using System.Collections.Generic;

namespace NDTV.Entities
{
    public class CricketShortScorecardResponse :Response
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response message to be parsed</param>
        public CricketShortScorecardResponse(string responseMessage)
            : base(responseMessage)
        {
            scoreCards = new List<CricketShortScorecard>();
            JSONParse();
        }

        /// <summary>
        /// Parse the response and fill the appropriate model
        /// </summary>
        private void JSONParse()
        {
            if (!string.IsNullOrEmpty(responseMessage))
            {
                scoreCards = Utilities.Utility.Deserialize<List<CricketShortScorecard>>(responseMessage);
            }
        }

        /// <summary>
        /// Score Cards associated with all the happening cricket matches.
        /// </summary>
        private List<CricketShortScorecard> scoreCards;

        /// <summary>
        /// Score Cards associated with all the happening cricket matches.
        /// </summary>
        public List<CricketShortScorecard> Scorecards
        {
            get 
            {
                return scoreCards; 
            }
        }
    }
}
