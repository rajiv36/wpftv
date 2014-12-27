
using System.Text;
namespace NDTV.Entities
{
    /// <summary>
    /// Entity holds the details of commentary
    /// </summary>
    public class CricketCommentaryItem
    {
        /// <summary>
        /// Over number
        /// </summary>
        public string OverNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Event which happened on the ball
        /// </summary>
        public string BallEvent
        {
            get;
            set;
        }

        /// <summary>
        /// Bowler to batsman detail
        /// </summary>
        public string BowlerToBatsmanDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Actual Commentary
        /// </summary>
        public string ActualCommentary
        {
            get;
            set;
        }

        /// <summary>
        /// Whether it is a ball commentary or a non-ball
        /// </summary>
        public string BallStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Commentary to be shown to the user
        /// </summary>
        public string DisplayedCommentary
        {
            get
            {
                StringBuilder text = new StringBuilder();
                text.Append(Constants.Constant.Space);
                if (null != BowlerToBatsmanDetails && false == string.IsNullOrEmpty(BowlerToBatsmanDetails))
                {
                    text.Append(BowlerToBatsmanDetails);
                    text.Append(Constants.Constant.CommaWithSpace);
                }
                if (null != BallEvent && false == string.IsNullOrEmpty(BallEvent))
                {
                    text.Append(BallEvent);
                    text.Append(Constants.Constant.CommaWithSpace);
                }
                if (null != ActualCommentary && false == string.IsNullOrEmpty(ActualCommentary))
                {
                    text.Append(ActualCommentary);
                }
                return text.ToString();
            }
        }
    }
}
