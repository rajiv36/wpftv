using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NDTV.SlateApp.ViewModel;
using System.Windows.Media.Animation;
using System;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Delegate for close button of full screen gallery view.
    /// </summary>
    public delegate void CloseUserControlHandler();

    /// <summary>
    /// Interaction logic for FullScreen_ImageGallery.xaml
    /// </summary>
    public partial class FullScreenImageGalleryControl : UserControl
    {
        /// <summary>
        /// Event for Full Screen Gallery View close button.
        /// </summary>
        public event CloseUserControlHandler RaiseCloseUserControlEvent;

        /// <summary>
        /// View model for Full screen gallery view.
        /// </summary>
        private ImageViewModel imageViewModel = null;

        /// <summary>
        /// Initializes a new instance of the FullScreen ImageGallery Control.
        /// </summary>
        public FullScreenImageGalleryControl()
        {
            this.InitializeComponent();
            imageViewModel = new ImageViewModel();
            this.DataContext = imageViewModel;
        }

        /// <summary>
        /// Event fired when close button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Routed Event Args. </param>
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            if (PlayPausetoggleButton.IsChecked.HasValue && PlayPausetoggleButton.IsChecked.Value)
            {
                PlayPausetoggleButton.IsChecked = false;
            }

            imageViewModel.Dispose();
            RaiseCloseUserControlEvent();
        }

        /// <summary>
        /// Double click event fired to go back to main gallery small screen.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Mouse Button Event Args.</param>
        private void GoBackMainGallerySmallEvent(object sender, MouseButtonEventArgs e)
        {
            if (PlayPausetoggleButton.IsChecked.HasValue && PlayPausetoggleButton.IsChecked.Value)
            {
                PlayPausetoggleButton.IsChecked = false;
            }
            imageViewModel.Dispose();
            RaiseCloseUserControlEvent();
        }

        /// <summary>
        /// for TapDownOnPanningItem Will be removed.
        /// </summary>
        private bool isFirstTime = true;

        /// <summary>
        /// Mouse down on Panning Item.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        private void TapDownOnPanningItem(object sender, MouseButtonEventArgs e)
        {
            if (PlayPausetoggleButton.IsChecked.HasValue && PlayPausetoggleButton.IsChecked.Value)
            {
                PlayPausetoggleButton.IsChecked = false;
            }
            else
            {
                if (imageViewModel.IsNewImage && isFirstTime)
                {
                    imageViewModel.IsNewImage = false;
                    imageViewModel.IsTouched = true;
                    isFirstTime = false;
                    imageViewModel.IsTouched = false;
                }
                else if (imageViewModel.IsNewImage)
                {
                    imageViewModel.IsNewImage = false;
                    imageViewModel.IsTouched = true;
                }
                else
                {
                    imageViewModel.IsTouched = !imageViewModel.IsTouched;
                }
            }
        }

        /// <summary>
        /// Notify Landscape View to the user control.
        /// </summary>
        public void NotifyLandscapeView()
        {
            Storyboard sb = (Storyboard)TryFindResource("PotraitLandscapeAnimation");
            sb.Begin();
            imageListBoxPortrait.Visibility = Visibility.Collapsed;
            imageListBox.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Notify Portrait View to the user control.
        /// </summary>
        public void NotifyPortraitView()
        {
            Storyboard sb = (Storyboard)TryFindResource("PotraitLandscapeAnimation");
            sb.Begin();
            imageListBox.Visibility = Visibility.Collapsed;
            imageListBoxPortrait.Visibility = Visibility.Visible;
        }
    }
}