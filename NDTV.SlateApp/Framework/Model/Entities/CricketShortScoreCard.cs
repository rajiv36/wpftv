using System.Runtime.Serialization;

namespace NDTV.Entities
{
    /// <summary>
    /// Cricket Short Score Card.
    /// </summary>
    [DataContract]
    public class CricketShortScorecard
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "match")]
        public string Match { get; set; }

        [DataMember(Name = "venue")]
        public string Venue { get; set; }

        [DataMember(Name = "type")]
        public string MatchType { get; set; }

        [DataMember(Name = "date")]
        public string Date { get; set; }

        [DataMember(Name = "time")]
        public string Time { get; set; }

        [DataMember(Name = "islive")]
        public string IsLive { get; set; }

        [DataMember(Name = "currentscore")]
        public string CurrentScore { get; set; }

        [DataMember(Name = "currentover")]
        public string CurrentOver { get; set; }

        [DataMember(Name = "current_batting_team")]
        public string CurrentBattingTeam { get; set; }

        [DataMember(Name = "matchresult")]
        public string MatchResult { get; set; }

        [DataMember(Name = "extrainfo")]
        public string ExtraInfo { get; set; }

        [DataMember(Name = "link")]
        public string Link { get; set; }

        public string ShortenedTeamName
        {
            get
            {
                return Utilities.Utility.GetTeamName(CurrentBattingTeam);
            }
        }

    }
}
