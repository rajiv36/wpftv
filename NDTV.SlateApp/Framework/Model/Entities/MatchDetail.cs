using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
namespace NDTV.Entities
{
    /// <summary>
    /// Class holds details of the match
    /// </summary>
    public class MatchDetail
    {
        /// <summary>
        /// innings summary for tabs(radio button)
        /// </summary>
        private ObservableCollection<InningsSummary> inningsSummary;

        /// <summary>
        /// isCurrent variable. bool
        /// </summary>
        private bool isCurrent;

        /// <summary>
        /// Name of Home team
        /// </summary>
        public string HomeTeam
        {
            get;
            set;
        }

        /// <summary>
        /// Name of Visiting team
        /// </summary>
        public string AwayTeam
        {
            get;
            set;
        }

        /// <summary>
        /// Summary of event
        /// </summary>
        public string Event
        {
            get;
            set;
        }

        /// <summary>
        /// Status of match
        /// </summary>
        public string MatchStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Venue of the match
        /// </summary>
        public string MatchVenue
        {
            get;
            set;
        }

        /// <summary>
        /// Match number
        /// </summary>
        public string MatchNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Day of the match(live)
        /// </summary>
        public string MatchDay
        {
            get;
            set;
        }

        /// <summary>
        /// Date of the match
        /// </summary>
        public string MatchDate
        {
            get;
            set;
        }

        /// <summary>
        /// Current status of the match
        /// </summary>
        public string MatchCurrentStatus
        {
            get
            {
                if (MatchFixtures.MatchType.Equals("Test"))
                {
                    return (Constants.Constant.Day + Constants.Constant.Space + MatchDay + Constants.Constant.Colon + Constants.Constant.Space + MatchStatus);
                }
                else
                {
                    return ("Status" + Constants.Constant.Space + MatchDay + Constants.Constant.Colon + Constants.Constant.Space + MatchStatus);
                }
            }
        }

        /// <summary>
        /// Details of first innings
        /// </summary>
        public CricketInnings FirstInnings
        {
            get;
            set;
        }

        /// <summary>
        /// Details of second innings
        /// </summary>
        public CricketInnings SecondInnings
        {
            get;
            set;
        }

        /// <summary>
        /// Details of third innings
        /// </summary>
        public CricketInnings ThirdInnings
        {
            get;
            set;
        }

        /// <summary>
        /// Details of fourth innings
        /// </summary>
        public CricketInnings FourthInnings
        {
            get;
            set;
        }

        /// <summary>
        /// Match fixtures
        /// </summary>
        public CricketFixtures MatchFixtures
        {
            get;
            set;
        }

        /// <summary>
        /// Match result
        /// </summary>
        public string MatchResult
        {
            get;
            set;
        }

        /// <summary>
        /// Title of the Match
        /// </summary>
        public string MatchTitle
        {
            get
            {
                return MatchFixtures.TeamAShortName + Constants.Constant.Vs + MatchFixtures.TeamBShortName + Constants.Constant.CommaWithSpace + MatchFixtures.MatchNumber + Constants.Constant.CommaWithSpace + MatchFixtures.Venue;
            }
        }

        /// <summary>
        /// Sub Title contains date.
        /// </summary>
        public string MatchSubtitle
        {
            get
            {
                return MatchFixtures.MatchDate.ToString(Constants.Constant.DateFormat,CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// List contains definition for radio buttons
        /// </summary>
        public ObservableCollection<InningsSummary> InningsSummaryList
        {
            get
            {
                return FillInningsList();
            }
        }

        /// <summary>
        /// Retrieve the current innings
        /// </summary>
        /// <returns>CricketInnings -currrent innings</returns>
        public CricketInnings RetrieveCurrentInnings()
        {
            if (null != FourthInnings)
            {
                isCurrent = IsCurrent(FourthInnings);
                if (isCurrent)
                {
                    return FourthInnings;
                }
                else
                {
                    if (MatchFixtures.IsLive)
                    {
                        return CheckCurrentInningsIfMatchIsInSuspendedMode();
                    }
                }
            }
            else if (null != ThirdInnings)
            {
                isCurrent = IsCurrent(ThirdInnings);
                if (isCurrent)
                {
                    return ThirdInnings;
                }
                else
                {
                    if (MatchFixtures.IsLive)
                    {
                        return CheckCurrentInningsIfMatchIsInSuspendedMode();
                    }
                }
            }
            else if (null != SecondInnings)
            {
                isCurrent = IsCurrent(SecondInnings);
                if (isCurrent)
                {
                    return SecondInnings;
                }
                else
                {
                    if (MatchFixtures.IsLive)
                    {
                        return CheckCurrentInningsIfMatchIsInSuspendedMode();
                    }
                }
            }
            else if (null != FirstInnings)
            {
                isCurrent = IsCurrent(FirstInnings);
                if (isCurrent)
                {
                    return FirstInnings;
                }
                else
                {
                    if (MatchFixtures.IsLive)
                    {
                        return CheckCurrentInningsIfMatchIsInSuspendedMode();
                    }
                }
            }
            
            return null;
        }

        /// <summary>
        /// Suppose all batsman are out and match status is innings break, decide which innings is live
        /// Lst played innings will be live
        /// </summary>
        /// <returns>CricketInnings</returns>
        private CricketInnings CheckCurrentInningsIfMatchIsInSuspendedMode()
        {
            if (null != FourthInnings)
            {
                return FourthInnings;
            }
            else if (null != ThirdInnings)
            {
                return ThirdInnings;
            }
            else if (null != SecondInnings)
            {
                return SecondInnings;
            }
            else if (null != FirstInnings)
            {
                return FirstInnings;
            }
            return null;
        }

        /// <summary>
        /// Get the current innings of the live match
        /// </summary>
        public CricketInnings CurrentInnings
        {
            get
            {
                return RetrieveCurrentInnings();
            }
        }


        /// <summary>
        /// Check if innings is current
        /// </summary>
        /// <param name="cricketInnings">CricketInnings</param>
        /// <returns>bool -whether innings is current</returns>
        private static bool IsCurrent(CricketInnings cricketInnings)
        {
            if (null != cricketInnings)
            {
                var batsman = from innings in cricketInnings.BatsmanList
                              where innings.HowGotDismissed.Equals(Constants.Constant.Batting)
                              select innings;
                if (batsman.Count() != 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Fill innings list for innings radio button list
        /// </summary>
        /// <returns>Collection of all the innings in the match</returns>
        private ObservableCollection<InningsSummary> FillInningsList()
        {
            if (null == inningsSummary)
            {
                inningsSummary = new ObservableCollection<InningsSummary>();
            }
            else
            {
                inningsSummary.Clear();
            }
            InningsSummary summary;
            if (null != FirstInnings && false == string.IsNullOrEmpty(FirstInnings.BattingTeam))
            {
                summary = new InningsSummary();
                summary.TeamShortName = FirstInnings.BattingTeam.Equals(MatchFixtures.TeamA) ? MatchFixtures.TeamAShortName : MatchFixtures.TeamBShortName;
                summary.TeamFlag = FirstInnings.BattingTeam.Equals(MatchFixtures.TeamA) ? MatchFixtures.TeamAFlag : MatchFixtures.TeamBFlag;
                summary.BowlingTeamFlag = FirstInnings.BowlingTeam.Equals(MatchFixtures.TeamA) ? MatchFixtures.TeamAFlag : MatchFixtures.TeamBFlag;
                summary.InningsType = (MatchFixtures.MatchType.Equals(Constants.Constant.TestInnings)) ? Constants.Constant.FirstInn : string.Empty;
                summary.InningsName = FirstInnings.InningsNumber;
                if (false == string.IsNullOrEmpty(summary.TeamShortName))
                {
                    summary.DisplayName = summary.TeamShortName + Constants.Constant.Space + summary.InningsType;
                }
                summary.IsActive = (null != CurrentInnings) ? (CurrentInnings.Equals(FirstInnings) ? true : false) : false;
                if (false == MatchFixtures.IsLive)
                {
                    summary.IsActive = true;
                }
                inningsSummary.Add(summary);
            }

            if (null != SecondInnings && false == string.IsNullOrEmpty(SecondInnings.BattingTeam))
            {
                summary = new InningsSummary();
                summary.TeamShortName = SecondInnings.BattingTeam.Equals(MatchFixtures.TeamA) ? MatchFixtures.TeamAShortName : MatchFixtures.TeamBShortName;
                summary.TeamFlag = SecondInnings.BattingTeam.Equals(MatchFixtures.TeamA) ? MatchFixtures.TeamAFlag : MatchFixtures.TeamBFlag;
                summary.BowlingTeamFlag = FirstInnings.BowlingTeam.Equals(MatchFixtures.TeamA) ? MatchFixtures.TeamAFlag : MatchFixtures.TeamBFlag;
                summary.InningsType = (MatchFixtures.MatchType.Equals(Constants.Constant.TestInnings)) ? Constants.Constant.FirstInn : string.Empty;
                summary.InningsName = SecondInnings.InningsNumber;
                if (false == string.IsNullOrEmpty(summary.TeamShortName))
                {
                    summary.DisplayName = summary.TeamShortName + Constants.Constant.Space + summary.InningsType;
                }
                summary.IsActive = (null != CurrentInnings) ? (CurrentInnings.Equals(SecondInnings) ? true : false) : false;
                inningsSummary.Add(summary);
            }

            if (null != ThirdInnings && false == string.IsNullOrEmpty(ThirdInnings.BattingTeam))
            {
                summary = new InningsSummary();
                summary.TeamShortName = ThirdInnings.BattingTeam.Equals(MatchFixtures.TeamA) ? MatchFixtures.TeamAShortName : MatchFixtures.TeamBShortName;
                summary.TeamFlag = ThirdInnings.BattingTeam.Equals(MatchFixtures.TeamA) ? MatchFixtures.TeamAFlag : MatchFixtures.TeamBFlag;
                summary.BowlingTeamFlag = FirstInnings.BowlingTeam.Equals(MatchFixtures.TeamA) ? MatchFixtures.TeamAFlag : MatchFixtures.TeamBFlag;
                summary.InningsType = (MatchFixtures.MatchType.Equals(Constants.Constant.TestInnings)) ? Constants.Constant.SecondInn : string.Empty;
                summary.InningsName = ThirdInnings.InningsNumber;
                if (false == string.IsNullOrEmpty(summary.TeamShortName))
                {
                    summary.DisplayName = summary.TeamShortName + Constants.Constant.Space + summary.InningsType;
                }
                summary.IsActive = (null != CurrentInnings) ? (CurrentInnings.Equals(ThirdInnings) ? true : false) : false;
                inningsSummary.Add(summary);
            }

            if (null != FourthInnings && false == string.IsNullOrEmpty(FourthInnings.BattingTeam))
            {
                summary = new InningsSummary();
                summary.TeamShortName = FourthInnings.BattingTeam.Equals(MatchFixtures.TeamA) ? MatchFixtures.TeamAShortName : MatchFixtures.TeamBShortName;
                summary.TeamFlag = FourthInnings.BattingTeam.Equals(MatchFixtures.TeamA) ? MatchFixtures.TeamAFlag : MatchFixtures.TeamBFlag;
                summary.BowlingTeamFlag = FirstInnings.BowlingTeam.Equals(MatchFixtures.TeamA) ? MatchFixtures.TeamAFlag : MatchFixtures.TeamBFlag;
                summary.InningsType = (MatchFixtures.MatchType.Equals(Constants.Constant.TestInnings)) ? Constants.Constant.SecondInn : string.Empty;
                summary.InningsName = FourthInnings.InningsNumber;
                if (false == string.IsNullOrEmpty(summary.TeamShortName))
                {
                    summary.DisplayName = summary.TeamShortName + Constants.Constant.Space + summary.InningsType;
                }
                summary.IsActive = (null != CurrentInnings) ? (CurrentInnings.Equals(FourthInnings) ? true : false) : false;
                inningsSummary.Add(summary);
            }
            return inningsSummary;
        }
    }
}
