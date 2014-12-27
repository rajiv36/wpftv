using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using NDTV.Entities;
using System.Windows.Threading;
using System.Timers;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// View Model for cricket commentary
    /// </summary>
    public class CricketCommentaryViewModel : ViewModelBase
    {
        /// <summary>
        /// Flag to check whether commentary is loaded or not
        /// </summary>
        private bool isCommentaryLoadingInProgress;

        /// <summary>
        /// innings name to fetch the commentary of selected innings
        /// </summary>
        private string inningsName;

        /// <summary>
        /// Commentary type, whether short or detailed
        /// </summary>
        private CricketCommentary type;

        /// <summary>
        /// Cricket fixture
        /// </summary>
        private CricketFixtures fixture;

        /// <summary>
        /// Call back for commentary response loaded
        /// </summary>
        private Action CommentaryLoaded;

        /// <summary>
        /// commentary list
        /// </summary>
        private ObservableCollection<CricketCommentaryItem> commentaryList;

        /// <summary>
        /// Timer to refresh commentary
        /// </summary>
        private Timer commentaryTimer;

        /// <summary>
        /// Commentary list
        /// </summary>
        public ObservableCollection<CricketCommentaryItem> CommentaryList
        {
            get { return commentaryList; }
            private set { commentaryList = value; OnPropertyChanged("CommentaryList"); }
        }

        /// <summary>
        /// Check wether commentary is loaded
        /// </summary>
        public bool IsCommentaryLoadingInProgress
        {
            get { return isCommentaryLoadingInProgress; }
            set 
            {
                isCommentaryLoadingInProgress = value;
                OnPropertyChanged("IsCommentaryLoadingInProgress");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inningsName">string - innings name</param>
        /// <param name="type">CricketCommentary - short or detailed</param>
        /// <param name="fixture">CricketFixtures</param>
        /// <param name="commentaryLoadedCallback">call back method</param>
        public CricketCommentaryViewModel(string inningsName, CricketCommentary type, CricketFixtures fixture,Action commentaryLoadedCallback)
        {
            IsCommentaryLoadingInProgress = true;
            this.CommentaryLoaded = commentaryLoadedCallback;
            if (null != fixture && null != inningsName && null != fixture.MatchFile)
            {
                if (fixture.IsLive)
                {
                   commentaryTimer = new Timer(int.Parse(Utilities.Utility.GetTimerInterval("CricketTimerInterval"), CultureInfo.InvariantCulture));
                   commentaryTimer.Elapsed += CommentaryTimerElapsed;
                }
                this.inningsName = inningsName;
                this.type = type;
                this.fixture = fixture;
                InitializeValues();
            }
        }

        /// <summary>
        /// Timer to get the latest response from API
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="elapsedEventArguments"></param>
        private void CommentaryTimerElapsed(object sender, ElapsedEventArgs elapsedEventArguments)
        {
            this.InitializeValues();
        }

        /// <summary>
        /// Initialize the values
        /// </summary>
        private void InitializeValues()
        {
            CricketCommentaryRequest request = new CricketCommentaryRequest(BuildCommentaryLink());
            ProcessRequest(request, LoadResponse,HandleError,false);
        }

        /// <summary>
        /// This method handles the error for Request 
        /// </summary>
        /// <param name="request">request</param>
        private void HandleError(Request request)
        {
            if (request.GetType() == typeof(CricketCommentaryRequest))
            {
                if (null != CommentaryLoaded && null != App.Current)
                {
                    IsCommentaryLoadingInProgress = false;
                    CommentaryList = new ObservableCollection<CricketCommentaryItem>();
                    (App.Current as App).Dispatcher.BeginInvoke(DispatcherPriority.Background, CommentaryLoaded);
                }
            }
        }

        /// <summary>
        /// Build commentary link
        /// </summary>
        /// <returns>commentary link</returns>
        private string BuildCommentaryLink()
        {
            StringBuilder link = new StringBuilder();
            link.Append(fixture.MatchFile);
            link.Append(Constants.Constant.Underscore);
            link.Append(Constants.Constant.Commentary);
            link.Append(Constants.Constant.Underscore);
            link.Append(inningsName);
            switch (type)
            {
                case CricketCommentary.Short:
                    link.Append(Constants.Constant.Underscore);
                    link.Append(Constants.Constant.LastTwelveBalls);
                    break;
                case CricketCommentary.Detailed:
                    link.Append(Constants.Constant.Underscore);
                    link.Append(Constants.Constant.FullInnings);
                    break;
            }
            return link.ToString();
        }

        /// <summary>
        /// Loads the content after it has been retrieved
        /// </summary>
        /// <param name="commentaryResponse">Response</param>
        private void LoadResponse(Response commentaryResponse)
        {
            if (null != commentaryResponse)
            {
                if (commentaryResponse.GetType() == typeof(CricketCommentaryResponse))
                {
                    CommentaryList = ((CricketCommentaryResponse)commentaryResponse).CommentaryList;
                }
            }
            isCommentaryLoadingInProgress = false;
            //start the timer
            if (null != commentaryTimer)
            {
                commentaryTimer.Start();
            }
            // call back method for commentary loaded
            if (null != CommentaryLoaded && null != App.Current)
            {
                (App.Current as App).Dispatcher.BeginInvoke(CommentaryLoaded, null);
            }
        }

        

        /// <summary>
        /// Disposes all the objects in the view model
        /// </summary>
        protected override void DisposeResources()
        {
            this.CommentaryList = null;
            if (null != commentaryTimer)
            {
                commentaryTimer.Stop();
            }
            this.commentaryTimer = null;
            this.CommentaryLoaded = null;
        }
    }
}
