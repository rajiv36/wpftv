using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
namespace NDTV.Entities
{
    /// <summary>
    /// Class to store attributes about batsman
    /// </summary>
    public class CricketBatsman
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CricketBatsman()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="element">XElement - response message</param>
        /// <param name="innings">string - innings name</param>
        public CricketBatsman(XElement element, string innings)
        {
            batsmenList = new SortedList<int, CricketBatsman>();
            Parse(element, innings);
        }

        /// <summary>
        /// Parse the incoming XElement and store the data in model
        /// </summary>
        /// <param name="element">XElement - xml containing response</param>
        /// <param name="innings">string</param>
        private void Parse(XElement element, string innings)
        {
            int result;

            var batsmen = (from eachItem in element.Elements(innings + "Batsman")
                           select new CricketBatsman()
                           {
                               BattingOrder = (null != eachItem.Attribute("Number")) ? (int.TryParse(eachItem.Attribute("Number").Value, out result) ? result : -1) : -1,
                               name = (null != eachItem)? eachItem.Value.ToString() : string.Empty,
                               HowGotDismissed = (null != eachItem.Attribute("Howout"))? eachItem.Attribute("Howout").Value.ToString(): string.Empty,
                               RunsScored = (null != eachItem.Attribute("Runs")) ? eachItem.Attribute("Runs").Value : string.Empty,
                               BallsFaced = (null != eachItem.Attribute("BallsFaced")) ? eachItem.Attribute("BallsFaced").Value : string.Empty,
                               Fours = (null != eachItem.Attribute("Fours")) ? eachItem.Attribute("Fours").Value : string.Empty,
                               Sixes = (null != eachItem.Attribute("Sixes")) ? eachItem.Attribute("Sixes").Value : string.Empty,
                               Striker = (null != eachItem.Attribute("Striker")) ? eachItem.Attribute("Striker").Value.ToString() : string.Empty
                           }).ToList();
            if (null != batsmen && batsmen.Count > 0)
            {
                for (int elementIndex = 0; elementIndex < batsmen.Count; elementIndex++)
                {
                    var currentElement = batsmen[elementIndex];
                    if (currentElement.BattingOrder == -1)
                    {
                        continue;
                    }
                    batsmenList.Add(currentElement.BattingOrder, currentElement);
                }
            }
        }

        /// <summary>
        /// Batsman list in sorted order
        /// </summary>
        private SortedList<int, CricketBatsman> batsmenList;

        /// <summary>
        /// List of batsman
        /// </summary>
        public List<CricketBatsman> BatsmenList
        {
            get
            {
                return batsmenList.Values.ToList();
            }
        }

        /// <summary>
        /// Order at which batsman is coming to crease for batting
        /// </summary>
        public int BattingOrder
        {
            get;
            set;
        }
        private string name;

        /// <summary>
        /// Name of the Batsman
        /// </summary>
        public string Name
        {
            get
            {
                if (false == string.IsNullOrWhiteSpace(name))
                {
                    return (name + (Striker.Equals(Constants.Constant.Yes) ? Constants.Constant.Asterisk : string.Empty));
                }
                return string.Empty;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Status of Batsman
        /// </summary>
        public string HowGotDismissed
        {
            get;
            set;
        }

        /// <summary>
        /// Runs scored by the batsman
        /// </summary>
        public string RunsScored
        {
            get;
            set;
        }

        /// <summary>
        /// Balls faced by the batsman
        /// </summary>
        public string BallsFaced
        {
            get;
            set;
        }

        /// <summary>
        /// 4's hit by batsman
        /// </summary>
        public string Fours
        {
            get;
            set;
        }

        /// <summary>
        /// 6's hit by batsman
        /// </summary>
        public string Sixes
        {
            get;
            set;
        }

        /// <summary>
        /// Whether batsman is striker
        /// </summary>
        public string Striker
        {
            get;
            set;
        }

        /// <summary>
        /// Strike rate
        /// </summary>
        public string StrikeRate
        {
            get
            {
                if (false == string.IsNullOrEmpty(RunsScored) && false == string.IsNullOrEmpty(BallsFaced))
                {
                    double strike = ((Convert.ToDouble(RunsScored,CultureInfo.InvariantCulture) / (Convert.ToDouble(BallsFaced,CultureInfo.InvariantCulture))) * 100);
                    if ((strike.ToString(CultureInfo.CurrentCulture).Contains("NaN")))
                    {
                        return Constants.Constant.Hyphen;
                    }
                    return string.Format(CultureInfo.CurrentCulture,Constants.Constant.StrikeRateFormat,strike);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Mini equation like 14(26)
        /// </summary>
        public string MiniEquation
        {
            get
            {
                if (false == string.IsNullOrWhiteSpace(RunsScored) && false == string.IsNullOrWhiteSpace(BallsFaced))
                {
                    return RunsScored + Constants.Constant.OpeningBrace + BallsFaced + Constants.Constant.ClosingBrace;
                }
                return string.Empty;
            }
        }
    }
}
