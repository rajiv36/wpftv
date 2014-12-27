using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NDTV.SlateApp.ViewModel;
using NDTV.Entities;
using System.Collections.ObjectModel;
using System.Threading;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for photoset.xaml
    /// </summary>
    public partial class ImageAlbumGallery : SlateWindow
    {
        /// <summary>
        /// Initializes a new instance of the ImageAlbumGallery class.
        /// </summary>
        public ImageAlbumGallery()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Main image Gallery Control.
        /// </summary>
        ImageGalleryControl mainImageGalleryControl = null;

        /// <summary>
        /// Full screen gallery control.
        /// </summary>
        FullScreenImageGalleryControl fullScreenGalleryControl = null;

        /// <summary>
        /// Initializes a new instance of the ImageAlbumGallery class.
        /// </summary>
        /// <param name="selectedAlbum">The selected album.</param>
        /// <param name="relatedAlbums">Related album.</param>
        public ImageAlbumGallery(ImageAlbum selectedAlbum, ObservableCollection<ImageAlbum> relatedAlbums)
        {
            this.InitializeComponent();
            mainImageGalleryControl = new ImageGalleryControl(selectedAlbum, relatedAlbums);
            mainImageGalleryControl.RaiseFullScreenEvent += new ImageGalleryControl.FullScreenHandler(ImageGalleryControlEvent);
            mainImageGalleryControl.RaiseWindowCloseEvent += new ImageGalleryControl.CloseButtonMainGalleryHandler(fsMainWindowClose);
            mainImageGalleryControl.Visibility = System.Windows.Visibility.Visible;
            this.LayoutRoot.Children.Add(mainImageGalleryControl);
            fullScreenGalleryControl = new FullScreenImageGalleryControl();
            fullScreenGalleryControl.RaiseCloseUserControlEvent += new FullScreenImageGalleryControl.CloseUserControlHandler(FullScreenGalleryControlRaiseEvent);
            fullScreenGalleryControl.Visibility = System.Windows.Visibility.Hidden;
            this.LayoutRoot.Children.Add(fullScreenGalleryControl);
        }

        /// <summary>
        /// Event for main window close button click.
        /// </summary>
        private void fsMainWindowClose()
        {
            this.LayoutRoot.Children.Remove(mainImageGalleryControl);
            this.LayoutRoot.Children.Remove(fullScreenGalleryControl);
            mainImageGalleryControl = null;
            fullScreenGalleryControl = null;
            this.Close();
        }

        /// <summary>
        /// Event for full screen button click 
        /// </summary>
        /// <param name="sender">The Sender object.</param>
        /// <param name="e">Image Gallery Args.</param>
        private void ImageGalleryControlEvent(object sender, NDTV.SlateApp.View.ImageGalleryControl.ImageGalleryArgs e)
        {
            mainImageGalleryControl.Visibility = System.Windows.Visibility.Hidden;
            fullScreenGalleryControl.imageListBox.ItemsSource = e.ImageList;
            fullScreenGalleryControl.imageListBox.SelectedIndex = e.SelectedIndex;
            fullScreenGalleryControl.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Full screen Gallery close button click click.
        /// </summary>
        void FullScreenGalleryControlRaiseEvent()
        {
            fullScreenGalleryControl.Visibility = System.Windows.Visibility.Hidden;
            mainImageGalleryControl.imageListBox.SelectedIndex = fullScreenGalleryControl.imageListBox.SelectedIndex;
            mainImageGalleryControl.Visibility = System.Windows.Visibility.Visible;
        }
    }
}