using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.Interfaces;
using NDTV.SlateApp.Framework.CustomEventArgs;
using NDTV.SlateApp.ViewModel;
using SlateAppProperties = NDTV.SlateApp.Properties;
using System.Windows.Media.Animation;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for ImageAlbumCarouselControl.xaml
    /// </summary>
    public partial class ImageAlbumCarouselControl : UserControl
    {
        /// <summary>
        /// View model for Carousel image view.
        /// </summary>
        private ImageSetViewModel imagesetViewModel = null;

        /// <summary>
        /// Last viewed category by the user.
        /// </summary>
        private ImageCategoryItem lastCategoryUsed = null;

        /// <summary>
        /// Currently selected category in the carousel control.
        /// </summary>
        private ImageCategoryItem selectedCategory = null;

        /// <summary>
        /// Event fired when the central album clicked in the carousel view.
        /// </summary>
        public event EventHandler<CarouselAlbumArgs> RaiseAlbumClickEvent;

        /// <summary>
        /// Event fired for close button of carousel view.
        /// </summary>
        public event ImageAlbumCarouselCloseEventHandler RaiseImageAlbumCarouselCloseEvent;

        /// <summary>
        /// Initializes a new instance of the ImageAlbumCarouselControl.
        /// </summary>
        public ImageAlbumCarouselControl()
        {
            InitializeComponent();
            lastCategoryUsed = new ImageCategoryItem();

            if (string.IsNullOrWhiteSpace(ApplicationData.Settings.ImageAlbumCarouselCategory))
            {
                imagesetViewModel = new ImageSetViewModel(ApplicationData.ImagesCategoryList.First());
            }
            else
            {
                if (null != ApplicationData.ImagesCategoryList && ApplicationData.ImagesCategoryList.Any(a => a.ImageAlbumTitle.Equals(ApplicationData.Settings.ImageAlbumCarouselCategory)))
                {
                    lastCategoryUsed = ApplicationData.ImagesCategoryList.Where(a => a.ImageAlbumTitle.Equals(ApplicationData.Settings.ImageAlbumCarouselCategory)).First();
                    imagesetViewModel = new ImageSetViewModel(lastCategoryUsed);
                }
            }
            this.DataContext = imagesetViewModel;
            (App.Current as App).OnApplicationOnline += OnApplicationOnline;
            (App.Current as App).OnApplicationOffline += OnApplicationOffline;
        }

        /// <summary>
        /// When any category radio button is clicked this event is fired.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">Routed Even tArgs.</param>
        private void RadioButtonClick(object sender, RoutedEventArgs e)
        {
            selectedCategory = null;
            if (ApplicationData.IsApplicationOnline)
            {
                if (e.OriginalSource.GetType().ToString().Substring(0).Contains("RadioButton"))
                {
                    selectedCategory = new ImageCategoryItem();
                    selectedCategory = (((FrameworkElement)e.OriginalSource).DataContext as ImageCategoryItem);
                    ApplicationData.Settings.ImageAlbumCarouselCategory = selectedCategory.ImageAlbumTitle;
                    imagesetViewModel.ImageSetViewModelReload(selectedCategory);
                    adBannerControl.RefreshAdBanner();
                    adBannerControlBig.RefreshAdBanner();
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(SlateAppProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
           
        }

        /// <summary>
        /// Event fires when application comes online.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        private void OnApplicationOnline(object sender, EventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                CategoriesList.IsEnabled = true;
                FlowControls3D.Items.Refresh();
            }));
        }

        /// <summary>
        /// Event fires when application comes offline.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args</param>
        private void OnApplicationOffline(object sender, EventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                CategoriesList.IsEnabled = false;
            }));
        }

        /// <summary>
        /// Arguments generated on any album click in the carousel.
        /// </summary>
        private CarouselAlbumArgs albumClickArgs = null;

        /// <summary>
        /// Event fired when the center album of the carousel clicked.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Routed Event Args.</param>
        private void TransparentButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                albumClickArgs = new CarouselAlbumArgs();
                albumClickArgs.SelectedAlbum = (FlowControls3D.SelectedItem as ImageAlbum);
                albumClickArgs.RelatedAlbumLists = imagesetViewModel.ImageAlbums;
                RaiseAlbumClickEvent(null, albumClickArgs);
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(SlateAppProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Event fired when close button clicked on carousel control/
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Routed Event Args.</param>
        private void CloseCauroselGalleryImage(object sender, RoutedEventArgs e)
        {
            imagesetViewModel.Dispose();
            this.DataContext = null;
            (App.Current as App).OnApplicationOnline -= OnApplicationOnline;
            (App.Current as App).OnApplicationOffline -= OnApplicationOffline;
            RaiseImageAlbumCarouselCloseEvent();
        }

        /// <summary>
        /// Notify Landscape View to the user control.
        /// </summary>
        public void NotifyLandscapeView()
        {
            PotraitGrid.Visibility = Visibility.Collapsed;
            Storyboard sb = (Storyboard)TryFindResource("PotraitLandscapeAnimation");
            sb.Begin();
            adBannerControl.RefreshAdBanner();
            adBannerControlBig.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Notify Portrait View to the user control.
        /// </summary>
        public void NotifyPortraitView()
        {
            PotraitGrid.Visibility = Visibility.Visible;
            Storyboard sb = (Storyboard)TryFindResource("PotraitLandscapeAnimation");
            sb.Begin();
            adBannerControlBig.Visibility = Visibility.Visible;
            adBannerControlBig.RefreshAdBanner();
        }
    }
}
