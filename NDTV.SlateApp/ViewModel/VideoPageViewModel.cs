using System.Collections.ObjectModel;
using NDTV.Controller;
using NDTV.Entities;
using System;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// View Model for videos related to cricket
    /// </summary>
    public class VideoPageViewModel : ViewModelBase
    {
        private ObservableCollection<VideoItem> videoList;

        /// <summary>
        /// Call back method when the video response is loaded
        /// </summary>
        private Action videoListLoaded;

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
        /// Default constructor
        /// </summary>
        public VideoPageViewModel(VideoCategories category,Action videoLoadedCallback)
        {
            videoListLoaded = videoLoadedCallback;
            RelatedVideosRequest request = new RelatedVideosRequest(ApplicationData.BuildVideosLink(category));
            ProcessRequest(request, LoadResponse);
            IsLoadingComplete = false;
        }

        /// <summary>
        /// Loads the content after it has been retrieved
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="eventArgs">Controller event args</param>
        protected void LoadResponse(Response videoResponse)
        {
            if (videoResponse.GetType() == typeof(RelatedVideosResponse))
            {
                VideoList = (ObservableCollection<VideoItem>)((RelatedVideosResponse)videoResponse).RelatedVideoList;
            }
            if (videoListLoaded != null)
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(videoListLoaded, null);
            }
            IsLoadingComplete = true;
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        protected override void DisposeResources()
        {
            VideoList = null;
        }
    }
}
