using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
namespace NDTV.Entities
{
    /// <summary>
    /// Class holds statistics of bowler
    /// </summary>
    public class CricketBowler
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CricketBowler()
        { 
        
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="element">XElement</param>
        /// <param name="innings">string - innings name</param>
        public CricketBowler(XElement element, string innings)
        {
            bowlerList = new SortedList<int, CricketBowler>();
            Parse(element, innings);
        }

        /// <summary>
        /// Parse the incoming XElement and store the data in model
        /// </summary>
        /// <param name="element">XElement - xml of response</param>
        /// <param name="innings">string</param>
        private void Parse(XElement element, string innings)
        {
            int result;
                var bowler = (from eachItem in element.Elements(innings + "Bowler")
                              select new CricketBowler()
                               {
                                   BowlingOrder = int.TryParse(eachItem.Attribute("Number").Value, out result) ? result : -1,
                                   Overs = eachItem.Attribute("Overs").Value.ToString(CultureInfo.CurrentCulture),
                                   MaidenOvers = int.TryParse(eachItem.Attribute("Maidens").Value, out result) ? result : -1,
                                   RunsGiven = int.TryParse(eachItem.Attribute("Runsgiven").Value, out result) ? result : -1,
                                   WicketsTaken = int.TryParse(eachItem.Attribute("Wickets").Value, out result) ? result : -1,
                                   NoBalls = int.TryParse(eachItem.Attribute("Noballs").Value, out result) ? result : -1,
                                   Wides = int.TryParse(eachItem.Attribute("Wides").Value, out result) ? result : -1,
                                   Bowler = (null != eachItem.Attribute("Bowlers")) ? eachItem.Attribute("Bowlers").Value.ToString(CultureInfo.InvariantCulture) : string.Empty,
                                   Bowling = (null != eachItem.Attribute("Bowling")) ? eachItem.Attribute("Bowling").Value.ToString(CultureInfo.InvariantCulture) : string.Empty,
                                   name = eachItem.Value.ToString(CultureInfo.CurrentCulture),
                               }).ToList();
                if (null != bowler && bowler.Count > 0)
                {
                    for (int elementIndex = 0; elementIndex < bowler.Count; elementIndex++)
                    {
                        var currentElement = bowler[elementIndex];
                        if (currentElement.BowlingOrder == -1)
                        {
                            continue;
                        }
                        bowlerList.Add(currentElement.BowlingOrder, currentElement);
                    }
                }
        }

        /// <summary>
        /// Bowler list in sorted order
        /// </summary>
        private SortedList<int, CricketBowler> bowlerList;

        /// <summary>
        /// Gets the category item list
        /// </summary>
        public List<CricketBowler> BowlerList
        {
            get
            {
                return bowlerList.Values.ToList();
            }
        }

        /// <summary>
        /// Order at which bowler is coming to bowl
        /// </summary>
        public int BowlingOrder
        {
            get;
            set;
        }

        /// <summary>
        /// If the bowler is cuurently bowling
        /// </summary>
        public string Bowler
        {
            get;
            set;
        }

        private string name;
        /// <summary>
        /// Name of the bowler
        /// </summary>
        public string Name
        {
            get
            {
                if (false == string.IsNullOrWhiteSpace(name))
                {
                    return (name + (Bowling.Equals(Constants.Constant.Yes) ? Constants.Constant.Asterisk : string.Empty));
                }
                return string.Empty;
            }
            set
            { 
                name = value;
            }
        }

        /// <summary>
        /// Overs bowled
        /// </summary>
        public string Overs
        {
            get;
            set;
        }

        /// <summary>
        /// Maiden overs bowled
        /// </summary>
        public int MaidenOvers
        {
            get;
            set;
        }

        /// <summary>
        /// Runs given by bowler
        /// </summary>
        public int RunsGiven
        {
            get;
            set;
        }

        /// <summary>
        /// Number of wickets taken
        /// </summary>
        public int WicketsTaken
        {
            get;
            set;
        }

        /// <summary>
        /// No balls bowled
        /// </summary>
        public int NoBalls
        {
            get;
            set;
        }

        /// <summary>
        /// Wides bowled
        /// </summary>
        public int Wides
        {
            get;
            set;
        }

        /// <summary>
        /// Calculate economy rate of the bowler
        /// </summary>
        public string EconomyRate
        {
            get
            {
                double netOvers = 0;
                double economy = 0;
                string economyRate = string.Empty;
                if (false == string.IsNullOrWhiteSpace(Overs))
                {
                    if (Overs.Contains("."))
                    {
                        string[] calculatedOvers = Overs.Split('.');
                        if (calculatedOvers.Length == 2)
                        {
                            netOvers = ((Convert.ToDouble(calculatedOvers[0], CultureInfo.InvariantCulture)) + (Convert.ToDouble(calculatedOvers[1], CultureInfo.InvariantCulture) / 6));
                        }
                    }
                    else
                    {
                        netOvers = Convert.ToDouble(Overs, CultureInfo.InvariantCulture);
                    }
                    if (netOvers != 0)
                    {
                        economy = (RunsGiven / netOvers);
                        economyRate = string.Format(CultureInfo.CurrentCulture, Constants.Constant.StrikeRateFormat, economy);
                    }
                    else
                    {
                        economyRate = Constants.Constant.Hyphen;
                    }
                    return economyRate;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Whether the bowler is currently bowling
        /// </summary>
        public string Bowling
        {
            get;
            set;
        }

        /// <summary>
        /// Mini equation for bowler like 14-1-26-2
        /// </summary>
        public string MiniEquation
        {
            get 
            {
                if (false == string.IsNullOrWhiteSpace(Name) && false == string.IsNullOrWhiteSpace(MaidenOvers.ToString(CultureInfo.InvariantCulture)) && false == string.IsNullOrWhiteSpace(RunsGiven.ToString(CultureInfo.InvariantCulture)))
                {
                    return (Overs + Constants.Constant.Hyphen + MaidenOvers.ToString(CultureInfo.CurrentCulture) + Constants.Constant.Hyphen + RunsGiven.ToString(CultureInfo.CurrentCulture) + Constants.Constant.Hyphen + WicketsTaken.ToString(CultureInfo.CurrentCulture));
                }
                return string.Empty;
            }
        }
    }
}
