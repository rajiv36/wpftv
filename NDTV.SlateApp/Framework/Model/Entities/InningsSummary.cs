
namespace NDTV.Entities
{
    /// <summary>
    /// Summary of innings
    /// </summary>
    public class InningsSummary
    {
        /// <summary>
        /// used to notify whether innings is active
        /// </summary>
        private bool active;

        /// <summary>
        /// Check which innings is active
        /// </summary>
        public bool IsActive
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        /// <summary>
        /// Team Flag
        /// </summary>
        public string TeamFlag
        {
            get;
            set;
        }

        /// <summary>
        /// Flag of bowling team
        /// </summary>
        public string BowlingTeamFlag
        {
            get;
            set;
        }

        /// <summary>
        /// Innings Type
        /// </summary>
        public string InningsType
        {
            get;
            set;
        }

        /// <summary>
        /// Team Short Name
        /// </summary>
        public string TeamShortName
        {
            get;
            set;
        }

        /// <summary>
        /// Innings Name
        /// </summary>
        public string InningsName
        {
            get;
            set;
        }

        /// <summary>
        /// Display name such as ENG 1st Inn
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }
    }
}
