using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.ViewModel;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using SlateAppProperties=NDTV.SlateApp.Properties;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for SearchScreen.xaml
    /// </summary>
    public partial class SearchScreen : SlateWindow
    {
        /// <summary>
        /// Search View Model instance.
        /// </summary>
        private SearchScreenViewModel searchViewModel;

        /// <summary>
        /// The previous string
        /// </summary>
        private string previousSearchString;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchScreen(string firstSearchString)
        {
            InitializeComponent();
            if (null != firstSearchString)
            {
                this.SearchTextBox.Text = firstSearchString;
                this.previousSearchString = firstSearchString;
            }
            this.searchViewModel = new SearchScreenViewModel();
            this.DataContext = this.searchViewModel;
            this.ResultsContainer.Visibility = System.Windows.Visibility.Hidden;
            this.SubHeaderTextBlock.Visibility = System.Windows.Visibility.Hidden;

            if (ApplicationData.IsApplicationOnline)
            {
                this.PerformSearchOperations();
            }

            this.SearchButton.Click += (s, args) =>
            {
                if (ApplicationData.IsApplicationOnline)
                {
                    if (false == string.IsNullOrWhiteSpace(this.SearchTextBox.Text) && this.SearchTextBox.Text!=this.previousSearchString)
                    {
                        this.PerformSearchOperations();
                    }
                }
                else
                {
                    (App.Current as App).DisplayErrorMessage(SlateAppProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
                }
            };

            this.SearchTextBox.KeyDown += (s, args) =>
            {
                switch (args.Key)
                {
                    case System.Windows.Input.Key.Enter:
                        if (ApplicationData.IsApplicationOnline)
                        {
                            if (false == string.IsNullOrWhiteSpace(this.SearchTextBox.Text) && this.SearchTextBox.Text != this.previousSearchString)
                            {
                                this.PerformSearchOperations();
                            }
                        }
                        else
                        {
                            (App.Current as App).DisplayErrorMessage(SlateAppProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
                        }
                        break;
                }
            };
            
            this.VideosList.SelectionChanged += (s, args) =>
            {
                if (this.VideosList.SelectedIndex != -1)
                {
                    var selectedVideoItem = this.VideosList.SelectedItem as VideoItem;
                    if (null != selectedVideoItem)
                    {
                        OpenPopupWindow(new VideoPlayer(selectedVideoItem), this, ModalBG,null);
                    }
                }
                this.VideosList.SelectedIndex = -1;
            };

            this.PhotosList.SelectionChanged += (s, args) =>
            {
                if (this.PhotosList.SelectedIndex != -1)
                {
                    var selectedPhotoItem = (this.PhotosList.SelectedItem as ImageAlbum);
                    if (null != selectedPhotoItem)
                    {
                        OpenPopupWindow(new ImageAlbumGalleryWindow(selectedPhotoItem, this.searchViewModel.PhotoSearchResults), this, ModalBG,null);
                    }
                }
                this.PhotosList.SelectedIndex = -1;
            };

            this.NewsArticlesList.SelectionChanged += (s, args) =>
            {
                if (this.NewsArticlesList.SelectedIndex != -1)
                {
                    TopStory topStoryCollection = new TopStory();
                    topStoryCollection.TopStoryCollection = new TopStoryCollection();
                    if (searchViewModel.NewsSearchResults.Count > 0)
                    {
                        foreach (var item in searchViewModel.NewsSearchResults)
                        {
                            topStoryCollection.TopStoryCollection.Add(item);
                        }
                        OpenPopupWindow(new Article((this.NewsArticlesList.SelectedItem as TopStoryItem), topStoryCollection), this, ModalBG,null);
                    }
                }
                this.NewsArticlesList.SelectedIndex = -1;
            };

            this.PortraitVideosList.SelectionChanged += (s, args) =>
            {
                if (this.PortraitVideosList.SelectedIndex != -1)
                {
                    var selectedVideoItem = this.PortraitVideosList.SelectedItem as VideoItem;
                    if (null != selectedVideoItem)
                    {
                        OpenPopupWindow(new VideoPlayer(selectedVideoItem), this, ModalBG,null);
                    }
                }
                this.PortraitVideosList.SelectedIndex = -1;
            };

            this.PortraitPhotosList.SelectionChanged += (s, args) =>
            {
                if (this.PortraitPhotosList.SelectedIndex != -1)
                {
                    var selectedPhotoItem = (this.PortraitPhotosList.SelectedItem as ImageAlbum);
                    if (null != selectedPhotoItem)
                    {
                        OpenPopupWindow(new ImageAlbumGalleryWindow(selectedPhotoItem, this.searchViewModel.PhotoSearchResults), this, ModalBG,null);
                    }
                }
                this.PortraitPhotosList.SelectedIndex = -1;
            };

            this.PortraitNewsArticlesList.SelectionChanged += (s, args) =>
            {
                if (this.PortraitNewsArticlesList.SelectedIndex != -1)
                {
                    TopStory topStoryCollection = new TopStory();
                    topStoryCollection.TopStoryCollection = new TopStoryCollection();
                    if (searchViewModel.NewsSearchResults.Count > 0)
                    {
                        foreach (var item in searchViewModel.NewsSearchResults)
                        {
                            topStoryCollection.TopStoryCollection.Add(item);
                        }
                        OpenPopupWindow(new Article((this.PortraitNewsArticlesList.SelectedItem as TopStoryItem), topStoryCollection), this, ModalBG,null);
                    }
                }
                this.PortraitNewsArticlesList.SelectedIndex = -1;
            };

            this.searchViewModel.SearchCompletedEvent += (sender, args) =>
            {
                switch (args.SearchKind)
                {
                    case SearchType.Videos:
                        this.searchViewModel.IsVideoContainerBusy = false;
                        if (args.SearchResults.Count <= 0)
                        {
                            this.EmptyVideosListContentControl.Visibility = System.Windows.Visibility.Visible;
                            this.PortraitEmptyVideosListContentControl.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            this.VideosList.ScrollIntoView(this.VideosList.Items[0]);
                            this.VideosList.SelectedIndex = -1;
                            this.EmptyVideosListContentControl.Visibility = System.Windows.Visibility.Hidden;
                            this.PortraitEmptyVideosListContentControl.Visibility = System.Windows.Visibility.Hidden;
                        }
                        break;
                    case SearchType.Photos:
                        this.searchViewModel.IsPhotoContainerBusy = false;
                        if (args.SearchResults.Count <= 0)
                        {
                            this.EmptyPhotosListContentControl.Visibility = System.Windows.Visibility.Visible;
                            this.PortraitEmptyPhotosListContentControl.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            this.PhotosList.ScrollIntoView(this.PhotosList.Items[0]);
                            this.PhotosList.SelectedIndex = -1;
                            this.EmptyPhotosListContentControl.Visibility = System.Windows.Visibility.Hidden;
                            this.PortraitEmptyPhotosListContentControl.Visibility = System.Windows.Visibility.Hidden;
                        }
                        break;
                    case SearchType.Articles:
                        this.searchViewModel.IsNewsArticleContainerBusy = false;
                        if (args.SearchResults.Count <= 0)
                        {
                            this.EmptyArticlesListContentControl.Visibility = System.Windows.Visibility.Visible;
                            this.PortraitEmptyArticlesListContentControl.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            this.NewsArticlesList.ScrollIntoView(this.NewsArticlesList.Items[0]);
                            this.NewsArticlesList.SelectedIndex = -1;
                            this.EmptyArticlesListContentControl.Visibility = System.Windows.Visibility.Hidden;
                            this.PortraitEmptyArticlesListContentControl.Visibility = System.Windows.Visibility.Hidden;
                        }
                        break;
                }
            };

            this.CloseButton.Click += (s, args) =>
            {
                this.Dispose();
                this.Close();
            };
        }

        /// <summary>
        /// Overriding the method where in the grids are Visibility controlled here..
        /// Potriat Container is hidden and Landscape Container is brought to focus
        /// </summary>
        protected override void SwitchToLandscapeView()
        {
            LandscapeView();
            this.SetWindowPosition();
        }

        /// <summary>
        ///  Overriding the method where in the grids are Visibility controlled here..
        ///  Landscape Container is hidden and Potrait Container is brought to focus
        /// </summary>
        protected override void SwitchToPortraitView()
        {
            PortraitView();
            this.SetWindowPosition();
        }

        /// <summary>
        /// The overriden method which sets the size of the window based on whether it is in landscape or potriat version
        /// </summary>
        protected override void SetSize()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                this.SetWindowPosition();
                LandscapeView();
            }
            else
            {
                this.SetWindowPosition();
                PortraitView();
            }
        }

        /// <summary>
        /// A Method which sets the window to Landscape orientation with all the required properties
        /// </summary>
        private void LandscapeView()
        {
            this.Height = 550;
            this.Width = 950;
            MainContainer.Opacity = 0;
            SearchBoxBorder.HorizontalAlignment = HorizontalAlignment.Center;
            SubHeaderTextBlock.SetValue(Grid.RowProperty, 0);
            Storyboard portraitToLandscapeAnimation = (Storyboard)TryFindResource("PotraitLandscapeAnimation");
            portraitToLandscapeAnimation.Begin();

            if (null != PortraitResultsContainer)
            {
                PortraitResultsContainer.Visibility = Visibility.Collapsed;
            }
            if (null != LandscapeResultsContainer)
            {
                LandscapeResultsContainer.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// A Method which sets the window to Potrait orientation with all the required properties
        /// </summary>
        private void PortraitView()
        {
            this.Height = 950;
            this.Width = 550;
            MainContainer.Opacity = 0;
            SearchBoxBorder.HorizontalAlignment = HorizontalAlignment.Left;
            SearchBoxBorder.Margin = new Thickness(15, 5, 0, 0);
            SubHeaderTextBlock.SetValue(Grid.RowProperty, 1);
            Storyboard landscapeToPortraitAnimation = (Storyboard)TryFindResource("PotraitLandscapeAnimation");
            landscapeToPortraitAnimation.Begin();

            if (null != LandscapeResultsContainer)
            {
                LandscapeResultsContainer.Visibility = Visibility.Collapsed;
            }
            if (null != PortraitResultsContainer)
            {
               PortraitResultsContainer.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Sets the window position calculating the resolution of the window.
        /// </summary>
        private void SetWindowPosition()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            Left = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
            Top = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;
        }

        /// <summary>
        /// Method to extract the Scroll Viewer from within the ListBox.
        /// </summary>
        /// <param name="activeListBox">The listbox from which the ScrollViewer has to be extracted</param>
        /// <returns>The ScrollViewer (if found)</returns>
        private static SurfaceScrollViewer FindScroll(SurfaceListBox activeListBox)
        {
            Grid scrollGrid = VisualTreeHelper.GetChild(activeListBox, 0) as Grid;
            if (scrollGrid is Grid)
            {
                SurfaceScrollViewer activeScrollViewer = scrollGrid.Children[0] as SurfaceScrollViewer;
                if (activeScrollViewer is SurfaceScrollViewer)
                {
                    return activeScrollViewer;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// On Surface List Box scroll value changed.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void OnSurfaceListBoxScrollValueChanged(object sender, ScrollChangedEventArgs e)
        {
            var listBoxScrollViewer = FindScroll((sender as SurfaceListBox));

            SearchType currentSearchType = SearchType.Articles;

            if (null != listBoxScrollViewer)
            {
                if (listBoxScrollViewer.ScrollableWidth != 0)
                {
                    if (e.HorizontalOffset == listBoxScrollViewer.ScrollableWidth)
                    {
                        switch ((sender as SurfaceListBox).Name)
                        {
                            case "PhotosList":
                                currentSearchType = SearchType.Photos; break;
                            case "VideosList":
                                currentSearchType = SearchType.Videos; break;
                        }
                        this.searchViewModel.FetchMoreDataCommand.Execute(currentSearchType);
                    }
                }
                else if (listBoxScrollViewer.ScrollableHeight != 0)
                {
                    if (e.VerticalOffset == listBoxScrollViewer.ScrollableHeight)
                    {
                        switch ((sender as SurfaceListBox).Name)
                        {
                            case "PortraitPhotosList":
                                currentSearchType = SearchType.Photos; break;
                            case "PortraitVideosList":
                                currentSearchType = SearchType.Videos; break;
                            case "NewsArticlesList":
                            case "PortraitNewsArticlesList":
                                currentSearchType = SearchType.Articles; break;
                        }
                        this.searchViewModel.FetchMoreDataCommand.Execute(currentSearchType);
                    }
                }
            }
        }

        /// <summary>
        /// Function to perform Search Operations.
        /// </summary>
        private void PerformSearchOperations()
        {
            this.EmptyVideosListContentControl.Visibility = System.Windows.Visibility.Hidden;
            this.EmptyPhotosListContentControl.Visibility = System.Windows.Visibility.Hidden;
            this.EmptyArticlesListContentControl.Visibility = System.Windows.Visibility.Hidden;
            this.PortraitEmptyArticlesListContentControl.Visibility = System.Windows.Visibility.Hidden;
            this.PortraitEmptyPhotosListContentControl.Visibility = System.Windows.Visibility.Hidden;
            this.PortraitEmptyVideosListContentControl.Visibility = System.Windows.Visibility.Hidden;
            (this.searchViewModel).OnSearchButtonClickCommand.Execute(this.SearchTextBox.Text);
            this.ResultsContainer.Visibility = System.Windows.Visibility.Visible;
            this.SubHeaderTextBlock.Visibility = System.Windows.Visibility.Visible;
            previousSearchString = this.SearchTextBox.Text;
        }

        /// <summary>
        /// Dispose Method
        /// </summary>
        protected override void DisposeResources()
        {
            base.DisposeResources();
        }
    }
}
