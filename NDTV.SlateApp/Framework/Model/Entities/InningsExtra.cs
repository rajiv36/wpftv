using System.Globalization;
using System.Text;
namespace NDTV.Entities
{
    /// <summary>
    /// Extra runs given in an inning
    /// </summary>
    public class InningsExtra
    {
        /// <summary>
        /// Number of byes given
        /// </summary>
        public int Byes
        { 
            get;
            set;
        }

        /// <summary>
        /// number of Legbyes given
        /// </summary>
        public int LegByes
        {
            get;
            set;
        }

        /// <summary>
        /// Total number of Wides bowled
        /// </summary>
        public int Wides
        {
            get;
            set;
        }

        /// <summary>
        /// Total number of no balls bowled
        /// </summary>
        public int NoBalls
        {
            get;
            set;
        }

        /// <summary>
        /// Runs awarded in Penalty
        /// </summary>
        public int Penalty
        {
            get;
            set;
        }

        /// <summary>
        /// Total extras
        /// </summary>
        public int Total
        {
            get
            {
                return (Byes + LegByes + Wides + NoBalls + Penalty);
            }
        }

        /// <summary>
        /// Format the string in particular fashion
        /// </summary>
        /// <returns>formatted string</returns>
        public override string ToString()
        {
            StringBuilder formattedString = new StringBuilder();
            formattedString.Append(Constants.Constant.OpeningBrace + Constants.Constant.Space);
            formattedString.Append("b - " + Byes.ToString(CultureInfo.InvariantCulture));
            formattedString.Append(" ,lb - " + LegByes.ToString(CultureInfo.InvariantCulture));
            formattedString.Append(" ,w - " + Wides.ToString(CultureInfo.InvariantCulture));
            formattedString.Append(" ,nb - " + NoBalls.ToString(CultureInfo.InvariantCulture));
            if (false == Penalty.Equals(0))
            {
                formattedString.Append(" ,penalty - " + LegByes.ToString(CultureInfo.InvariantCulture) + Constants.Constant.Space + Constants.Constant.ClosingBrace);
            }
            else
            {
                formattedString.Append(Constants.Constant.Space + Constants.Constant.ClosingBrace);
            }
            return formattedString.ToString();
        }
    }
}
