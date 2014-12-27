using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using NDTV.Controller;
using NDTV.Entities;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// Contains items for related news and related videos
    /// </summary>
    public class RelatedItemsViewModel : ViewModelBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RelatedItemsViewModel(Action relatedItemsLoaded)
        {
            this.ItemsLoaded = relatedItemsLoaded;
            LoadData();
        }

        private ObservableCollection<VideoItem> videoList;
        private VideoPageViewModel sportsVideos;
        private TopStory cricketNews;
        private bool isBusy;

        /// <summary>
        /// Call back for related videos and news response loaded
        /// </summary>
        private Action ItemsLoaded;

        /// <summary>
        /// Check wether loading is in progress
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }
        
        /// <summary>
        /// Collection containd the list of videos
        /// </summary>
        public ObservableCollection<VideoItem> VideoList
        {
            get { return videoList; }
            set
            {
                videoList = value;
                OnPropertyChanged("VideoList");
            }
        }

        /// <summary>
        /// Related News for cricket
        /// </summary>
        public TopStory CricketNews
        {
            get
            {
                return cricketNews;
            }
            set
            {
                cricketNews = value;
                OnPropertyChanged("CricketNews");
            }
        }

        /// <summary>
        /// Load data
        /// </summary>
        private void LoadData()
        {
            IsBusy = true;
            sportsVideos = new VideoPageViewModel(VideoCategories.Sports,VideoLoadCompleted);         
        }

        /// <summary>
        /// Call back method when the video list is loaded
        /// </summary>
        private void VideoLoadCompleted()
        {
            VideoList = sportsVideos.VideoList;
            if (null != ApplicationData.CricketCategoryItem)
            {
                ProcessRequest(new TopStoryFeedRequest(ApplicationData.CricketCategoryItem), LoadTopStoryFeedResponse);
            }
        }

        private void LoadTopStoryFeedResponse(Response response)
        {
            CricketNews = ((TopStoryFeedResponse)response).TopStory;
            IsBusy = false;
            if (null != ItemsLoaded)
            {
                (App.Current as App).Dispatcher.BeginInvoke(DispatcherPriority.Background, ItemsLoaded);
            }
        }

        /// <summary>
        /// Disposes all the objects in the view models
        /// </summary>
        protected override void DisposeResources()
        {            
            
        }
    }
}
