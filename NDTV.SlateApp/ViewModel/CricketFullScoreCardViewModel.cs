using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.Properties;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// ViewModel for Cricket full score card
    /// </summary>
    public class CricketFullScorecardViewModel : ViewModelBase
    {

        /// <summary>
        /// complete details of the match score
        /// </summary>
        private MatchDetail matchDetail;

        /// <summary>
        /// Cricket fixture
        /// </summary>
        private CricketFixtures cricketFixture;

        /// <summary>
        /// Full scorecard response
        /// </summary>
        private FullScoreboardResponse responseScorecard;

        /// <summary>
        /// Relay command for innings selection changed
        /// </summary>
        private RelayCommand inningsCommand;

        /// <summary>
        /// Currently selected innings
        /// </summary>
        private CricketInnings selectedInnings;

        /// <summary>
        /// mini scorecard detials
        /// </summary>
        private MiniScorecardItem miniScoreDetails;

        /// <summary>
        /// check wether scorecard is loaded
        /// </summary>
        private bool isScorecardLoaded;

        /// <summary>
        /// Call back method
        /// when the response is loaded, this method will be fired
        /// </summary>
        private Action FullScorecardLoaded;

        /// <summary>
        /// Timer for refreshing cricket score
        /// </summary>
        private Timer cricketScoreTimer;

        /// <summary>
        /// Current match result
        /// </summary>
        private string currentMatchEquation;

        /// <summary>
        /// Request for full score card
        /// </summary>
        private FullScoreboardRequest request;

        /// <summary>
        /// Match Details
        /// </summary>
        public MatchDetail MatchDetail
        {
            get { return matchDetail; }
            set
            {
                matchDetail = value;
                if (null != value)
                {
                    ShareMediaType shareType = value.MatchFixtures.IsLive ? ShareMediaType.ScorecardLiveMatch : ShareMediaType.ScorecardRecentMatch;
                    ApplicationData.SetCurrentItem(matchDetail.MatchTitle, string.Empty, string.Empty, string.Empty, string.Format(CultureInfo.InvariantCulture,
                        Resources.CricketShareLink, new object[] { matchDetail.MatchFixtures.MatchFile }), shareType);
                }
                OnPropertyChanged("MatchDetail");
            }
        }
        
        /// <summary>
        /// Is score card response loaded
        /// </summary>
        public bool IsScorecardLoadingUnderProgress
        {
            get { return isScorecardLoaded; }
            set 
            { 
                isScorecardLoaded = value;
                OnPropertyChanged("IsScorecardLoadingUnderProgress");
            }
        }

        /// <summary>
        /// List of live cricket matches
        /// </summary>
        public ObservableCollection<CricketFixtures> LiveMatchList
        {
            get
            {
                if (null != ApplicationData.MatchFixtureResponse && null != ApplicationData.MatchFixtureResponse.LiveMatchList)
                {
                    return new ObservableCollection<CricketFixtures>(ApplicationData.MatchFixtureResponse.LiveMatchList);
                }
                else
                {
                    return new ObservableCollection<CricketFixtures>();
                }
            }
        }
       
        /// <summary>
        /// Mini score card
        /// </summary>
        public MiniScorecardItem MiniScoreDetails
        {
            get { return miniScoreDetails; }
            set { miniScoreDetails = value; OnPropertyChanged("MiniScoreDetails"); }
        }

        /// <summary>
        /// Current match result
        /// </summary>
        public string CurrentMatchEquation
        {
            get { return currentMatchEquation; }
            set { currentMatchEquation = value; OnPropertyChanged("CurrentMatchEquation"); }
        }

        /// <summary>
        /// Gets the selected innings
        /// </summary>
        public ICommand InningsCommand
        {
            get
            {
                if (null == inningsCommand)
                {
                    inningsCommand = new RelayCommand(InningsSelection);
                }
                return inningsCommand;
            }
        }

        /// <summary>
        /// Currently selected innings
        /// </summary>
        public CricketInnings SelectedInnings
        {
            get { return selectedInnings; }
            set
            {
                selectedInnings = value;
                OnPropertyChanged("SelectedInnings");
            }
        }
                        
        /// <summary>
        /// Response from full scorecard
        /// </summary>
        public FullScoreboardResponse ScorecardResponse
        {
            get { return responseScorecard; }
            set { responseScorecard = value; OnPropertyChanged("ScorecardResponse"); }
        }

        /// <summary>
        /// Constructor of the view model which will start the execution.
        /// </summary>
        /// <param name="fileName">Cricket match file name</param>
        /// <param name="state">state of match- whether live or recent</param>
        /// <param name="responseLoaded">call back method</param>
        public CricketFullScorecardViewModel(string fileName, CricketFixture state, Action responseLoaded)
        {
            IsScorecardLoadingUnderProgress = true;
            this.FullScorecardLoaded = responseLoaded;
            StartTimerIfMatchIsLive(state);
            this.cricketFixture = ApplicationData.RetrieveCricketFixture(fileName, state);
            if (null != cricketFixture)
            {
                InitializeValues();
            }
        }

        /// <summary>
        /// Constructor of the view model which will start the execution.
        /// </summary>
        /// <param name="fixture">Cricket fixture</param>
        /// <param name="state">state of match- whether live or recent</param>
        /// <param name="responseLoaded">call back method</param>
        public CricketFullScorecardViewModel(CricketFixtures fixture, CricketFixture state, Action responseLoaded)
        {
            IsScorecardLoadingUnderProgress = true;
            this.FullScorecardLoaded = responseLoaded;
            this.cricketFixture = fixture;
            StartTimerIfMatchIsLive(state);
            if (null != cricketFixture)
            {
                InitializeValues();
            }
        }

        /// <summary>
        /// Start the timer is match is live
        /// </summary>
        /// <param name="state">CricketFixture - whether match is live or not</param>
        private void StartTimerIfMatchIsLive(CricketFixture state)
        {
            if (state.Equals(CricketFixture.Live))
            {
                cricketScoreTimer = new Timer(int.Parse(Utilities.Utility.GetTimerInterval("CricketTimerInterval"), CultureInfo.InvariantCulture));
                this.cricketScoreTimer.Elapsed += CricketScoreTimerElapsed;
            }
        }

        /// <summary>
        /// Timer to get the latest response from API if the match is live
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="elapsedEventArgument">ElapsedEventArgs</param>
        private void CricketScoreTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgument)
        {
            this.InitializeValues();
        }

        /// <summary>
        /// Initialize the values
        /// </summary>
        private void InitializeValues()
        {
            request = new FullScoreboardRequest(this.cricketFixture);
            ProcessRequest(request, LoadResponse,null,false);
        }

        /// <summary>
        /// Loads the content after it has been retrieved
        /// </summary>
        /// <param name="fullScorecardResponse">Response</param>
        private void LoadResponse(Response fullScorecardResponse)
        {
            if (fullScorecardResponse.GetType() == typeof(FullScoreboardResponse))
            {
                ScorecardResponse = (FullScoreboardResponse)fullScorecardResponse;
                if (null != responseScorecard.DetailedMatchStatistics)
                {
                    SelectedInnings = ((null == responseScorecard.DetailedMatchStatistics.CurrentInnings) ? responseScorecard.DetailedMatchStatistics.FirstInnings :
                        responseScorecard.DetailedMatchStatistics.CurrentInnings);
                }
                MatchDetail = ScorecardResponse.DetailedMatchStatistics;
                if (null != MatchDetail && null != SelectedInnings && null != MatchDetail.MatchFixtures)
                {
                    CurrentMatchEquation = (false == MatchDetail.MatchFixtures.IsLive) ? MatchDetail.MatchResult : SelectedInnings.BattingTeam + Constants.Constant.Space + SelectedInnings.Equation.TotalScore + Constants.Constant.ForwardSlash + SelectedInnings.Equation.TotalWickets;
                }
                GenerateMiniScoreDetails();
                IsScorecardLoadingUnderProgress = false;
                //start the timer
                if (null != cricketScoreTimer)
                {
                    cricketScoreTimer.Start();
                }
                if (null != FullScorecardLoaded)
                {
                    (App.Current as App).Dispatcher.BeginInvoke(DispatcherPriority.Background, FullScorecardLoaded);
                }
            }

        }

        /// <summary>
        /// Set the selected innings based on radio button selection
        /// </summary>
        /// <param name="tag">object</param>
        private void InningsSelection(object tag)
        {
            switch (tag.ToString())
            {
                case Constants.Constant.FirstInnings:
                    SelectedInnings = MatchDetail.FirstInnings;
                    break;
                case Constants.Constant.SecondInnings:
                    SelectedInnings = MatchDetail.SecondInnings;
                    break;
                case Constants.Constant.ThirdInnings:
                    SelectedInnings = MatchDetail.ThirdInnings;
                    break;
                case Constants.Constant.FourthInnings:
                    SelectedInnings = MatchDetail.FourthInnings;
                    break;
            }
        }

        /// <summary>
        /// Generate the mini score card detail from full score card
        /// </summary>
        /// <returns>MiniScoreCard</returns>
        private MiniScorecardItem GenerateMiniScoreDetails()
        {
            if (null != this.MatchDetail && null != this.MatchDetail.CurrentInnings)
            {
                MiniScoreDetails = new MiniScorecardItem(this.MatchDetail.CurrentInnings);
                return miniScoreDetails;
            }
            return null;
        }

        /// <summary>
        /// Disposes all the objects in the view model
        /// </summary>
        protected override void DisposeResources()
        {
            if (null != cricketScoreTimer)
            {
                cricketScoreTimer.Stop();
            }
            this.cricketFixture = null;
            this.cricketScoreTimer = null;
            this.inningsCommand = null;
            this.MatchDetail = null;
            this.MiniScoreDetails = null;
            this.ScorecardResponse = null;
            this.SelectedInnings = null;
            this.FullScorecardLoaded = null;
        }

    }
}
