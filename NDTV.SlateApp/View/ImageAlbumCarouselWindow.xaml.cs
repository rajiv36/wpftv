using System.Windows;
using NDTV.SlateApp.Framework.CustomEventArgs;
using System;
using NDTV.Controller;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for ImageAlbumCarouselWindow.xaml
    /// </summary>
    public partial class ImageAlbumCarouselWindow : SlateWindow
    {
        /// <summary>
        /// Initializes a new instance of the ImageAlbumCarouselWindow class.
        /// </summary>
        public ImageAlbumCarouselWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Helper function for SwitchToLandscapeView.
        /// </summary>
        protected override void SwitchToLandscapeView()
        {
            this.Width = 950;
            this.Height = 550;
            imageAlbumCarouselControl.NotifyLandscapeView();
            imageGalleryControl.NotifyLandscapeView();
            fsGalleryControl.NotifyLandscapeView();
            SetWindowPosition();
        }

        /// <summary>
        /// Helper function for SwitchToPortraitView.
        /// </summary>
        protected override void SwitchToPortraitView()
        {
            this.Width = 550;
            this.Height = 950;
            imageAlbumCarouselControl.NotifyPortraitView();
            imageGalleryControl.NotifyPortraitView();
            fsGalleryControl.NotifyPortraitView();
            SetWindowPosition();
        }

        /// <summary>
        /// The overriden method which sets the size of the window based on whether it is in landscape or potriat version
        /// </summary>
        protected override void SetSize()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                this.SetWindowPosition();
                SwitchToLandscapeView();
            }
            else
            {
                this.SetWindowPosition();
                SwitchToPortraitView();
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
        /// Click on album in carousel images view.
        /// </summary>
        /// <param name="albumClickArgs">Album Click Args.</param>
        private void AlbumClickEvent(object sender, CarouselAlbumArgs albumClickArgs)
        {
            imageAlbumCarouselControl.Visibility = Visibility.Hidden;
            imageGalleryControl.CurrentlySelectedAlbum = albumClickArgs.SelectedAlbum;
            imageGalleryControl.RelatedAlbumList = albumClickArgs.RelatedAlbumLists;
            imageGalleryControl.Visibility = Visibility.Visible;
            fsGalleryControl.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// When main window close button pressed.
        /// </summary>
        private void ImageControlCloseEvent()
        {
            imageGalleryControl.Visibility = Visibility.Hidden;
            imageAlbumCarouselControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Full screen button pressed on image gallery control.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Image Gallery Args</param>
        private void ImageGalleryControlFullScreenEvent(object sender, ImageGalleryArgs e)
        {
            imageGalleryControl.Visibility = Visibility.Hidden;
            fsGalleryControl.imageListBox.ItemsSource = e.ImageList;
            fsGalleryControl.imageListBox.SelectedIndex = e.SelectedIndex;
            fsGalleryControl.imageListBoxPortrait.ItemsSource = e.ImageList;
            fsGalleryControl.imageListBoxPortrait.SelectedIndex = e.SelectedIndex;
            fsGalleryControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// When close button pressed on full screen.
        /// </summary>
        private void FullScreenGalleryControlCloseEvent()
        {
            fsGalleryControl.Visibility = Visibility.Hidden;
            imageGalleryControl.imageListBox.SelectedIndex = fsGalleryControl.imageListBox.SelectedIndex;
            imageGalleryControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Image Carousel Window close event.
        /// </summary>
        private void CarouselCloseEvent()
        {
            this.Dispose();
            this.Close();
        }

        /// <summary>
        /// Dispose view.
        /// </summary>
        protected override void DisposeResources()
        {
            imageAlbumCarouselControl = null;
            imageGalleryControl = null;
            fsGalleryControl = null;
        }
    }
}
