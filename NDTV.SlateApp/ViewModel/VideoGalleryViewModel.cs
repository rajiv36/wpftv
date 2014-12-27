using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using NDTV.Controller;
using NDTV.Entities;


namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// Video gallery view model.
    /// </summary>
    public class VideoGalleryViewModel : ViewModelBase
    {

        private VideoListCategories currentVideoCategory;
        private ObservableCollection<VideoItem> videoList;
        private ObservableCollection<VideoListCategories> videoCategoryList;
        private RelayCommand videoGalleryCommand;
        private bool isLoadingComplete;

        #region PROPERTIES

        /// <summary>
        /// Get or set video categories.
        /// </summary>
        public ObservableCollection<VideoListCategories> VideoCategories
        {
            get { return videoCategoryList; }
            private set
            {
                videoCategoryList = value;
                OnPropertyChanged("VideoCategoryList");
            }
        }

        /// <summary>
        /// Stores the video collection
        /// </summary>
        public ObservableCollection<VideoItem> VideoList
        {
            get { return videoList; }
            private set
            {
                videoList = value;
                OnPropertyChanged("VideoList");
            }
        }

        /// <summary>
        /// Get or Set the current video category
        /// </summary>
        public VideoListCategories CurrentVideoCategory
        {
            get
            {
                return currentVideoCategory;
            }
            set
            {
                if (null != value)
                {                 
                    currentVideoCategory = value;
                    currentVideoCategory.IsSelected = true;
                    OnPropertyChanged("CurrentVideoCategory");
                }
            }
        }

        /// <summary>
        /// Get or set the default video category
        /// </summary>
        public VideoListCategories DefaultVideoCategory
        {
            get;
            private set;
        }



        /// <summary>
        /// Determines if all videos are retrived for current category.
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
        /// Gets the Video gallery command
        /// </summary>
        public ICommand VideoGalleryCommand
        {
            get
            {
                if (null == videoGalleryCommand)
                {
                    videoGalleryCommand = new RelayCommand(ProcessRequestForCategory);
                }
                return videoGalleryCommand;
            }
        }

        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Default constructor . Raises request for the video
        /// </summary>
        public VideoGalleryViewModel()
        {
            Initialize();
            CurrentVideoCategory = DefaultVideoCategory;
        }

        /// <summary>
        /// Parameterized contructor, to load video list with selected video category
        /// </summary>
        /// <param name="videoCategory">
        /// Parameter videoCategory is of Type Enum VideoCategories.
        /// </param>
        public VideoGalleryViewModel(VideoListCategories videoCategory)
        {
            Initialize();
            if (null == videoCategory)
            {
                CurrentVideoCategory = DefaultVideoCategory;
            }
            else
            {
                CurrentVideoCategory = videoCategory;
            }
        }

        /// <summary>
        /// Parameterized contructor, to load video list with selected video category
        /// </summary>
        /// <param name="videoCategory">
        /// Parameter videoCategory is of type String.
        /// </param>
        public VideoGalleryViewModel(String videoCategory)
        {
            Initialize();
            CurrentVideoCategory = ConvertStringToVideoCategory(videoCategory);
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Convert string type video category to the object type.
        /// </summary>
        /// <param name="videoCategory"> String type video category</param>
        /// <returns> Returns Video List Category type object</returns>
        public VideoListCategories ConvertStringToVideoCategory(String videoCategory)        
        {
            VideoListCategories categoryObj = null;
            
            /* If parameter is null */

            if (String.IsNullOrEmpty(videoCategory))
            {
                categoryObj = DefaultVideoCategory;
            }
            else
            {
                /* Otherwise search object in category collections. */
                categoryObj = VideoCategories.FirstOrDefault(category => category.CategoryTitle.Equals(videoCategory));
                if (null == categoryObj)
                {
                    /* If not able to find video category return first category. */
                    categoryObj = DefaultVideoCategory;
                }
            }
            return categoryObj;
        }

        #endregion

        #region PRIVATE METHDOS

        /// <summary>
        /// Initialized the paramters and category list.
        /// </summary>
        private void Initialize()
        {
            VideoList = new ObservableCollection<VideoItem>();
            VideoCategories = new ObservableCollection<VideoListCategories>();

            /* Retrieve video categories from the Application Data. */
            foreach (VideoListCategories videoCategory in ApplicationData.RelatedVideoList)
            {
                /* Set category is selected to false for UI checkbox binding. */
                videoCategory.IsSelected = false;
                VideoCategories.Add(videoCategory);
            }
            DefaultVideoCategory = VideoCategories.FirstOrDefault();
        }

        /// <summary>
        /// Start requesting videos for category.
        /// </summary>
        /// <param name="videoCategoryObject"> The video category.</param>
        private void ProcessRequestForCategory(object videoCategoryObject)
        {
            IsLoadingComplete = false;
            RelatedVideosRequest request = new RelatedVideosRequest(((VideoListCategories)videoCategoryObject).Link);
            ProcessRequest(request, LoadLatestVideoGalleryResponse);
        }

        #endregion

        #region PROTECTED METHDOS

        /// <summary>
        /// Response for the video gallery request
        /// </summary>
        /// <param name="videoResponse"> Collection of video from the requested category.</param>
        protected void LoadLatestVideoGalleryResponse(Response videoResponse)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                if (null != videoResponse && null != VideoList && videoResponse.GetType() == typeof(RelatedVideosResponse))
                {
                    foreach (VideoItem item in (ObservableCollection<VideoItem>)((RelatedVideosResponse)videoResponse).RelatedVideoList)
                    {
                        VideoList.Add(item);
                    }
                }
            }));
            IsLoadingComplete = true;
        }

        /// <summary>
        /// Dispoing resources.
        /// </summary>        
        protected override void DisposeResources()
        {
            this.DefaultVideoCategory = null;
            this.CurrentVideoCategory = null;
            this.VideoList = null;
            this.VideoCategories = null;
        }
        
        #endregion
    }

}