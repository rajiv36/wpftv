using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NDTV.Entities
{
    /// <summary>
    /// Model stores data for MiniScorecardItem
    /// </summary>
    public class MiniScorecardItem
    {
        /// <summary>
        /// list of wickets
        /// </summary>
        private FallOfWickets lastWicketData;

        /// <summary>
        /// partnership
        /// </summary>
        private string partnership;

        /// <summary>
        /// Current innings
        /// </summary>
        public CricketInnings Innings
        {
            get;
            set;
        }

        /// <summary>
        /// List contains batsman who are currently on crease
        /// </summary>
        public List<CricketBatsman> CurrentlyPlayingBatsmen
        {
            get;
            set;
        }

        /// <summary>
        /// List contains bowlers who are currently bowling
        /// </summary>
        public List<CricketBowler> CurrentlyBowlingBowler
        {
            get;
            set;
        }

        /// <summary>
        /// Current run rate
        /// </summary>
        public string CurrentRunRate
        {
            get
            {
                if (null != Innings && null != Innings.Equation)
                {
                    return ((null != Innings) ? Innings.Equation.RunRate.ToString() : string.Empty);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Last wicket
        /// </summary>
        public string LastWicket
        {
            get;
            set;
        }

        /// <summary>
        /// Get the short summary of equation of the innings
        /// </summary>
        public string Equation
        {
            get
            {
                string battingHeader = string.Empty;
                if (null != Innings && null != Innings.Equation)
                {
                    battingHeader = Constants.Constant.Space + Innings.Equation.TotalScore.ToString(CultureInfo.CurrentCulture) + Constants.Constant.ForwardSlash + Innings.Equation.TotalWickets.ToString(CultureInfo.CurrentCulture);
                    battingHeader += Constants.Constant.Space + Constants.Constant.OpeningBrace + Innings.Equation.TotalOvers + Constants.Constant.Space + "Ovs" + Constants.Constant.ClosingBrace;
                    return battingHeader;
                }
                return battingHeader;
            }
        }

        /// <summary>
        /// Partnership of the current pair
        /// </summary>
        public string Partnership
        {
            get { return partnership; }
            set { partnership = value; }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="innings">CricketInnings - current innings</param>
        public MiniScorecardItem(CricketInnings innings)
        {
            Innings = innings;
            CurrentlyPlayingBatsmen = new List<CricketBatsman>();
            CurrentlyBowlingBowler = new List<CricketBowler>();
            Parse();
        }

        /// <summary>
        /// Parse the incoming response
        /// </summary>
        private void Parse()
        {
            if (null != Innings)
            {
                if(Innings.BatsmanList.Any(a => a.HowGotDismissed.Equals(Constants.Constant.Batting)))
                {
                    var bats = (from batsman in Innings.BatsmanList
                                where batsman.HowGotDismissed.Equals(Constants.Constant.Batting)
                                select batsman).ToList();
                    if (null != bats && bats.Count != 0)
                    {
                        for (int elementIndex = 0; elementIndex < bats.Count; elementIndex++)
                        {
                            var currentElement = bats[elementIndex];
                            CurrentlyPlayingBatsmen.Add(currentElement);
                        }
                        AddBlankRowIfCountIsNotTwo(CurrentlyPlayingBatsmen);
                    }
                    else
                    {
                        AddBlankRowIfCountIsNotTwo(CurrentlyPlayingBatsmen);
                    }
                }
                else
                {
                    var bats = (from batsman in Innings.BatsmanList
                                where batsman.HowGotDismissed.Equals(Constants.Constant.NotOut)
                                select batsman).ToList();
                    if (null != bats && bats.Count != 0 && bats.Count == 2)
                    {
                        for (int elementIndex = 0; elementIndex < bats.Count; elementIndex++)
                        {
                            var currentElement = bats[elementIndex];
                            CurrentlyPlayingBatsmen.Add(currentElement);
                        }
                    }
                    else
                    {
                        if (null != bats && bats.Count > 0)
                        {
                            CurrentlyPlayingBatsmen.Add(bats[0]);
                            CricketBatsman cricketBats = LastBatsman();
                            if (null != cricketBats)
                            {
                                CurrentlyPlayingBatsmen.Add(cricketBats);
                            }
                            AddBlankRowIfCountIsNotTwo(CurrentlyPlayingBatsmen);
                        }
                        else
                        {
                            AddBlankRowIfCountIsNotTwo(CurrentlyPlayingBatsmen);
                        }
                    }
                }

                if (Innings.BowlerList.Any(a => a.Bowler.Equals(Constants.Constant.Yes)))
                {
                    var bowlerList = (from bowler in Innings.BowlerList
                                      where bowler.Bowler.Equals(Constants.Constant.Yes)
                                      select bowler).ToList();
                    if (null != bowlerList && bowlerList.Count != 0)
                    {
                        for (int elementIndex = 0; elementIndex < bowlerList.Count; elementIndex++)
                        {
                            var currentElement = bowlerList[elementIndex];
                            CurrentlyBowlingBowler.Add(currentElement);
                        }
                        AddBlankRowIfBowlerCountIsNotTwo(CurrentlyBowlingBowler);
                    }
                    else
                    {
                        AddBlankRowIfBowlerCountIsNotTwo(CurrentlyBowlingBowler);
                    }
                }
                else
                {
                    var bowlerList = Innings.BowlerList.Take(2).ToList();
                    if (null != bowlerList && bowlerList.Count != 0)
                    {
                        for (int elementIndex = 0; elementIndex < bowlerList.Count; elementIndex++)
                        {
                            var currentElement = bowlerList[elementIndex];
                            CurrentlyBowlingBowler.Add(currentElement);
                        }
                    }
                    AddBlankRowIfBowlerCountIsNotTwo(CurrentlyBowlingBowler);
                }
                if (null != Innings.ListOfWickets && Innings.ListOfWickets.Count != 0)
                {
                    var wicketsFall = (from wickets in Innings.ListOfWickets
                                       select wickets).Last();
                    if (null != wicketsFall.Value)
                    {
                        lastWicketData = (FallOfWickets)wicketsFall.Value;
                        if (null != lastWicketData && Innings.BatsmanList.Count != 0)
                        {
                            var lastBatsman = (from lastWicket in Innings.BatsmanList
                                               where lastWicket.Name.Contains(lastWicketData.BatsmanName)
                                               select lastWicket).First();
                            if (null != lastBatsman)
                            {
                                CricketBatsman cricketBats = (CricketBatsman)lastBatsman;
                                StringBuilder lastBatsmanDismissed = new StringBuilder();
                                lastBatsmanDismissed.Append(cricketBats.Name);
                                lastBatsmanDismissed.Append(Constants.Constant.Space);
                                lastBatsmanDismissed.Append(cricketBats.RunsScored + Constants.Constant.OpeningBrace);
                                lastBatsmanDismissed.Append(cricketBats.BallsFaced + Constants.Constant.ClosingBrace);
                                LastWicket = lastBatsmanDismissed.ToString();

                                CalculatePartnership(lastWicketData);
                            }
                        }
                        else
                        {
                            LastWicket = string.Empty;
                        }
                    }
                }
                else
                {
                    CalculatePartnership(null);
                }
            }
        }

        /// <summary>
        /// Add blank batsman if the count is not 2
        /// </summary>
        /// <param name="currentlyBowlingBowler">Bowler list</param>
        private void AddBlankRowIfBowlerCountIsNotTwo(List<CricketBowler> currentlyBowlingBowler)
        {
            if (null != currentlyBowlingBowler && currentlyBowlingBowler.Count != 2)
            {
                CricketBowler bowler = new CricketBowler();
                bowler.Name = string.Empty;
                bowler.Overs = string.Empty;
                CurrentlyBowlingBowler.Add(bowler);
                AddBlankRowIfBowlerCountIsNotTwo(CurrentlyBowlingBowler);
            }
        }

        /// <summary>
        /// Add blank batsman if the count is not 2
        /// </summary>
        /// <param name="currentlyPlayingBatsmen">List of batsman</param>
        private void AddBlankRowIfCountIsNotTwo(List<CricketBatsman> currentlyPlayingBatsmen)
        {
            if (null != currentlyPlayingBatsmen && currentlyPlayingBatsmen.Count != 2)
            {
                CricketBatsman batsman = new CricketBatsman();
                batsman.Name = string.Empty;
                batsman.RunsScored = string.Empty;
                batsman.BallsFaced = string.Empty;
                CurrentlyPlayingBatsmen.Add(batsman);
                AddBlankRowIfCountIsNotTwo(CurrentlyPlayingBatsmen);
            }
        }

        /// <summary>
        /// Last batsman who got out
        /// </summary>
        /// <returns>CricketBatsman</returns>
        private CricketBatsman LastBatsman()
        {
            if (null != Innings.ListOfWickets && Innings.ListOfWickets.Count != 0)
            {
                var wicketsFall = (from wickets in Innings.ListOfWickets
                                   select wickets).Last();
                if (null != wicketsFall.Value)
                {
                    lastWicketData = (FallOfWickets)wicketsFall.Value;
                    if (null != lastWicketData && Innings.BatsmanList.Count != 0)
                    {
                        var lastBatsman = (from lastWicket in Innings.BatsmanList
                                           where lastWicket.Name.Contains(lastWicketData.BatsmanName)
                                           select lastWicket).First();
                        if (null != lastBatsman)
                        {
                            return (CricketBatsman)lastBatsman;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Calculate partnership
        /// </summary>
        /// <param name="lastWicketData">lastWicketData</param>
        private void CalculatePartnership(FallOfWickets lastWicketData)
        {
            int currentRuns = 0;
            int runsAtLastWicket = 0;
            int completedOvers;
            int balls;
            int totalBalls = 0;
            int ballsAtLastWicket = 0;
            int netBalls = 0;
            if (null != Innings && null != Innings.Equation)
            {
                if (null != lastWicketData)
                {
                    currentRuns = Innings.Equation.TotalScore;
                    runsAtLastWicket = Convert.ToInt32(lastWicketData.Run, CultureInfo.InvariantCulture);
                    int partenershipRuns = currentRuns - runsAtLastWicket;
                    totalBalls = CalculateBallsFromOver();
                    if (null != lastWicketData.Over)
                    {
                        if (lastWicketData.Over.Contains(Constants.Constant.FullStop))
                        {
                            completedOvers = 0;
                            balls = 0;
                            string[] oversEquation = lastWicketData.Over.Split('.');
                            if (oversEquation.Length == 2)
                            {
                                completedOvers = Convert.ToInt32(oversEquation[0], CultureInfo.InvariantCulture);
                                balls = Convert.ToInt32(oversEquation[1], CultureInfo.InvariantCulture);
                                ballsAtLastWicket = (completedOvers * 6) + balls;
                            }
                            else
                            {
                                ballsAtLastWicket = (Convert.ToInt32(lastWicketData.Over, CultureInfo.InvariantCulture) * 6);
                            }
                        }
                        else
                        {
                            ballsAtLastWicket = (Convert.ToInt32(lastWicketData.Over, CultureInfo.InvariantCulture) * 6);
                        }
                    }
                    netBalls = totalBalls - ballsAtLastWicket;
                    partnership = Constants.Constant.Partnership + partenershipRuns.ToString(CultureInfo.CurrentCulture) + Constants.Constant.OpeningBrace + netBalls.ToString(CultureInfo.CurrentCulture) + Constants.Constant.ClosingBrace;
                }
                else
                {
                    partnership = Constants.Constant.Partnership + Innings.Equation.TotalScore.ToString(CultureInfo.CurrentCulture) + Constants.Constant.OpeningBrace + CalculateBallsFromOver().ToString(CultureInfo.CurrentCulture) + Constants.Constant.ClosingBrace;
                }
            }
            else
            {
                partnership = string.Empty;
            }
        }

        /// <summary>
        /// Calculate total balls from over
        /// </summary>
        /// <returns>number of balls</returns>
        private int CalculateBallsFromOver()
        {
            int totalBalls = 0;
            if (Innings.Equation.TotalOvers.Contains(Constants.Constant.FullStop))
            {
                string[] oversEquation = Innings.Equation.TotalOvers.Split('.');
                if (oversEquation.Length == 2)
                {
                    totalBalls = ((Convert.ToInt32(oversEquation[0], CultureInfo.InvariantCulture)) * 6) + Convert.ToInt32(oversEquation[1], CultureInfo.InvariantCulture);
                }
            }
            else
            {
                totalBalls = (Convert.ToInt32(Innings.Equation.TotalOvers, CultureInfo.InvariantCulture) * 6);
            }
            return totalBalls;
        }

    }
}
