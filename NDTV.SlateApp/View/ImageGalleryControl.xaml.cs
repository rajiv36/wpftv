using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.Interfaces;
using NDTV.SlateApp.Framework.CustomEventArgs;
using NDTV.SlateApp.ViewModel;
using SlateAppProperties = NDTV.SlateApp.Properties;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Collections.Generic;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for ImageGalleryControl.xaml
    /// </summary>
    public partial class ImageGalleryControl : UserControl
    {
        /// <summary>
        /// Selected Album.
        /// </summary>
        public ImageAlbum CurrentlySelectedAlbum
        {
            get
            {
                return (ImageAlbum)GetValue(CurrentlySelectedAlbumProperty);
            }
            set
            {
                SetValue(CurrentlySelectedAlbumProperty, value);
                if (null != value)
                {
                    ApplicationData.SetCurrentItem(value.AlbumTitle, value.AlbumDescription, value.AlbumId.ToString(CultureInfo.InvariantCulture),
                        value.ThumbnailLink, value.AlbumLink, ShareMediaType.PhotoSet);
                }
            }
        }

        // Using a DependencyProperty as the backing store for CurrentlySelectedAlbum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentlySelectedAlbumProperty =
            DependencyProperty.Register("CurrentlySelectedAlbum", typeof(ImageAlbum), typeof(ImageGalleryControl), new UIPropertyMetadata(null));

        /// <summary>
        /// Related Album list.
        /// </summary>
        public ObservableCollection<ImageAlbum> RelatedAlbumList
        {
            get { return (ObservableCollection<ImageAlbum>)GetValue(RelatedAlbumListProperty); }
            set { SetValue(RelatedAlbumListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RelatedAlbumList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RelatedAlbumListProperty =
            DependencyProperty.Register("RelatedAlbumList", typeof(ObservableCollection<ImageAlbum>), typeof(ImageGalleryControl), new UIPropertyMetadata(RelatedAlbumCallBack));

        /// <summary>
        /// Image album ViewModel.
        /// </summary>
        private ImageAlbumViewModel albums = null;

        /// <summary>
        /// Event raised if full screen button pressed.
        /// </summary>
        public event EventHandler<ImageGalleryArgs> RaiseFullScreenEventHandler;

        /// <summary>
        /// Event raised if close screen button pressed.
        /// </summary>
        public event CloseButtonMainGalleryEventHandler RaiseWindowCloseEventHandler;

        /// <summary>
        /// Initializes a new instance of ImageGalleryControl class.
        /// </summary>
        /// <param name="selectedAlbum">Selected Album.</param>
        /// <param name="relatedAlbums">Related Albums.</param>
        public ImageGalleryControl()
        {
            InitializeComponent();
            ApplicationData.IsPopUpOpenValueChanged += IsPopUpOpenValueChanged;
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
                this.adBannerControl.ShowAdvertisement = false;
            }
            else
            {
                ModalPopup.Visibility = Visibility.Collapsed;
                this.adBannerControl.ShowAdvertisement = true;
            }
        }

        /// <summary>
        /// Static method call back of RelatedAlbumList.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void RelatedAlbumCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as ImageGalleryControl).RelatedAlbumInitializeCallBack();
        }

        /// <summary>
        /// Callback of RelatedAlbumList.
        /// </summary>
        private void RelatedAlbumInitializeCallBack()
        {
            if ((null != this.CurrentlySelectedAlbum) && (null != this.RelatedAlbumList))
            {
                albums = new ImageAlbumViewModel(this.CurrentlySelectedAlbum, this.RelatedAlbumList);
                albums.RaisSelectionChangeEventHandler += albums_RaisSelectionChangeEventHandler;
                this.DataContext = albums;
                this.albumGalleryItemsControl.SelectedItem = this.CurrentlySelectedAlbum;
            }
        }

       
        /// <summary>
        /// Full screen click fired.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Full Screen Routed event args.</param>
        private void FullScreenClick(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                if (PlayPausetoggleButton.IsChecked.HasValue && PlayPausetoggleButton.IsChecked.Value)
                {
                    PlayPausetoggleButton.IsChecked = false;
                }

                ImageGalleryArgs args = new ImageGalleryArgs();
                if (ApplicationData.IsLandscapeOrientation)
                {
                    args.ImageList = albums.ImageItems;
                    args.SelectedIndex = imageListBox.SelectedIndex;
                    RaiseFullScreenEventHandler(null, args);
                }
                else
                {
                    args.ImageList = albums.ImageItems;
                    //  args.SelectedIndex = imagePortraitListBox.SelectedIndex;
                    RaiseFullScreenEventHandler(null, args);
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(SlateAppProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Close button even fired.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Routed Event Args</param>
        private void CloseGalleryImage(object sender, RoutedEventArgs e)
        {
            ApplicationData.IsPopUpOpenValueChanged -= IsPopUpOpenValueChanged;
            if (PlayPausetoggleButton.IsChecked.HasValue && PlayPausetoggleButton.IsChecked.Value)
            {
                PlayPausetoggleButton.IsChecked = false;
            }
            //ViewModel Disposed.
            albums.Dispose();
            this.DataContext = null;
            this.CurrentlySelectedAlbum = null;
            this.RelatedAlbumList = null;
            albums.RaisSelectionChangeEventHandler -= albums_RaisSelectionChangeEventHandler;
            RaiseWindowCloseEventHandler();
        }

        /// <summary>
        /// Double click on panning control to enter Full Screen mode.
        /// </summary>
        /// <param name="sender">The Sender Object.</param>
        /// <param name="e">Mouse Button Event Args.</param>
        private void FullScreenDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                if (PlayPausetoggleButton.IsChecked.HasValue && PlayPausetoggleButton.IsChecked.Value)
                {
                    PlayPausetoggleButton.IsChecked = false;
                }

                ImageGalleryArgs args = new ImageGalleryArgs();
                args.ImageList = albums.ImageItems;
                args.SelectedIndex = imageListBox.SelectedIndex;
                RaiseFullScreenEventHandler(sender, args);
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(SlateAppProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Click on Album gallery list.
        /// </summary>
        /// <param name="sender">The Sender object.</param>
        /// <param name="e">Selection Changed Event Args.</param>
        private void ImagesGalleryItemsControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                if (imagesGalleryItemsControl.SelectedIndex != -1)
                {
                    this.imageListBox.SelectedIndex = imagesGalleryItemsControl.SelectedIndex;
                    ListBoxItem selectedListBoxItem = this.imagesGalleryItemsControl.ItemContainerGenerator.ContainerFromIndex(imagesGalleryItemsControl.SelectedIndex) as ListBoxItem;
                    if (selectedListBoxItem != null)
                    {
                        selectedListBoxItem.IsSelected = true;
                        selectedListBoxItem.Focus();
                    }
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(SlateAppProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Click on images of the selected album.
        /// </summary>
        /// <param name="sender">The Sender object.</param>
        /// <param name="e">Selection Changed Event Args.</param>
        private void AlbumGalleryItemsControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (ApplicationData.IsApplicationOnline)
            {
                if (imagesGalleryItemsControl.SelectedIndex != -1)
                {
                    ImageAlbum currentAlbum = (e.OriginalSource as ListBox).SelectedItem as ImageAlbum;
                    if (PlayPausetoggleButton.IsChecked.HasValue && PlayPausetoggleButton.IsChecked.Value)
                    {
                        PlayPausetoggleButton.IsChecked = false;
                    }
                    this.albums.ImageAlbumViewModelReload(currentAlbum);
                    ApplicationData.SetCurrentItem(currentAlbum.AlbumTitle, currentAlbum.AlbumDescription, currentAlbum.AlbumId.ToString(CultureInfo.InvariantCulture),
                                                    currentAlbum.ThumbnailLink, currentAlbum.AlbumLink, ShareMediaType.PhotoSet);
                    adBannerControl.RefreshAdBanner();
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(SlateAppProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Mouse down on Panning Item.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        private void TapDownOnPanningItem(object sender, MouseButtonEventArgs e)
        {
            //if (PlayPausetoggleButton.IsChecked.HasValue && PlayPausetoggleButton.IsChecked.Value)
            //{
            //    PlayPausetoggleButton.IsChecked = false;
            //}
            //else
            //{
            //    if (albums.IsNewImage)
            //    {
            //        albums.IsNewImage = false;
            //        albums.IsTouched = true;
            //        albums.IsTouched = false;
            //    }
            //    else
            //    {
            //        albums.IsTouched = !albums.IsTouched;
            //    }
            //}
        }

        /// <summary>
        /// On Grid View Screen click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewScreenClick(object sender, RoutedEventArgs e)
        {
            imagesGalleryGridView.SelectedIndex = -1;
            if (PlayPausetoggleButton.IsChecked.HasValue && PlayPausetoggleButton.IsChecked.Value)
            {
                PlayPausetoggleButton.IsChecked = false;
            }
            if (ApplicationData.IsLandscapeOrientation)
            {
                imageListBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                // imagePortraitListBox.Visibility = Visibility.Collapsed; //TODO:
            }
            ForwardButton.Opacity = 0;
            BackButton.Opacity = 0;
            imagesGalleryItemsControl.Visibility = Visibility.Hidden;
            imagesGalleryGridView.Visibility = Visibility.Visible;
            imagesGalleryGrid.Visibility = Visibility.Visible;
        }

        private void ImagesGalleryGridViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            
            imageListBox.SelectedIndex = imagesGalleryGridView.SelectedIndex;
            ForwardButton.Opacity = 1;
            BackButton.Opacity = 1;
            imagesGalleryGridView.Visibility = Visibility.Hidden;
            imagesGalleryGrid.Visibility = Visibility.Hidden;
            imagesGalleryItemsControl.Visibility = Visibility.Visible;
            if (ApplicationData.IsLandscapeOrientation)
            {
                imageListBox.Visibility = Visibility.Visible;
            }
            else
            {
                //imagePortraitListBox.Visibility = Visibility.Visible;  //TODO:
            }
            
        }

        /// <summary>
        /// Click on slide show button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Routed Event Args</param>
        private void PlaySlideshowButtonClick(object sender, RoutedEventArgs e)
        {
            //Hides the grid view and starts the normal slide show.
            ForwardButton.Opacity = 1;
            BackButton.Opacity = 1;
            imagesGalleryGridView.Visibility = Visibility.Hidden;
            imagesGalleryGrid.Visibility = Visibility.Hidden;
            imagesGalleryItemsControl.Visibility = Visibility.Visible;
            if (ApplicationData.IsLandscapeOrientation)
            {
                imageListBox.Visibility = Visibility.Visible;
            }
            else
            {
                // imagePortraitListBox.Visibility = Visibility.Visible;  //TODO:
            }
        }

        /// <summary>
        /// Notify Landscape View to the user control.
        /// </summary>
        public void NotifyLandscapeView()
        {
            Storyboard sb = (Storyboard)TryFindResource("PotraitLandscapeAnimation");
            sb.Begin();
            RelatedVideosColumn.Width = new GridLength(320, GridUnitType.Pixel);
            RelatedVidoesContainer.SetValue(Grid.RowProperty, 0);
            RelatedVidoesContainer.SetValue(Grid.ColumnProperty, 1);
            imagePortraitListBox.Visibility = Visibility.Collapsed;
            imageListBox.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Notify Portrait View to the user control.
        /// </summary>
        public void NotifyPortraitView()
        {
            Storyboard sb = (Storyboard)TryFindResource("PotraitLandscapeAnimation");
            sb.Begin();
            RelatedVideosColumn.Width = GridLength.Auto;
            RelatedVidoesContainer.SetValue(Grid.RowProperty, 1);
            RelatedVidoesContainer.SetValue(Grid.ColumnProperty, 0);
            imageListBox.Visibility = Visibility.Collapsed;
            imagePortraitListBox.Visibility = Visibility.Visible;
        }

        //private void imageListBox_ManipulationStarted(object sender, ManipulationStartingEventArgs e)
        //{
        //    if (e.Manipulators.ToList().Count == 2)
        //    {
        //        imageListBox.Opacity = 0;
        //        imageListBox.Visibility = Visibility.Collapsed;
                
        //        //grid1.Visibility = Visibility.Visible;
        //        //newImage.Visibility = System.Windows.Visibility.Visible;

        //        grid1.Opacity = 1;
        //        newImage.Opacity = 1;
        //        e.Handled = true;
        //       // grid1.Visibility = Visibility.Visible;
        //       //newImage.Visibility = Visibility.Visible;
        //        ////grid1.Visibility = true;
                
        //        //newImage_ManipulationDelta(newImage, e);
        //        ////newImage.Opacity=1;
        //        //grid1.Visibility = Visibility.Visible;
        //        //newImage.Visibility = System.Windows.Visibility.Visible;
        //        //imageListBox.Visibility = Visibility.Hidden;
        //    }
        
        //}
        //private void imageListBox_ManipulationInertiaStarting(object sender, ManipulationStartedEventArgs e)
        //{
        //    if (e.Manipulators.ToList().Count == 2)
        //    {
        //        //imageListBox.Opacity = 0;
        //       // imageListBox.Visibility = Visibility.Collapsed;

        //        //newImage.Opacity = 1;
        //        //newImage.Visibility = System.Windows.Visibility.Visible;
                
        //    }
        //}
        private Matrix tempMatrix;

        private Matrix tempMatrix2;

        private bool pinchingStarted;

        private void imageListBox_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {

            //if (e.Manipulators.ToList().Count == 2)
            //{
            //    var element = e.Source as FrameworkElement;
            //    if (element != null)
            //    {
            //        //e.DeltaManipulation has the changes 
            //        // Scale is a delta multiplier; 1.0 is last size,  (so 1.1 == scale 10%, 0.8 = shrink 20%) 
            //        // Rotate = Rotation, in degrees
            //        // Pan = Translation, == Translate offset, in Device Independent Pixels 


            //        var deltaManipulation = e.DeltaManipulation;

            //        if (pinchingStarted)
            //        {
            //            tempMatrix = imagematrix.Matrix;
            //        }
               
            //        var matrix = ((MatrixTransform)element.RenderTransform).Matrix;
            //        // find the old center; arguaby this could be cached 
            //        Point center = new Point(element.ActualWidth / 2, element.ActualHeight / 2);
            //        // transform it to take into account transforms from previous manipulations 
            //        center = matrix.Transform(center);
            //        //this will be a Zoom. 
            //        matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
            //        // Rotation 
            //        //matrix.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);
            //        //Translation (pan) 
            //        matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);

            //        ((MatrixTransform)element.RenderTransform).Matrix = matrix;

            //        // e.Handled = true;

            //        // We are only checking boundaries during inertia 
            //        // in real world, we would check all the time 
            //        if (e.IsInertial)
            //        {
            //            Rect containingRect = new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);

            //            Rect shapeBounds = element.RenderTransform.TransformBounds(new Rect(element.RenderSize));

            //            // Check if the element is completely in the window.
            //            // If it is not and intertia is occuring, stop the manipulation.
            //            if (e.IsInertial && !containingRect.Contains(shapeBounds))
            //            {
            //                //Report that we have gone over our boundary 
            //                e.ReportBoundaryFeedback(e.DeltaManipulation);
            //                // comment out this line to see the Window 'shake' or 'bounce' 
            //                // similar to Win32 Windows when they reach a boundary; this comes for free in .NET 4                
            //                e.Complete();
            //            }



            //        }
            //    }
            //    pinchingStarted = true;
            //}
            Matrix matrix = imagematrix.Matrix;
            if (e.Manipulators.ToList().Count == 2 && (matrix.M11 <= 2))
            {
                // just before scaling..
                tempMatrix2 = matrix;
                if (false == pinchingStarted)
                {
                    tempMatrix = imagematrix.Matrix;
                }

                
                // matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
                matrix.ScaleAt(e.DeltaManipulation.Scale.X, e.DeltaManipulation.Scale.Y,
                e.ManipulationOrigin.X, e.ManipulationOrigin.Y);

                imagematrix.Matrix = matrix;
                pinchingStarted = true;
                // e.Handled = true;
            }
            else if (e.Manipulators.ToList().Count == 2 && (matrix.M11 > 2))
            {
                //matrix.M11 = 2;
                //matrix.M22 = 2;
               imagematrix.Matrix = tempMatrix2;
            }

            
            //}
        }

        private void albums_RaisSelectionChangeEventHandler()
        {
            if (imageListBox.SelectedIndex != -1)
            {
                if (pinchingStarted)
                {
                    imagematrix.Matrix = tempMatrix;
                    pinchingStarted = false;
                }
                
            }
        }

      
        void canvas_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 DIPS per inch / 1000ms^2)
            e.TranslationBehavior = new InertiaTranslationBehavior()
            {
                InitialVelocity = e.InitialVelocities.LinearVelocity,
                DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0)
            };

            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 DIPS per inch / (1000ms^2)
            e.ExpansionBehavior = new InertiaExpansionBehavior()
            {
                InitialVelocity = e.InitialVelocities.ExpansionVelocity,
                DesiredDeceleration = 0.1 * 96 / 1000.0 * 1000.0
            };

            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior = new InertiaRotationBehavior()
            {
                InitialVelocity = e.InitialVelocities.AngularVelocity,
                DesiredDeceleration = 720 / (1000.0 * 1000.0)
            };
            e.Handled = true;
        }
        UIElement last;
        private void Grid_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            // lazy hack not in the blog post.. 
            var uie = e.OriginalSource as UIElement;
            if (uie != null)
            {
                if (last != null)
                {
                    Grid.SetZIndex(last, 0);

                }
                Grid.SetZIndex(uie, 2);
                last = uie;
            }

            //canvas is the parent of the image starting the manipulation;
            //Container does not have to be parent, but that is the most common scenario
            e.ManipulationContainer = grid1;


            e.Handled = true;
        }

        private void imageListBox_TouchUp(object sender, TouchEventArgs e)
        {
            if (PlayPausetoggleButton.IsChecked.HasValue && PlayPausetoggleButton.IsChecked.Value)
            {
                PlayPausetoggleButton.IsChecked = false;
            }
            else
            {
                if (albums.IsNewImage)
                {
                    albums.IsNewImage = false;
                    albums.IsTouched = true;
                    albums.IsTouched = false;
                }
                else
                {
                    albums.IsTouched = !albums.IsTouched;
                }
            }

        }

        

        
    }
}
