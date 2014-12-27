using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.Utilities;
namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// Gets the current and upcoming show from the call back method and bind to the view
    /// </summary>
    public class ChannelScheduleViewModel:ViewModelBase
    {

        private RelayCommand channelScheduleCommand;
        private ChannelSchedule channelSchedule;
        private string currentShow;
        private string upcomingShow;
        private string videoLink;
        private ObservableCollection<VideoItem> videoList;

        #region Properties
        /// <summary>
        /// Video items for the video gallery
        /// </summary>
        public ObservableCollection<VideoItem> VideoList
        {
            get
            {
                return videoList;
            }
            set
            {
                videoList = value;
            }
        }
             
        /// <summary>
        /// Live TV Video URL 
        /// </summary>
        public string VideoLink
        {
            get { return videoLink; }
            set
            {
                videoLink = value;
                OnPropertyChanged("VideoURL");
            }
        }

        /// <summary>
        /// Upcoming Show in Live TV 
        /// </summary>
        public string UpcomingShow
        {
            get { return upcomingShow; }
            set
            {
                if (false == string.IsNullOrEmpty(value))
                {
                    upcomingShow = value;                    
                }
                else
                {
                    upcomingShow = Constants.Constant.NotAvailableMsg;
                }
                OnPropertyChanged("UpcomingShow");
            }
        }
        
        /// <summary>
        /// Program Schedule Data for Live TV
        /// </summary>
        public ChannelSchedule ChannelSchedule
        {
            get { return channelSchedule; }
            set
            {
                channelSchedule = value;
                OnPropertyChanged("ChannelSchedule");
            }
        }

        /// <summary>
        /// Current Show in Live TV
        /// </summary>
        public string CurrentShow
        {
            get { return currentShow; }
            set
            {
                if (false == string.IsNullOrEmpty(value))
                {                    
                    currentShow = value;
                }
                else
                {
                   currentShow = Constants.Constant.NotAvailableMsg;
                }
                OnPropertyChanged("CurrentShow");
            }
        }

        /// <summary>
        /// Loading request
        /// </summary>
        private bool isLoadingComplete;

        /// <summary>
        /// Loading request
        /// </summary>
        public bool IsLoadingComplete
        {
            get { return isLoadingComplete; }
            set
            {
                isLoadingComplete = value;
                OnPropertyChanged("IsLoadingComplete");
            }
        }

        /// <summary>
        /// Gets the channel schedule command
        /// </summary>
        public ICommand ChannelScheduleCommand
        {
            get 
            {
                if (null == channelScheduleCommand)
                {
                    channelScheduleCommand = new RelayCommand(GetCurrentChannel);
                }
                return channelScheduleCommand;
            }
            
        }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Constructor for Channel Schedule View Model; Initializes the command object for the channels
        /// </summary>
        /// <param name="currentChannel">current channel</param>
        public ChannelScheduleViewModel()
        {
            IsLoadingComplete = false;
            RelatedVideosRequest request = new RelatedVideosRequest(ApplicationData.BuildVideosLink(VideoCategories.Featured));
            ProcessRequest(request, LoadLatestVideoResponse);
            VideoList = new ObservableCollection<VideoItem>();
        }
       
        #endregion Constructor
       

        #region Private methods
        /// <summary>
        /// Raises the request to the channel schedule API for the schedule info
        /// </summary>
        /// <param name="currentChannel">current channel</param>
        private void GetCurrentChannel(object scheduleURL)
        {

            if (scheduleURL.ToString() == Constants.LinkNames.LiveTVNDTVHinduSchedule)
            {
                CurrentShow = Constants.Constant.NotAvailableMsg;
                UpcomingShow = Constants.Constant.NotAvailableMsg;
                
            }
            else
            {
                ProcessRequest(new ChannelScheduleRequest(Utility.GetLink((string)scheduleURL)), LoadChannelScheduleResponse,
                    new Action<Request>(delegate(Request request) 
                                         {
                                             CurrentShow = Constants.Constant.NotAvailableMsg;
                                             UpcomingShow = Constants.Constant.NotAvailableMsg;
                                         }), false);
            }
        }


        /// <summary>
        /// Call back method for the schedule. This returns the current and upcoming show from API
        /// </summary>
        /// <param name="response"> schedule response </param>
        private void LoadChannelScheduleResponse(Response response)
        {
            if (response.GetType() == typeof(ChannelScheduleResponse))
            {
                ChannelSchedule = ((ChannelScheduleResponse)response).ChannelScheduleDetails;
                CurrentShow = ChannelSchedule.CurrentShow;
                UpcomingShow = ChannelSchedule.UpcomingShow;
            }
        }

        /// <summary>
        /// Get the video gallery list
        /// </summary>
        /// <param name="videoResponse"> video response</param>
        private void LoadLatestVideoResponse(Response videoResponse)
        {
            if (videoResponse.GetType() == typeof(RelatedVideosResponse))
            {
                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    RelatedVideosResponse response = (RelatedVideosResponse)videoResponse;
                    foreach (VideoItem item in response.RelatedVideoList)
                    {
                        VideoList.Add(item);
                    }

                }), DispatcherPriority.Background);
            }
            IsLoadingComplete = true;
        }

        #endregion Private methods




        /// <summary>
        /// Disposing managed resources.
        /// </summary>
        protected override void DisposeResources()
        {
            VideoList = null;
            VideoLink = null;
            UpcomingShow = null;
            ChannelSchedule = null;
            CurrentShow = null;
            channelScheduleCommand = null;
        }

    }
}
