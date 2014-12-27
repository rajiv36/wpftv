using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NDTV.SlateApp.Controls
{
    /// <summary>
    /// Preload image control. This has a default image till the image is loaded
    /// </summary>
    public class PreloadImageControl : Image
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static PreloadImageControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
             typeof(PreloadImageControl),
             new FrameworkPropertyMetadata(typeof(PreloadImageControl)));
        }

        /// <summary>
        /// On initializing the control
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Source = new BitmapImage(new System.Uri(this.DefaultImageSource, UriKind.RelativeOrAbsolute));
            this.Stretch = this.DefaultImageStretch;
            HandleImageDownload(this.ImageSource);           
        }

        /// <summary>
        /// This method handles image download
        /// </summary>
        /// <param name="imageSource">Image source</param>
        private void HandleImageDownload(string imageSource)
        {
            if (false == string.IsNullOrEmpty(imageSource))
            {
                this.Visibility = Visibility.Visible;
                BitmapImage bitmapImage = new BitmapImage(new Uri(imageSource, UriKind.RelativeOrAbsolute));
                bitmapImage.DownloadCompleted += ImageDownloadCompleted;
                bitmapImage.DownloadFailed += ImageDownloadFailed;
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// On image download failure, make the control invisible
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event args</param>
        private void ImageDownloadFailed(object sender, ExceptionEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
      
        /// <summary>
        /// Dependency property to bind default image source
        /// </summary>
        public static readonly DependencyProperty DefaultImageSourceProperty
           = DependencyProperty.Register("DefaultImageSource", typeof(string), typeof(PreloadImageControl), new FrameworkPropertyMetadata(string.Empty));      
        
        /// <summary>
        /// When image download is completed
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void ImageDownloadCompleted(object sender, EventArgs e)
        {
            this.Source = (sender as BitmapImage);
            this.Stretch = LoadedImageStretch;
        }

        /// <summary>
        /// Gets and sets the default image source
        /// </summary>
        public string DefaultImageSource
        {
            get
            {
               return (GetValue(DefaultImageSourceProperty) as string);
            }
            set
            {
                if (DefaultImageSource.Equals(value))
                {
                    return;
                }
               SetValue(DefaultImageSourceProperty, value);
               this.Source = new BitmapImage(new System.Uri(value));
            }
        }

        /// <summary>
        /// Gets and sets the images source
        /// </summary>
        public string ImageSource
        {
            get { return (GetValue(ImageSourceProperty)as string); }
            set 
            {
                if (ImageSource.Equals(value))
                {
                    return;
                }
                SetValue(ImageSourceProperty, value);
                HandleImageDownload(value);
            }
        }

        /// <summary>
        /// Dependency property for image source property
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(PreloadImageControl), new UIPropertyMetadata(string.Empty));


        /// <summary>
        /// Gets and sets the default image stretch
        /// </summary>
        public Stretch DefaultImageStretch
        {
            get { return (Stretch)GetValue(DefaultImageStretchProperty); }
            set 
            {                
                SetValue(DefaultImageStretchProperty, value);                 
            }
        }

        /// <summary>
        /// Dependency property for default image stretch
        /// </summary>
        public static readonly DependencyProperty DefaultImageStretchProperty =
            DependencyProperty.Register("DefaultImageStretch", typeof(Stretch), typeof(PreloadImageControl), new UIPropertyMetadata(Stretch.None));


        /// <summary>
        /// Gets and sets the loaded image stretch
        /// </summary>
        public Stretch LoadedImageStretch
        {
            get { return (Stretch)GetValue(LoadedImageStretchProperty); }
            set { SetValue(LoadedImageStretchProperty, value); }
        }

        /// <summary>
        /// Dependency property for loaded image stretch property
        /// </summary>
        public static readonly DependencyProperty LoadedImageStretchProperty =
            DependencyProperty.Register("LoadedImageStretch", typeof(Stretch), typeof(PreloadImageControl), new UIPropertyMetadata(Stretch.None));                   
        
    }
}
