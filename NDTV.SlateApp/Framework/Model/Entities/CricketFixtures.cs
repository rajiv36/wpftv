using System;
using System.Globalization;
using System.IO;

namespace NDTV.Entities
{
    /// <summary>
    /// Model to store values for Recent and upcoming matches.
    /// </summary>
    public class CricketFixtures
    {
        /// <summary>
        /// Whether match is daylight
        /// </summary>
        public bool IsDayNight
        {
            get;
            set;
        }

        /// <summary>
        /// Whether match is Live
        /// </summary>
        public bool IsLive
        {
            get;
            set;
        }

        /// <summary>
        /// Date of the match in IST
        /// </summary>
        public DateTime MatchDate
        {
            get;
            set;
        }

        /// <summary>
        /// Match Date in IST
        /// </summary>
        public string MatchDateIST
        {
            get
            {
                return MatchDate.ToString(Constants.Constant.DateFormat,CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// ID of the file to be used to populate scorecard
        /// </summary>
        public string MatchFile
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
        /// Match Type
        /// </summary>
        public string MatchType
        {
            get;
            set;
        }

        /// <summary>
        /// Result of the match
        /// </summary>
        public string MatchResult
        {
            get;
            set;
        }

        /// <summary>
        /// Name of team A
        /// </summary>
        public string TeamA
        {
            get;
            set;
        }

        /// <summary>
        /// Name of team B
        /// </summary>
        public string TeamB
        {
            get;
            set;
        }

        /// <summary>
        /// Short name of team A
        /// </summary>
        public string TeamAShortName
        {
            get;
            set;
        }

        /// <summary>
        /// Short name of team b
        /// </summary>
        public string TeamBShortName
        {
            get;
            set;
        }

        /// <summary>
        /// Whether match is up-coming
        /// </summary>
        public bool Upcoming
        {
            get;
            set;
        }

        /// <summary>
        /// Whether match is recent
        /// </summary>
        public bool Recent
        {
            get;
            set;
        }

        /// <summary>
        /// Status of the match
        /// </summary>
        public string MatchStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Venue of the match
        /// </summary>
        public string Venue
        {
            get;
            set;
        }

        /// <summary>
        /// Id of team A
        /// </summary>
        public string TeamAId
        {
            get;
            set;
        }

        /// <summary>
        /// Id of team B
        /// </summary>
        public string TeamBId
        {
            get;
            set;
        }

        /// <summary>
        /// Flag of Team A
        /// </summary>
        public string TeamAFlag
        {
            get 
            {
                string appImagesPath = Utilities.Utility.FetchImagePath();
                string flagLocation = Path.Combine(appImagesPath, TeamAId + Constants.Constant.GifExtension);
                return flagLocation;
            }
        }

        /// <summary>
        /// Flag of Team B
        /// </summary>
        public string TeamBFlag
        {
            get
            {
                string appImagesPath = Utilities.Utility.FetchImagePath();
                string flagLocation = Path.Combine(appImagesPath, TeamBId + Constants.Constant.GifExtension);
                return flagLocation;
            }
        }

        /// <summary>
        /// Match teams. 
        /// Like : India vs England
        /// </summary>
        public string MatchTeams
        {
            get
            {
                if (false == string.IsNullOrWhiteSpace(TeamA) && false == string.IsNullOrWhiteSpace(TeamB))
                {
                    return (TeamA + Constants.Constant.Vs + TeamB);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Match teams short name
        /// Like: Ind vs Eng
        /// </summary>
        public string MatchTeamShortNames
        {
            get
            {
                if (false == string.IsNullOrWhiteSpace(TeamAShortName) && false == string.IsNullOrWhiteSpace(TeamBShortName))
                {
                    return (TeamAShortName + Constants.Constant.Vs + TeamBShortName);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Indian Standard Time of the match
        /// </summary>
        public string MatchTimeIST
        {
            get;
            set;
        }

        /// <summary>
        /// Local match Time
        /// </summary>
        public string MatchTimeLocal
        {
            get;
            set;
        }

        /// <summary>
        /// Check current displayed match as selected
        /// </summary>
        public bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// Day night string format
        /// </summary>
        public string DayOrDayNight
        {
            get
            {
                if (false == string.IsNullOrWhiteSpace(MatchNumber))
                {
                    return (IsDayNight ? MatchNumber + Constants.Constant.Space + "(D/N)" : MatchNumber + Constants.Constant.Space + "(D)");
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Date time format for upcoming matches
        /// </summary>
        public string UpcomingDateTimeFormat
        {
            get
            {
                if (false == string.IsNullOrWhiteSpace(MatchTimeLocal) && false == string.IsNullOrWhiteSpace(MatchTimeIST))
                {
                    return (MatchTimeLocal.Equals(MatchTimeIST) ? MatchTimeLocal + Constants.Constant.Space + "(Local)" : (MatchTimeIST + Constants.Constant.Space + "(IST) |" + Constants.Constant.Space + MatchTimeLocal + Constants.Constant.Space + "(Local)"));
                }
                return string.Empty;
            }
        }
    }
}
