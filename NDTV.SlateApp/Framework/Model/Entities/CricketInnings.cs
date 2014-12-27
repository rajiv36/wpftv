using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
namespace NDTV.Entities
{
    /// <summary>
    /// Class holds details of innings
    /// </summary>
    public class CricketInnings
    {
        /// <summary>
        /// Batsman Entity
        /// </summary>
        private CricketBatsman batsman;

        /// <summary>
        /// Cricket fixture entity
        /// </summary>
        private CricketFixtures fixture;

        /// <summary>
        /// Batsman list
        /// </summary>
        private List<CricketBatsman> batsmanList;

        /// <summary>
        /// Bowler entity
        /// </summary>
        private CricketBowler bowler;

        /// <summary>
        /// List of wickets in order
        /// </summary>
        private SortedList<int, FallOfWickets> listOfWickets;

        /// <summary>
        /// used to notify wether the inning is active
        /// </summary>
        public bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor which takes XElement and innings as parameter
        /// </summary>
        /// <param name="element">XElement - response which needs to be parsed</param>
        /// <param name="inningsElementName">innigs specifying whether it is first,second,third or fourth</param>
        public CricketInnings(XElement element, string inningsElementName, CricketFixtures fixture)
        {
            this.fixture = fixture;
            Parse(element, inningsElementName);
        }

        /// <summary>
        /// Name of Batting team
        /// </summary>
        public string BattingTeam
        {
            get;
            set;
        }

        /// <summary>
        /// Name of Bowling Team
        /// </summary>
        public string BowlingTeam
        {
            get;
            set;
        }

        /// <summary>
        /// Target
        /// </summary>
        public string Target
        {
            get;
            set;
        }
                
        /// <summary>
        /// List of Batsmen
        /// </summary>
        public ObservableCollection<CricketBatsman> BatsmanList
        {
            get
            {
                return new ObservableCollection<CricketBatsman>(batsmanList);
            }
        }
                
        /// <summary>
        /// List of bowlers
        /// </summary>
        public List<CricketBowler> BowlerList
        {
            get;
            set;
        }

        /// <summary>
        /// Runs scored through extra's
        /// </summary>
        public InningsExtra ExtraRunsGiven
        {
            get;
            set;
        }

        /// <summary>
        /// Equation of the innings
        /// </summary>
        public CricketInningsEquation Equation
        {
            get;
            set;
        }

        /// <summary>
        /// Fall of wickets
        /// </summary>
        public FallOfWickets FallOfWicket
        {
            get;
            set;
        }
            
        /// <summary>
        /// List of wickets
        /// </summary>
        public SortedList<int, FallOfWickets> ListOfWickets
        {
            get { return listOfWickets; }
            private set { listOfWickets = value; }
        }

        /// <summary>
        /// Extra Runs Given in formatted string
        /// </summary>
        public string ExtrasGiven
        {
            get
            {
                return ExtraRunsGiven.ToString();
            }
        }

        /// <summary>
        /// Fall of wickets in formatted string
        /// </summary>
        public string FallOfWicketString
        {
            get
            {
                return FallOfWicket.ToString();
            }
        }

        /// <summary>
        /// Flag of the batting team
        /// </summary>
        public string BattingTeamFlag
        {
            get;
            set;
        }

        /// <summary>
        /// Flag of the bowling team
        /// </summary>
        public string BowlingTeamFlag
        {
            get;
            set;
        }

        /// <summary>
        /// Batting Team short name
        /// </summary>
        public string BattingTeamShortName
        {
            get
            {
                if (null != fixture)
                {
                    return (BattingTeam.Equals(fixture.TeamA) ? fixture.TeamAShortName : fixture.TeamBShortName);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Bowling team short name
        /// </summary>
        public string BowlingTeamShortName
        {
            get
            {
                if (null != fixture)
                {
                    return (BowlingTeam.Equals(fixture.TeamA) ? fixture.TeamAShortName : fixture.TeamBShortName);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Innings number of the match
        /// </summary>
        public string InningsNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Parse the response and fill the appropriate model
        /// </summary>
        /// <param name="element">XElement</param>
        /// <param name="inningsElementName">string</param>
        private void Parse(XElement element, string inningsElementName)
        {
            if (null != element && false == string.IsNullOrWhiteSpace(inningsElementName) && null != element.Attributes())
            {
                int result;
                BattingTeam = null != element.Attribute("Battingteam") ? element.Attribute("Battingteam").Value.ToString() : string.Empty;
                BowlingTeam = null != element.Attribute("Bowlingteam") ? element.Attribute("Bowlingteam").Value.ToString() : string.Empty;
                Target = (null != element.Attribute("Target")) ? element.Attribute("Target").Value.ToString() : string.Empty;
                BattingTeamFlag = BattingTeam.Equals(fixture.TeamA) ? fixture.TeamAFlag : fixture.TeamBFlag;
                BowlingTeamFlag = BowlingTeam.Equals(fixture.TeamA) ? fixture.TeamAFlag : fixture.TeamBFlag;
                if (null != element.Element(inningsElementName + "Batsmen"))
                {
                    batsman = new CricketBatsman(element.Element(inningsElementName + "Batsmen"), inningsElementName);
                    batsmanList = batsman.BatsmenList;
                }
                if (null != element.Element(inningsElementName + "Bowlers"))
                {
                    bowler = new CricketBowler(element.Element(inningsElementName + "Bowlers"), inningsElementName);
                    BowlerList = bowler.BowlerList;
                }
                if (null != element.Element(inningsElementName + "Extras").Attributes())
                {
                    ExtraRunsGiven = new InningsExtra()
                    {
                        Byes = (null != element.Element(inningsElementName + "Extras").Attribute("Byes")) ? (int.TryParse(element.Element(inningsElementName + "Extras").Attribute("Byes").Value, out result) ? result : 0) : 0,
                        Wides = (null != element.Element(inningsElementName + "Extras").Attribute("TotalWides")) ? (int.TryParse(element.Element(inningsElementName + "Extras").Attribute("TotalWides").Value, out result) ? result : 0) : 0,
                        NoBalls = (null != element.Element(inningsElementName + "Extras").Attribute("TotalNoballs")) ? (int.TryParse(element.Element(inningsElementName + "Extras").Attribute("TotalNoballs").Value, out result) ? result : 0) : 0,
                        LegByes = (null != element.Element(inningsElementName + "Extras").Attribute("Legbyes")) ? (int.TryParse(element.Element(inningsElementName + "Extras").Attribute("Legbyes").Value, out result) ? result : 0) : 0,
                        Penalty = (null != element.Element(inningsElementName + "Extras").Attribute("Penalty")) ? (int.TryParse(element.Element(inningsElementName + "Extras").Attribute("Penalty").Value, out result) ? result : 0) : 0
                    };
                }
                if (null != element.Element(inningsElementName + "Equation") && null != element.Element(inningsElementName + "Equation").Attributes())
                {
                    Equation = new CricketInningsEquation()
                    {
                        TotalScore = int.TryParse(element.Element(inningsElementName + "Equation").Attribute("Total").Value, out result) ? result : 0,
                        TotalOvers = element.Element(inningsElementName + "Equation").Attribute("Overs").Value.ToString(),
                        RunRate = element.Element(inningsElementName + "Equation").Attribute("Runrate").Value.ToString(),
                        TotalWickets = int.TryParse(element.Element(inningsElementName + "Equation").Attribute("Wickets").Value, out result) ? result : 0,
                        BowlersUsed = int.TryParse(element.Element(inningsElementName + "Equation").Attribute("Bowlersused").Value, out result) ? result : 0,
                    };
                }
                if (null != element.Element(inningsElementName + "FallofWickets") && null != element.Element(inningsElementName + "FallofWickets").Attributes())
                {
                    FallOfWicket = new FallOfWickets(element.Element(inningsElementName + "FallofWickets"));
                    ListOfWickets = FallOfWicket.ListOfWickets;
                }
            }
        }
    }
}
