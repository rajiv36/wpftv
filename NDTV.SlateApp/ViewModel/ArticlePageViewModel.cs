using System.Collections.ObjectModel;
using NDTV.Controller;
using NDTV.Entities;

namespace NDTV.SlateApp.ViewModel
{
    public class ArticlePageViewModel:ViewModelBase
    {
        #region Properties

        /// <summary>
        /// TopStories Source in th form of Container which gets refreshed after scrolling..TopStoriesContainer1
        /// </summary>
        private ObservableCollection<TopStoryItem> topStoriesContainer1 = new ObservableCollection<TopStoryItem>();
        public ObservableCollection<TopStoryItem> TopStoriesContainer1
        {
            get 
            { 
                return topStoriesContainer1; 
            }
            set
            {
                if (null != topStoriesContainer1 && null != value && value.Equals(topStoriesContainer1))
                {
                    return;
                }
                topStoriesContainer1 = value;
                OnPropertyChanged("TopStoriesContainer1");
            }
        }

        /// <summary>
        /// A property used to set the isSelected Index , which is used to maintain the selected item in the source container
        /// </summary>
        private int selectedIndex = -1;
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        /// <summary>
        /// The TopStoryItem that is been selected
        /// </summary>
        private TopStoryItem topStoryActiveItem = new TopStoryItem();
        public TopStoryItem TopStoryActiveItem
        {
            get 
            { 
                return topStoryActiveItem; 
            }
            set
            {
                topStoryActiveItem = value;
                if (topStoryActiveItem != null)
                {
                    string imageLink = topStoryActiveItem.ImageLinkStatic == null? string.Empty : topStoryActiveItem.ImageLinkStatic.ToString();
                    ApplicationData.SetCurrentItem(topStoryActiveItem.Title, topStoryActiveItem.Description, topStoryActiveItem.Guid,
                        imageLink, topStoryActiveItem.LinkArticle.ToString(), ShareMediaType.Article);                    
                }
                OnPropertyChanged("TopStoryItem");
            }
        }

        /// <summary>
        /// The TopStory item which gets loaded either through an API call as mentioned above or else references the TopStory object created already..
        /// </summary>
        private TopStory topStories;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ArticlePageViewModel()
        {

        }

        /// <summary>
        /// The constructor called when we want the same TopStory to be referenced..
        /// </summary>
        /// <param name="topStoryItem">selected Article</param>
        /// <param name="topStoryCollection">Articles Collection</param>
        public ArticlePageViewModel(TopStoryItem topStoryItem,TopStory topStory)
        {            
            TopStoriesContainer1 = new ObservableCollection<TopStoryItem>(topStory.TopStoryCollection);
            topStories = topStory;
            TopStoryActiveItem = topStoryItem;
            if (TopStoriesContainer1.Contains(topStoryItem))
            {
                SelectedIndex = TopStoriesContainer1.IndexOf(topStoryItem);
            }
            else
            {
                SelectedIndex = -1;
            }
        }

        #endregion

        #region Public calls

        /// <summary>
        /// The method which sets the active TopStory item..
        /// </summary>
        /// <param name="topStoryItem">selected Article</param>
        public void ReflectArticlePageChanges(TopStoryItem topStoryItem)
        {
            TopStoryActiveItem = topStoryItem;
        }

        /// <summary>
        /// Disposes the objects in the view model
        /// </summary>
        protected override void DisposeResources()
        {            
            this.topStories = null;
            this.TopStoriesContainer1 = null;
            this.TopStoryActiveItem = null;
        }
        #endregion

    }
}
