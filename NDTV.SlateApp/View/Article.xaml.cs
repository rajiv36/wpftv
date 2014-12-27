using System;
using System.Windows;
using System.Windows.Controls;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.ViewModel;
using SlateProperties = NDTV.SlateApp.Properties;
using SlateUtility = NDTV.SlateApp.Framework.Utilities;

namespace NDTV.SlateApp.View
{
	/// <summary>
	/// Interaction logic for article.xaml
	/// </summary>
    public partial class Article : SlateWindow
	{

        private bool isSizeSet = false;

        /// <summary>
        /// the main data context property of the xaml file
        /// </summary>
        private ArticlePageViewModel articleViewModel;

        /// <summary>
        /// Constructor Default
        /// </summary>
        public Article()
        {            
        }

        /// <summary>
        /// Event handler indicating changed in pop up visibility.
        /// </summary>
        /// <param name="sender">Application Data</param>
        /// <param name="e">Event arguments</param>
        private void IsPopUpOpenValueChanged(object sender, EventArgs e)
        {
            if (ApplicationData.IsPopUpOpen)
            {
                ModalPopup.Visibility = Visibility.Visible;
                browserControl.Visibility = Visibility.Collapsed;
                browserControlPotriat.Visibility = Visibility.Collapsed;
                adBannerPotriat.ShowAdvertisement = false;
                adBanner.ShowAdvertisement = false;
            }
            else
            {
                ModalPopup.Visibility = Visibility.Collapsed;
                browserControl.Visibility = Visibility.Visible;
                browserControlPotriat.Visibility = Visibility.Visible;
                adBannerPotriat.ShowAdvertisement = true;
                adBanner.ShowAdvertisement = true;
            }
        }

        /// <summary>
        /// Constructor that gets the property articleViewModel loaded
        /// </summary>
        /// <param name="item">item selected</param>
        /// <param name="topStory">the entire collection</param>
        public Article(TopStoryItem item, TopStory topStory)
		{
			this.InitializeComponent();

            ApplicationData.IsPopUpOpenValueChanged += IsPopUpOpenValueChanged;

			// Insert code required on object creation below this point.           
            if (null == articleViewModel)
            {
                articleViewModel = new ArticlePageViewModel(item, topStory);
            }
            
            //BrowserLoad(articleViewModel.TopStoryActiveItem.LinkForSlate);//No need of an explicit call as this is handled in the ArticleClick which is called automatically on bind..
            PreloaderBrowserVisibilityRefresh(true);
            this.DataContext = articleViewModel;
            SetSize();
            isSizeSet = true;
		}

        /// <summary>
        /// Closing the Window
        /// </summary>
        /// <param name="sender">button which got clicked</param>
        /// <param name="args">arguments sent in default by the button</param>
        private void CloseArticle(object sender, EventArgs args)
        {
            ApplicationData.IsPopUpOpenValueChanged -= IsPopUpOpenValueChanged;
            this.Close();
            articleViewModel.Dispose();
            browserControl.Dispose();
            browserControlPotriat.Dispose();
        }

        /// <summary>
        /// Click of the Article on the left hand pane automatically removes the respective Article from the List and considers it read
        /// The same method is called from both the Landscape and the Potriat Version of it.
        /// </summary>
        /// <param name="sender">button which got clicked</param>
        /// <param name="args">arguments sent in default by the button</param>
        private void ArticleClick(object sender, SelectionChangedEventArgs args)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                //TO DO: still under construction , to decide whether we need the TAGS or not.                
                if (null != sender)
                {
                    FrameworkElement frameworkElement = sender as FrameworkElement;
                    if (
                         (null != frameworkElement.Tag && frameworkElement.Tag.ToString() == Portrait && false == ApplicationData.IsLandscapeOrientation) ||
                         (null != frameworkElement.Tag && frameworkElement.Tag.ToString() == Landscape && ApplicationData.IsLandscapeOrientation)
                       )
                    {
                        var articleListBox = args.OriginalSource as ListBox;
                        articleViewModel.SelectedIndex = articleListBox.SelectedIndex;
                        PreloaderBrowserVisibilityRefresh(true);
                        if (null != args && null != args.OriginalSource && null != ((ListBox)(args.OriginalSource)).SelectedItem)
                        {
                            articleViewModel.ReflectArticlePageChanges((Entities.TopStoryItem)articleListBox.SelectedItem);
                            BrowserLoad(articleViewModel.TopStoryActiveItem.LinkForSlate);
                        }
                        RefreshAd();
                        articleListBox.ScrollIntoView(articleListBox.SelectedItem);
                    }
                }
            }
            else
            {
                if (articleViewModel.SelectedIndex != ((ListBox)(args.OriginalSource)).SelectedIndex)
                {
                    (App.Current as App).DisplayErrorMessage(SlateProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
                    //This is done to ensure that the selected item style doesn't apply to the new item when application is offline
                    articleViewModel.SelectedIndex = articleViewModel.SelectedIndex;               
                }                                                
            }

            args.Handled = true;
            //articleViewModel.SelectedIndex = ((ListBox)(args.OriginalSource)).SelectedIndex;//No need of this as ListBox takes care of selectedIndex automatically..
        }     
      
        /// <summary>
        /// Fired after the html is rendered to the UX..
        /// </summary>
        /// <param name="sender">webbrowser on the window</param>
        /// <param name="e">event raised by the browser after the entire html DOM is been loaded</param>
        private void BrowserControlLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            PreloaderBrowserVisibilityRefresh(false);
        }

        /// <summary>
        /// Refresh Advertisements Banner on this page
        /// </summary>
        private void RefreshAd()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                adBanner.RefreshAdBanner();
            }
            else
            {
                adBannerPotriat.RefreshAdBanner();
            }
        }

        /// <summary>
        /// Converts the Visibility of the Preloader and the Browser simultaneouly
        /// </summary>
        /// <param name="showPreloader">true if preloader has to be shown</param>
        private void PreloaderBrowserVisibilityRefresh(bool showPreloader)
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                if (showPreloader)
                {
                    preLoader.Visibility = Visibility.Visible;
                    browserControl.Visibility = Visibility.Collapsed;
                }
                else
                {
                    browserControl.Visibility = Visibility.Visible;
                    preLoader.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (showPreloader)
                {
                    preLoaderPotriat.Visibility = Visibility.Visible;
                    browserControlPotriat.Visibility = Visibility.Collapsed;
                }
                else
                {
                    browserControlPotriat.Visibility = Visibility.Visible;
                    preLoaderPotriat.Visibility = Visibility.Collapsed;
                }
            }

        }

        /// <summary>
        /// load the html on to the browser
        /// </summary>
        /// <param name="link">associated URI</param>
        private void BrowserLoad(Uri link)
        {
            if (null != link)
            {
                if (ApplicationData.IsLandscapeOrientation)
                {
                    SlateUtility.JavaScriptInterOp.HideScriptErrors(browserControl, true);
                    browserControl.Source = link;
                }
                else
                {
                    SlateUtility.JavaScriptInterOp.HideScriptErrors(browserControlPotriat, true);
                    browserControlPotriat.Source = link;
                }
            }
            else
            {
                //have to implement a new error message which is shown when the URI is not prperly bound to the WebSource
            }
        }

        /// <summary>
        /// Overriding the method where in the grids are Visibility controlled here..
        /// Potriat Container is hidden and Landscape Container is brought to focus
        /// </summary>
        protected override void SwitchToLandscapeView()
        {
            LandscapeView();
            SetWindowPosition();
            StateMaintainer();
            RefreshAd();
        }

        /// <summary>
        ///  Overriding the method where in the grids are Visibility controlled here..
        ///  Landscape Container is hidden and Potriat Container is brought to focus
        /// </summary>
        protected override void SwitchToPortraitView()
        {
            PortriatView();
            SetWindowPosition();
            StateMaintainer();
            RefreshAd();
        }

        /// <summary>
        /// The overriden method which sets the size of the window based on whether it is in landscape or potriat version
        /// </summary>
        protected override void SetSize()
        {
            if (!isSizeSet)
            {
                if (ApplicationData.IsLandscapeOrientation)
                {
                    LandscapeView();
                }
                else
                {
                    PortriatView();
                }
            }
        }

        /// <summary>
        /// A Method which sets the window to Landscape orientation with all the required properties
        /// </summary>
        private void LandscapeView()
        {
            Height = 560;
            Width = 970;
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
        private void PortriatView()
        {
            Width = 560;
            Height = 970;
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
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            Left = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
            Top = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;
        }

        /// <summary>
        /// The only state to be maintained in this page is the Article and the selection that was active..
        /// Called when the user switches from one Orientation to an other.
        /// </summary>
        private void StateMaintainer()
        {
            if (null != articleViewModel && null != articleViewModel.TopStoryActiveItem)
            {
                BrowserLoad(articleViewModel.TopStoryActiveItem.LinkForSlate);
            }
        }
	}
}