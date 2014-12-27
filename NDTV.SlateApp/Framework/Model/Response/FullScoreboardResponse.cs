using System.Xml.Linq;
using System.Linq;


namespace NDTV.Entities
{
    /// <summary>
    /// Class to fill Model for Full scorecard screen
    /// </summary>
    public class FullScoreboardResponse : Response
    {
        /// <summary>
        /// Cricket fixture
        /// </summary>
        private CricketFixtures fixture;

        /// <summary>
        /// Property to get or set match details.
        /// </summary>
        public MatchDetail DetailedMatchStatistics
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response message to be parsed</param>
        public FullScoreboardResponse(string responseMessage, CricketFixtures cricketFixture)
            : base(responseMessage)
        {
            this.fixture = cricketFixture;
            Parse();
        }

        /// <summary>
        /// Parse the response
        /// </summary>
        private void Parse()
        {
            XElement element = XElement.Parse(responseMessage);
            if (null != element && null != element.Attributes())
            {
                DetailedMatchStatistics = new MatchDetail()
                {
                    Event = (null != element.Attribute("Event")) ? element.Attribute("Event").Value.ToString() : string.Empty,
                    HomeTeam = (null != element.Attribute("Hometeam")) ? element.Attribute("Hometeam").Value.ToString() : string.Empty,
                    AwayTeam = (null != element.Attribute("Awayteam")) ? element.Attribute("Awayteam").Value.ToString() : string.Empty,
                    MatchNumber = (null != element.Attribute("MatchNo")) ? element.Attribute("MatchNo").Value.ToString() : string.Empty,
                    MatchDate = (null != element.Attribute("matchdate")) ? element.Attribute("matchdate").Value.ToString() : string.Empty,
                    MatchVenue = (null != element.Attribute("Venue")) ? element.Attribute("Venue").Value.ToString() : string.Empty,
                    MatchStatus = (null != element.Attribute("Status")) ? element.Attribute("Status").Value.ToString() : string.Empty,
                    MatchDay = (null != element.Attribute("Matchday")) ? element.Attribute("Matchday").Value.ToString() : string.Empty,
                    MatchResult = (null != element.Attribute("MatchResult")) ? element.Attribute("MatchResult").Value.ToString() : string.Empty,
                };

                if (null != element.Element("FirstInnings") && null != element.Element("FirstInnings").Elements() && null != element.Element("FirstInnings").Attributes() && element.Element("FirstInnings").Attributes().Count()>0)
                {
                    DetailedMatchStatistics.FirstInnings = new CricketInnings(element.Element("FirstInnings"), "FI",fixture);
                    DetailedMatchStatistics.FirstInnings.InningsNumber = Constants.Constant.FirstInnings;
                    if (null != element.Element("SecondInnings") && null != element.Element("SecondInnings").Elements() && null != element.Element("SecondInnings").Attributes() && element.Element("SecondInnings").Attributes().Count() > 0)
                    {
                        DetailedMatchStatistics.SecondInnings = new CricketInnings(element.Element("SecondInnings"), "SI",fixture);
                        DetailedMatchStatistics.SecondInnings.InningsNumber = Constants.Constant.SecondInnings; 
                        if (null != element.Element("ThirdInnings") && null != element.Element("ThirdInnings").Elements() && null != element.Element("ThirdInnings").Attributes() && element.Element("ThirdInnings").Attributes().Count() > 0)
                        {
                            DetailedMatchStatistics.ThirdInnings = new CricketInnings(element.Element("ThirdInnings"), "TI",fixture);
                            DetailedMatchStatistics.ThirdInnings.InningsNumber = Constants.Constant.ThirdInnings;
                            if (null != element.Element("FourthInnings") && null != element.Element("FourthInnings").Elements() && null != element.Element("FourthInnings").Attributes() && element.Element("FourthInnings").Attributes().Count() > 0)
                            {
                                DetailedMatchStatistics.FourthInnings = new CricketInnings(element.Element("FourthInnings"), "FOI",fixture);
                                DetailedMatchStatistics.FourthInnings.InningsNumber = Constants.Constant.FourthInnings;
                            }
                        }
                    }
                }

                DetailedMatchStatistics.MatchFixtures = fixture;
            }
        }

    }
}
