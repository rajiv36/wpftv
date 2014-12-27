using System;
using System.Windows;
using System.Windows.Controls;
using NDTV.SlateApp.View;
using NDTV.SlateApp.ViewModel;
using NDTV.Controller;
using System.Collections.ObjectModel;
using SlateProperties = NDTV.SlateApp.Properties;
using Entities = NDTV.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NDTV.SlateApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SlateWindow
    {
        /// <summary>
        /// private property that is used for setting  the context 
        /// </summary>
        private LandingPageViewModel landingPage;
        
        /// <summary>
        /// Constructor that gets invoked from the App.xaml
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            if (null == landingPage)
            {
                landingPage = new LandingPageViewModel();
            }
            this.DataContext = landingPage;
            menuItem.Click += new RoutedEventHandler(MenuItemClicked);

            this.LiveTVButton.Click += (s, args) =>
            {
                OpenPopupWindow(new LiveTVView(), this, ModalBG, HandleAdBannerVisibility);
            };

            this.stock.Click += (s, args) =>
            {
                OpenPopupWindow(new SensexScorecard((landingPage).StockTickerDetails), this, ModalBG, HandleAdBannerVisibility);
            };

            this.PotraitStock.Click += (s,args) =>
            {
                OpenPopupWindow(new SensexScorecard((landingPage).StockTickerDetails), this, ModalBG, HandleAdBannerVisibility);
            };
 
            this.CricketTicketButton.Click += (s, args) =>
            {
                CricketButtonClicked();
            };

            this.PotraitCricketTicketButton.Click += (s, args) =>
            {
                CricketButtonClicked();
            };

            this.LandscapeSearchBox.SearchStarted += (s, args) =>
            {
                OpenPopupWindow(new SearchScreen(args.SearchString), this, ModalBG, HandleAdBannerVisibility);
            };
            this.PortraitSearchBox.SearchStarted += (s, args) =>
            {
                OpenPopupWindow(new SearchScreen(args.SearchString), this, ModalBG, HandleAdBannerVisibility);
            };

            this.PortraitLiveTVButton.Click += (s, args) =>
            {
                OpenPopupWindow(new LiveTVView(), this, ModalBG, HandleAdBannerVisibility);
            };
            SetOrientationOnLoad();
        }

        /// <summary>
        /// Cricket ticker clicked event handler
        /// </summary>
        private void CricketButtonClicked()
        {
            string matchId = null;

            if (null != landingPage)
            {
                if (true == this.landingPage.IsMatchLive)
                {
                    if (null != landingPage.LiveScorecards)
                    {
                        if (landingPage.LiveScorecards.Count > 0 && landingPage.LiveMatchSelectedScorecard < landingPage.LiveScorecards.Count)
                        {
                            if (landingPage.LiveScorecards[landingPage.LiveMatchSelectedScorecard].IsLive.Equals("1"))
                            {
                                matchId = (landingPage.LiveScorecards[landingPage.LiveMatchSelectedScorecard].Id);
                            }
                        }
                    }
                }
                else
                {
                    if (null != landingPage.RecentScorecards)
                    {
                        if (landingPage.RecentScorecards.Count > 0 && landingPage.RecentMatchSelectedScorecard < this.landingPage.RecentScorecards.Count)
                        {
                            matchId = null;
                        }
                    }
                }
                OpenPopupWindow(new CricketWindow(matchId), this, ModalBG, HandleAdBannerVisibility);
            }
        }

        /// <summary>
        /// Event thats set in the UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ArticleClick(object sender, EventArgs args)
        {
            OpenPopupWindow(new Article((Entities.TopStoryItem)(((FrameworkElement)sender).DataContext), landingPage.TopStories),
                this, ModalBG, HandleAdBannerVisibility);
        }

        /// <summary>
        /// A general event which gets invoked from the UI executes that method that is bound through the command parameter..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MenuItemClicked(object sender, RoutedEventArgs args)
        {
            args.Handled = true; // so that this routedEvent doesnt invoke this instance repeatedly..
            if (ApplicationData.IsApplicationOnline)
            {
                if (null != args && null != landingPage)
                {
                    if (landingPage.IsCategoryFromNDTVSource((args.OriginalSource as FrameworkElement).DataContext))
                    {
                        landingPage.SwitchPreloaderOn();
                        menuItem.Command.Execute(args.OriginalSource);
                        ScrollBackToStart();
                        RefreshAdBanners();
                        //photosVideosScroll.ScrollToHome();//The scrollbar associated with the Images and Videos on a whole changed..
                    }
                    else
                    {
                        switch (landingPage.CategoryType((args.OriginalSource as FrameworkElement).DataContext).ToLowerInvariant())
                        {
                            case "videos":
                                {
                                    OpenPopupWindow(new VideoGallery(), this, ModalBG, HandleAdBannerVisibility);
                                    break;
                                }
                            case "photos":
                                {
                                    OpenPopupWindow(new ImageAlbumCarouselWindow(), this, ModalBG, HandleAdBannerVisibility);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }

        }

        /// <summary>
        /// flag used to make the event ThreadSafe, or else the same event will be hit many times
        /// </summary>
        bool flag_Scroll = false;

        /// <summary>
        /// Scroll Viewer handler which gets fired when the user has traversed 75% of the 
        /// Scroll portion
        /// </summary>
        /// <param name="sender">Scrollviewer</param>
        /// <param name="e">Positions of the scroll when the event was invoked</param>
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer source = (ScrollViewer)sender;
            if (Convert.ToInt32(source.ScrollableHeight) == 0) return;
            if (((e.VerticalOffset + e.ViewportHeight) / (source.ScrollableHeight + e.ViewportHeight)) < 0.95)
                flag_Scroll = false;
            if (false == flag_Scroll && ((e.VerticalOffset + e.ViewportHeight) / (source.ScrollableHeight + e.ViewportHeight)) >= 0.95)
            {
                flag_Scroll = true;
                landingPage.LandingPageTopStoryItemsRelayCommander.Execute(sender);
            }
        }

        /// <summary>
        /// Dispose of the MainWindow object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CloseMainPage(object sender, EventArgs args)
        {
            //landingPage.Dispose();
            this.Close();
        }

        /// <summary>
        /// Open video gallery
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVideosClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenPopupWindow(new VideoGallery(), this, ModalBG, HandleAdBannerVisibility);
        }

        /// <summary>
        /// Opens Video Player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRelatedVideosClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenPopupWindow(new VideoPlayer((((FrameworkElement)e.OriginalSource).DataContext as NDTV.Entities.VideoItem)),
                this,
                ModalBG, HandleAdBannerVisibility);
        }

        /// <summary>
        /// When the Refresh button is clicked 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnRefreshClicked(object sender, EventArgs args)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                landingPage.SwitchPreloaderOn();
                ScrollBackToStart();
                RefreshAdBanners();
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// A method which scrolls all the scrollViewers to the start
        /// </summary>
        private void ScrollBackToStart()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                topStoryScroll.ScrollToHome();
                videosScroll.ScrollToHome();
                photosScroll.ScrollToHome();
            }
            else
            {
                PotraittopStoryScroll.ScrollToHome();
                PotraitVideoScroll.ScrollToHome();
                PotraitImageScroll.ScrollToHome();
            }
        }

        /// <summary>
        /// This method handles the ad banner visibility
        /// </summary>
        /// <param name="isVisible">Flag denoting whether the ad banner is visible or not</param>
        private void HandleAdBannerVisibility(bool isVisible)
        {
            //LandscapeAdBanner.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;//handled at the AdBanner control
            //PortraitAdBanner.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;

            LandscapeAdBanner.ShowAdvertisement = isVisible;
            PortraitAdBanner.ShowAdvertisement = isVisible;
        }

        /// <summary>
        /// Image Carousel View Event Handled on click of the button
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event Args</param>
        private void ImageCarouselViewClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenPopupWindow(new ImageAlbumCarouselWindow(), this, ModalBG, HandleAdBannerVisibility);
        }

        /// <summary>
        /// Image Gallery image of the clicked Image.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Event args.</param>
        private void ImageAlbumClick(object sender, System.Windows.Input.MouseButtonEventArgs args)
        {
            OpenPopupWindow(new ImageAlbumGalleryWindow((((FrameworkElement)args.OriginalSource).DataContext as NDTV.Entities.ImageAlbum), new ObservableCollection<NDTV.Entities.ImageAlbum>(landingPage.PhotoAlbums)),
                this,
                ModalBG, HandleAdBannerVisibility);
        }

        /// <summary>
        /// Overriding the method where in the grids are Visibility controlled here..
        /// Potriat Container is hidden and Landscape Container is brought to focus
        /// </summary>
        protected override void SwitchToLandscapeView()
        {
            LandscapeView();
            SetWindowPosition();
            RefreshAdBanners();
        }

        /// <summary>
        ///  Overriding the method where in the grids are Visibility controlled here..
        ///  Landscape Container is hidden and Potriat Container is brought to focus
        /// </summary>
        protected override void SwitchToPortraitView()
        {
            PortraitView();
            SetWindowPosition();
            RefreshAdBanners();
        }

        /// <summary>
        /// Method called on load of the page initially
        /// </summary>
        private void SetOrientationOnLoad()
        {
            if (true == ApplicationData.IsLandscapeOrientation) 
            { 
                SwitchToLandscapeView(); 
            } 
            else 
            { 
                SwitchToPortraitView(); 
            }
        }

        /// <summary>
        /// Click event on articles surface button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void OnArticlesSurfaceButtonClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (((FrameworkElement)sender).DataContext.GetType() == typeof(Entities.TopStoryItem))
            {
                OpenPopupWindow(new Article((Entities.TopStoryItem)(((FrameworkElement)sender).DataContext), landingPage.TopStories),
                    this, ModalBG, HandleAdBannerVisibility);
            }
        }

        /// <summary>
        /// Click event on related videos surface button 
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void OnRelatedVideosSurfaceButtonClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (((FrameworkElement)sender).DataContext.GetType() == typeof(Entities.VideoItem))
            {
                OpenPopupWindow(new VideoPlayer((((FrameworkElement)e.OriginalSource).DataContext as NDTV.Entities.VideoItem)),
                   this,
                   ModalBG, HandleAdBannerVisibility);
            }
        }

        /// <summary>
        /// Click event on album item surface button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void AlbumItemSurfaceButtonClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (((FrameworkElement)sender).DataContext.GetType() == typeof(Entities.ImageAlbum))
            {
                OpenPopupWindow(new ImageAlbumGalleryWindow((((FrameworkElement)e.OriginalSource).DataContext as NDTV.Entities.ImageAlbum), new ObservableCollection<NDTV.Entities.ImageAlbum>(landingPage.PhotoAlbums)),
                   this,
                   ModalBG, HandleAdBannerVisibility);
            }
        }

        /// <summary>
        /// A private method which refreshes the AdBanners
        /// </summary>
        private void RefreshAdBanners()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                LandscapeAdBanner.RefreshAdBanner();
            }
            else
            {
                PortraitAdBanner.RefreshAdBanner();
            }
        }

        /// <summary>
        /// A Method which sets the window to Landscape orientation with all the required properties
        /// </summary>
        private void LandscapeView()
        {
            Width = 1024;
            Height = 600;
            if (null != PotraitContainer)
            {
                PotraitContainer.Visibility = Visibility.Collapsed;
            }
            if (null != LandscapeContainer)
            {
                LandscapeContainer.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// A Method which sets the window to Potriat orientation with all the required properties
        /// </summary>
        private void PortraitView()
        {
            Height = 1024;
            Width = 600;
            if (null != LandscapeContainer)
            {
                LandscapeContainer.Visibility = Visibility.Collapsed;
            }
            if (null != PotraitContainer)
            {
                PotraitContainer.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Sets the window posistion calculating the resolution of the window.
        /// </summary>
        private void SetWindowPosition()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Left = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
            Top = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;
        }

        /// <summary>
        /// Onclick of Weather button
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e">click arguments</param>
        private void OnWeatherButtonClick(object sender, RoutedEventArgs e)
        {
            OpenPopupWindow(new Weather(), this, ModalBG, HandleAdBannerVisibility);
        }        

        /// <summary>
        /// Click event of the settings button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
            OpenPopupWindow(new SettingsScreen(), this, ModalBG, HandleAdBannerVisibility);
            landingPage.RefreshTickerData();
        }
    }
}
