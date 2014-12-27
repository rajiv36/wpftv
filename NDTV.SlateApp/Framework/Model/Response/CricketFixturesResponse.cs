using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Globalization;

namespace NDTV.Entities
{
    /// <summary>
    /// Response for cricket fixtures
    /// </summary>
    public class CricketFixturesResponse : Response
    {
        /// <summary>
        /// List contains all the match fixtures
        /// </summary>
        private ObservableCollection<CricketFixtures> matchFixturelList;

        /// <summary>
        /// Live match list
        /// </summary>
        private List<CricketFixtures> liveMatchList;

        /// <summary>
        /// upcoming match list
        /// </summary>
        private List<CricketFixtures> upcomingMatchList;

        /// <summary>
        /// recent match list
        /// </summary>
        private List<CricketFixtures> recentMatchList;

        /// <summary>
        /// List of live cricket matches
        /// </summary>
        public ObservableCollection<CricketFixtures> LiveMatchList
        {
            get
            {
                return new ObservableCollection<CricketFixtures>(liveMatchList);
            }
        }

        /// <summary>
        /// List of recent cricket matches
        /// </summary>
        public ObservableCollection<CricketFixtures> RecentMatchList
        {
            get { return new ObservableCollection<CricketFixtures>(recentMatchList); }
        }

        /// <summary>
        /// List of upcoming cricket matches
        /// </summary>
        public ObservableCollection<CricketFixtures> UpcomingMatchList
        {
            get { return new ObservableCollection<CricketFixtures>(upcomingMatchList); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response message to be parsed</param>
        public CricketFixturesResponse(string responseMessage)
            : base(responseMessage)
        {
            matchFixturelList = new ObservableCollection<CricketFixtures>();
            Parse();
        }

        /// <summary>
        /// Parse the response and fill the appropriate model
        /// </summary>
        private void Parse()
        {
            XElement element = XElement.Parse(responseMessage);
            if (null != element && null != element.Elements())
            {
                var fixtures = (from eachItem in element.Elements("match")
                                select new CricketFixtures()
                                {
                                    IsDayNight = (null != eachItem.Attribute("daynight")) ? (eachItem.Attribute("daynight").Value.Equals("yes") ? true : false) : false,
                                    IsLive = (null != eachItem.Attribute("live")) ? (eachItem.Attribute("live").Value.Equals("0") ? false : true) : false,
                                    MatchDate = (null != eachItem.Attribute("matchdate_ist")) ? Convert.ToDateTime(eachItem.Attribute("matchdate_ist").Value,CultureInfo.InvariantCulture) : DateTime.Now.Date,
                                    MatchFile = (null != eachItem.Attribute("matchfile")) ? eachItem.Attribute("matchfile").Value : string.Empty,
                                    MatchNumber = (null != eachItem.Attribute("matchnumber")) ? eachItem.Attribute("matchnumber").Value : string.Empty,
                                    MatchResult = (null != eachItem.Attribute("matchresult")) ? eachItem.Attribute("matchresult").Value : string.Empty,
                                    TeamA = (null != eachItem.Attribute("teama")) ? eachItem.Attribute("teama").Value : string.Empty,
                                    TeamAShortName = (null != eachItem.Attribute("teama_short")) ? eachItem.Attribute("teama_short").Value : string.Empty,
                                    TeamB = (null != eachItem.Attribute("teamb")) ? eachItem.Attribute("teamb").Value : string.Empty,
                                    TeamBShortName = (null != eachItem.Attribute("teamb_short")) ? eachItem.Attribute("teamb_short").Value : string.Empty,
                                    Upcoming = (null != eachItem.Attribute("upcoming")) ? (eachItem.Attribute("upcoming").Value.Equals("0") ? false : true) : false,
                                    Recent = (null != eachItem.Attribute("recent")) ? (eachItem.Attribute("recent").Value.Equals("no") ? false : true) : false,
                                    Venue = (null != eachItem.Attribute("venue")) ? eachItem.Attribute("venue").Value : string.Empty,
                                    TeamAId = (null != eachItem.Attribute("teama_Id")) ? eachItem.Attribute("teama_Id").Value : string.Empty,
                                    TeamBId = (null != eachItem.Attribute("teamb_Id")) ? eachItem.Attribute("teamb_Id").Value : string.Empty,
                                    MatchType = (null != eachItem.Attribute("matchtype")) ? eachItem.Attribute("matchtype").Value : string.Empty,
                                    MatchStatus = (null != eachItem.Attribute("matchstatus")) ? eachItem.Attribute("matchstatus").Value : string.Empty,
                                    MatchTimeIST = (null != eachItem.Attribute("matchtime_ist")) ? eachItem.Attribute("matchtime_ist").Value : string.Empty,
                                    MatchTimeLocal = (null != eachItem.Attribute("matchtime_local")) ? eachItem.Attribute("matchtime_local").Value : string.Empty,
                                }).ToList();
                if (null != fixtures && fixtures.Count > 0)
                {
                    for (int elementIndex = 0; elementIndex < fixtures.Count; elementIndex++)
                    {
                        var currentElement = fixtures[elementIndex];
                        matchFixturelList.Add(currentElement);
                    }
                }
                FilterCricketFixtures();
            }
        }

        /// <summary>
        /// Filter the list based on tab selection whether it is Live, recent or Upcoming
        /// </summary>
        private void FilterCricketFixtures()
        {
            if (null != matchFixturelList)
            {
                liveMatchList = (matchFixturelList.Any(status => status.IsLive.Equals(true))) ? matchFixturelList.Where(status => status.IsLive.Equals(true)).ToList() : new List<CricketFixtures>();
                recentMatchList = (matchFixturelList.Any(status => status.Recent.Equals(true))) ? matchFixturelList.Where(status => status.Recent.Equals(true)).ToList() : new List<CricketFixtures>();
                upcomingMatchList = (matchFixturelList.Any(status => status.Upcoming.Equals(true))) ? matchFixturelList.Where(status => status.Upcoming.Equals(true)).ToList() : new List<CricketFixtures>();
            }
        }
    }
}
