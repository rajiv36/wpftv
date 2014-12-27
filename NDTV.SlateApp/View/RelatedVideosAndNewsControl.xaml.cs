using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NDTV.SlateApp.ViewModel;
using System.Collections.ObjectModel;
using NDTV.Entities;
using NDTV.Controller;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for RelatedVideosAndNewsControl.xaml
    /// </summary>
    public partial class RelatedVideosAndNewsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RelatedVideosAndNewsControl()
        {
            InitializeData();
        }

        /// <summary>
        /// RelatedItem view model instance
        /// </summary>
        private RelatedItemsViewModel relatedItemsViewModel;

        /// <summary>
        /// Stores the previous share data.
        /// </summary>
        private ShareData previousShareData;

        /// <summary>
        /// Inintialize data
        /// </summary>
        private void InitializeData()
        {
            InitializeComponent();
            SetOrientation();
            this.RelatedItems.Visibility = Visibility.Collapsed;
            this.preLoader.Visibility = Visibility.Visible;
            relatedItemsViewModel = new RelatedItemsViewModel(RelatedItemsLoaded);
        }

        /// <summary>
        /// Callback method for videos and news loaded
        /// </summary>
        private void RelatedItemsLoaded()
        {
            this.RelatedItems.Visibility = Visibility.Visible;
            this.DataContext = relatedItemsViewModel;
        }

        /// <summary>
        /// Related videos selected
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MouseButtonEventArgs</param>
        private void RelatedVideosButtonClick(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                this.previousShareData = ApplicationData.CurrentItem;
                VideoPlayer videoPlayer = new VideoPlayer((Entities.VideoItem)(((FrameworkElement)e.OriginalSource).DataContext));
                videoPlayer.Owner = App.Current.MainWindow;
                videoPlayer.Closed += PopUpClosed;                
                videoPlayer.ShowDialog();
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Handles the close of the pop up.
        /// </summary>
        /// <param name="sender">Pop up window</param>
        /// <param name="e">Event arguments</param>
        private void PopUpClosed(object sender, EventArgs e)
        {
            ApplicationData.CurrentItem = this.previousShareData;
        }

        /// <summary>
        /// related news event click
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="args">RoutedEventArgs</param>
        private void RelatedNewsButtonClick(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                this.previousShareData = ApplicationData.CurrentItem;
                Article articleWindow = new Article((Entities.TopStoryItem)(((FrameworkElement)e.OriginalSource).DataContext), relatedItemsViewModel.CricketNews);
                articleWindow.Owner = App.Current.MainWindow;
                articleWindow.Closed += PopUpClosed;
                articleWindow.ShowDialog();
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Set orientation to the user control.
        /// </summary>
        public void SetOrientation()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                RelatedItems.Margin = new Thickness(20,0,20,0);
            }
            else
            {
                RelatedItems.Margin = new Thickness(0, 0, 0, 0);
            }
        }
    }
}
