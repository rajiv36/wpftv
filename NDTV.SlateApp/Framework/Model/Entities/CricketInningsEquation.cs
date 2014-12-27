
namespace NDTV.Entities
{
    /// <summary>
    /// Class holds the equation of the innings
    /// </summary>
    public class CricketInningsEquation
    {
        /// <summary>
        /// Total Score of the innings
        /// </summary>
        public int TotalScore
        {
            get;
            set;
        }

        /// <summary>
        /// Total wickets 
        /// </summary>
        public int TotalWickets
        {
            get;
            set;
        }

        /// <summary>
        /// Total overs bowled in the innings
        /// </summary>
        public string TotalOvers
        {
            get;
            set;
        }

        /// <summary>
        /// Number of bowlers used
        /// </summary>
        public int BowlersUsed
        {
            get;
            set;
        }

        /// <summary>
        /// Run rate of the innings
        /// </summary>
        public string RunRate
        {
            get;
            set;
        }
    }
}
