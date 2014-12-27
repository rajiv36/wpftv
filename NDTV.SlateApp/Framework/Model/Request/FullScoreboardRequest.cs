using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Request to get the full scoreboard
    /// </summary>
    public class FullScoreboardRequest : Request
    {
        private CricketFixtures fixture;
        /// <summary>
        /// Default constructor
        /// </summary>
        public FullScoreboardRequest(CricketFixtures cricketFixture)
            : base("FullScoreboardRequest", BuildScoreCardLink(cricketFixture.MatchFile), HttpOperation.Get)
        {
            fixture = cricketFixture;
            enableCache = false;
        }

        /// <summary>
        /// Append the URL with the file name
        /// </summary>
        /// <returns>API link to be called</returns>
        private static string BuildScoreCardLink(string matchFileName)
        {
            StringBuilder baseUrl = new StringBuilder(Utility.GetLink(Constants.LinkNames.CricketFullScorecard));
            if (false == string.IsNullOrWhiteSpace(matchFileName))
            {
                baseUrl.Append(matchFileName);
                baseUrl.Append(Constants.Constant.XmlExtension);
            }
            return baseUrl.ToString();
        }

        /// <summary>
        /// Overridden method to build response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            FullScoreboardResponse response = new FullScoreboardResponse(responseString, fixture);
            return response;
        }
    }
}
