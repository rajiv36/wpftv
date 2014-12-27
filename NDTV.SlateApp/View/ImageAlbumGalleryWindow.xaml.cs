using System.Collections.ObjectModel;
using System.Windows;
using NDTV.Entities;
using NDTV.SlateApp.Framework.CustomEventArgs;
using System;
using NDTV.Controller;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for photoset.xaml
    /// </summary>
    public partial class ImageAlbumGalleryWindow : SlateWindow
    {
        /// <summary>
        /// Initializes a new instance of the ImageAlbumGallery class.
        /// </summary>
        public ImageAlbumGalleryWindow()
        {
            this.InitializeComponent();
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
        /// Helper function for SwitchToLandscapeView.
        /// </summary>
        protected override void SwitchToLandscapeView()
        {
            this.Width = 950;
            this.Height = 550;
            imageGalleryControl.NotifyLandscapeView();
            fullScreenGalleryControl.NotifyLandscapeView();
            SetWindowPosition();
        }

        /// <summary>
        /// Helper function for SwitchToPortraitView.
        /// </summary>
        protected override void SwitchToPortraitView()
        {
            this.Width = 550;
            this.Height = 950;
            imageGalleryControl.NotifyPortraitView();
            fullScreenGalleryControl.NotifyPortraitView();
            SetWindowPosition();
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
        /// Initializes a new instance of the ImageAlbumGallery class.
        /// </summary>
        /// <param name="selectedAlbum">The selected album.</param>
        /// <param name="relatedAlbums">Related album.</param>
        public ImageAlbumGalleryWindow(ImageAlbum selectedAlbum, ObservableCollection<ImageAlbum> relatedAlbums)
        {
            this.InitializeComponent();
            imageGalleryControl.CurrentlySelectedAlbum = selectedAlbum;
            imageGalleryControl.RelatedAlbumList = relatedAlbums;
        }

        /// <summary>
        /// Event for main window close button click.
        /// </summary>
        private void ImageControlCloseEvent()
        {
            imageGalleryControl.RelatedAlbumList = null;
            imageGalleryControl.CurrentlySelectedAlbum = null;
            this.Dispose();
            this.Close();
        }

        /// <summary>
        /// Event for full screen button click 
        /// </summary>
        /// <param name="sender">The Sender object.</param>
        /// <param name="e">Image Gallery Args.</param>
        private void ImageGalleryControlFullScreenEvent(object sender, ImageGalleryArgs e)
        {
            imageGalleryControl.Visibility = Visibility.Hidden;
            fullScreenGalleryControl.imageListBox.ItemsSource = e.ImageList;
            fullScreenGalleryControl.imageListBox.SelectedIndex = e.SelectedIndex;
            fullScreenGalleryControl.imageListBoxPortrait.ItemsSource = e.ImageList;
            fullScreenGalleryControl.imageListBoxPortrait.SelectedIndex = e.SelectedIndex;
            fullScreenGalleryControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Full screen Gallery close button click click.
        /// </summary>
        void FullScreenGalleryControlCloseEvent()
        {
            fullScreenGalleryControl.Visibility = Visibility.Hidden;
            imageGalleryControl.imageListBox.SelectedIndex = fullScreenGalleryControl.imageListBox.SelectedIndex;
            imageGalleryControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Dispose view.
        /// </summary>
        protected override void DisposeResources()
        {
            imageGalleryControl = null;
            fullScreenGalleryControl = null;
        }
    }
}