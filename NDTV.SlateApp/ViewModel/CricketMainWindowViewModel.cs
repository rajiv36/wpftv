using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Windows.Threading;
using NDTV.Controller;
using NDTV.Entities;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// View model for cricket main window
    /// </summary>
    public class CricketMainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Callback method for CricketFixtureResponseLoaded
        /// </summary>
        private Action CricketFixtureResponseLoaded;

        /// <summary>
        /// Fixture request
        /// </summary>
        private CricketFixturesRequest cricketFixtureRequest;

        /// <summary>
        /// Live match list
        /// </summary>
        private List<CricketFixtures> liveMatchList;

        /// <summary>
        /// User control - Cricket calendar
        /// </summary>
        private View.CricketCalendarControl cricketCalendarControl;

        /// <summary>
        /// User control - mini scorecard
        /// </summary>
        private View.MiniScorecard miniScorecard;

        /// <summary>
        /// Current fixture
        /// </summary>
        private CricketFixtures currentFixture;

        /// <summary>
        /// User control - Full scorecard
        /// </summary>
        private View.FullScorecard fullScore;

        /// <summary>
        /// User control - Cricket commentary
        /// </summary>
        private View.CricketCommentary fullCricketCommentary;

        /// <summary>
        /// View model - cricket fixtures
        /// </summary>
        private CricketFixturesViewModel fixtureViewModel;

        /// <summary>
        /// Timer for cricket fixture
        /// </summary>
        private Timer cricketFixtureTimer;

        /// <summary>
        /// Timer to get the latest response from API
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="elapsedEventArgument">ElapsedEventArgs</param>
        private void CricketFixtureTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgument)
        {
            LoadCricketFixtures();
        }

        /// <summary>
        /// Full Cricket commentary
        /// </summary>
        public View.CricketCommentary FullCricketCommentary
        {
            get { return fullCricketCommentary; }
            set { fullCricketCommentary = value; }
        } 

        /// <summary>
        /// Full Score card 
        /// </summary>
        public View.FullScorecard FullScore
        {
            get { return fullScore; }
            set { fullScore = value;}
        }

        /// <summary>
        /// CricketCalendarControl
        /// </summary>
        public View.CricketCalendarControl CricketCalendarControl
        {
            get { return cricketCalendarControl; }
            set { cricketCalendarControl = value; }
        }
       
        /// <summary>
        /// Mini score card
        /// </summary>
        public View.MiniScorecard MiniScorecard
        {
            get { return miniScorecard; }
            set { miniScorecard = value;}
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseLoadedForCricketFixture">call back method</param>
        public CricketMainWindowViewModel(Action responseLoadedForCricketFixture)
        {
            fixtureViewModel = new CricketFixturesViewModel();
            cricketFixtureTimer = new Timer(int.Parse(Utilities.Utility.GetTimerInterval("CricketTimerInterval"), CultureInfo.InvariantCulture));
            cricketFixtureTimer.Elapsed +=CricketFixtureTimerElapsed;
            this.CricketFixtureResponseLoaded = responseLoadedForCricketFixture;
            LoadCricketFixtures();
        }

        /// <summary>
        /// Load mini score card
        /// </summary>
        public void LoadMiniScorecard()
        {
            CricketCalendarControl = new View.CricketCalendarControl(Constants.Constant.Recent);
            MiniScorecard = new View.MiniScorecard() { MatchFile = currentFixture };
        }

        /// <summary>
        /// Load Live match and returns the cricket fixture
        /// </summary>
        /// <param name="fileName">fileName of the current match</param>
        /// <returns>CricketFixtures of the current match</returns>
        public CricketFixtures LoadLiveMatchFile(string fileName)
        {
            liveMatchList = ApplicationData.MatchFixtureResponse.LiveMatchList.ToList();
            if (null == fileName && null != liveMatchList && liveMatchList.Count > 0)
            {
                bool indiaMatchExist = false;
                foreach (var item in liveMatchList)
                {
                    if (item.TeamA.Contains("India") || item.TeamB.Contains("India"))
                    {
                        indiaMatchExist = true;
                        break;
                    }
                }
                if (indiaMatchExist)
                {
                    var indiaFixture = (from fixture in liveMatchList
                                        where (fixture.TeamA.Contains("India") || fixture.TeamB.Contains("India"))
                                        select fixture).First();
                    if (null != indiaFixture)
                    {
                        currentFixture = (CricketFixtures)indiaFixture;
                        return currentFixture;
                    }
                    else
                    {
                        currentFixture = liveMatchList.First();
                        return currentFixture;
                    }
                }
                else
                {
                    currentFixture = liveMatchList.First();
                    return currentFixture;
                }
            }
            else if (null != fileName && false == string.IsNullOrEmpty(fileName) && null != liveMatchList && liveMatchList.Count > 0)
            {
                currentFixture = (ApplicationData.RetrieveCricketFixture(fileName, CricketFixture.Live));
                return currentFixture; 
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Show full score card
        /// </summary>
        /// <param name="matchFile">match file of the match</param>
        /// <param name="fullScorecardModel">CricketFullScorecardViewModel</param>
        public void ShowFullScorecard(string matchFile, CricketFullScorecardViewModel fullScorecardModel)
        {
            if (null != FullScore)
            {
                FullScore = null;
            }
            FullScore = (null != matchFile) ? new View.FullScorecard(matchFile, CricketFixture.Recent) : new View.FullScorecard(fullScorecardModel);
        }

        /// <summary>
        /// Show full commentary
        /// </summary>
        /// <param name="fullScorecardViewModel">CricketFullScoreCardViewModel</param>
        public void ShowFullCommentary(CricketFullScorecardViewModel fullScorecardViewModel)
        {
            if (null != FullCricketCommentary)
            {
                FullCricketCommentary = null;
            }
            FullCricketCommentary = new View.CricketCommentary(fullScorecardViewModel);
        }

        /// <summary>
        /// Loads the Cricket fixtures after it has been retrieved
        /// </summary>
        /// <param name="response">Response</param>
        private void LoadCricketFixturesResponse(Response response)
        {
            if (response.GetType() == typeof(CricketFixturesResponse))
            {
                ApplicationData.MatchFixtureResponse = (CricketFixturesResponse)response;
                if (null != fixtureViewModel)
                {
                    fixtureViewModel.FixtureResponse = ApplicationData.MatchFixtureResponse;
                }
            }
           
            //start the timer
            if (null != cricketFixtureTimer)
            {
                cricketFixtureTimer.Start();
            }
            if (null != this.CricketFixtureResponseLoaded && null != App.Current)
            {
                (App.Current as App).Dispatcher.BeginInvoke(DispatcherPriority.Background, CricketFixtureResponseLoaded);
            }
        }

        /// <summary>
        /// Load cricket fixtures
        /// </summary>
        private void LoadCricketFixtures()
        {
            cricketFixtureRequest = new CricketFixturesRequest();
            ProcessRequest(cricketFixtureRequest, LoadCricketFixturesResponse);
        }

        /// <summary>
        /// Disposes the objects in the view model
        /// </summary>
        protected override void DisposeResources()
        {
            this.FullCricketCommentary = null;
            this.FullScore = null;
            this.liveMatchList = null;
            this.MiniScorecard = null;
            if (null != cricketFixtureTimer)
            {
                cricketFixtureTimer.Stop();
            }
            this.cricketFixtureTimer = null;
            this.cricketFixtureRequest = null;
            this.CricketCalendarControl = null;
            this.fixtureViewModel = null;
            this.CricketFixtureResponseLoaded = null;
        }

    }
}
