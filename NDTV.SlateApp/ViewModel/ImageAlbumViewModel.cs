using System.Collections.ObjectModel;
using System.Globalization;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.Properties;
using SlateAppProperties = NDTV.SlateApp.Properties;
using System;

namespace NDTV.SlateApp.ViewModel
{
    public class ImageAlbumViewModel : ViewModelBase
    {
        /// <summary>
        /// Delegate for Close Button of Main Window.
        /// </summary>
        public delegate void OnSelectionChangeEventHandler();

        /// <summary>
        /// Event raised if close screen button pressed.
        /// </summary>
        public event OnSelectionChangeEventHandler RaisSelectionChangeEventHandler;

        /// <summary>
        /// Gives the current image number displayed against the Total number of images available.. eg 3/20.
        /// </summary>
        private string currentImageAgainstTotalImagesText;

        /// <summary>
        /// Gives the current image number displayed against the Total number of images available.. eg 3/20.
        /// </summary>
        public string CurrentImageAgainstTotalImagesText
        {
            get { return currentImageAgainstTotalImagesText; }
            set
            {
                currentImageAgainstTotalImagesText = value;
                OnPropertyChanged("CurrentImageAgainstTotalImagesText");
            }
        }

        /// <summary>
        /// Sets false when the image keeps panning. 
        /// </summary>
        public bool IsNewImage
        {
            get;
            set;
        }

        private bool isTouched;
        /// <summary>
        /// Sets when it gets the mouse down event.
        /// </summary>
        public bool IsTouched
        {
            get { return isTouched; }
            set
            {
                isTouched = value;
                OnPropertyChanged("IsTouched");
            }
        }

        /// <summary>
        /// Index of the selected image.
        /// </summary>
        private int selectedImageIndex;

        /// <summary>
        /// Index of the selected image.
        /// </summary>
        public int SelectedImageIndex
        {
            get
            {
                return selectedImageIndex;
            }
            set
            {
                selectedImageIndex = value;
                OnPropertyChanged("SelectedImageIndex");
            }
        }

        /// <summary>
        /// Play Pause slide Button Checked/Unchecked.
        /// </summary>
        private bool playPauseSlideshowIsChecked;

        /// <summary>
        /// Play Pause slide Button Checked/Unchecked.
        /// </summary>
        public bool PlayPauseSlideshowIsChecked
        {
            get { return playPauseSlideshowIsChecked; }
            set
            {
                playPauseSlideshowIsChecked = value;
                OnPropertyChanged("PlayPauseSlideShowIsChecked");
            }
        }

        /// <summary>
        /// Forward Button Click Command.
        /// </summary>
        private RelayCommand forwardButtonClickCommand;

        /// <summary>
        /// Forward Button Click Command.
        /// </summary>
        public RelayCommand ForwardButtonClickCommand
        {
            get
            {
                if (forwardButtonClickCommand == null)
                {
                    forwardButtonClickCommand = new RelayCommand((e) => OnForwardButtonClick());
                }
                return forwardButtonClickCommand;
            }
        }

        /// <summary>
        /// Back Button Click Command.
        /// </summary>
        private RelayCommand backButtonClickCommand;

        /// <summary>
        /// Back Button Click Command.
        /// </summary>
        public RelayCommand BackButtonClickCommand
        {
            get
            {
                if (backButtonClickCommand == null)
                {
                    backButtonClickCommand = new RelayCommand((e) => OnBackButtonClick());
                }
                return backButtonClickCommand;
            }
        }

        private RelayCommand onImageChangeCommand;

        /// <summary>
        /// On Image Change Command.
        /// </summary>
        public RelayCommand OnImageChangeCommand
        {
            get
            {
                if (onImageChangeCommand == null)
                {
                    onImageChangeCommand = new RelayCommand((e) => OnImageSelectionChanged());
                }
                return onImageChangeCommand;
            }
        }

        /// <summary>
        /// On ToggleButton Play.
        /// </summary>
        private RelayCommand beginSlideshowCommand;

        /// <summary>
        /// On ToggleButton Play.
        /// </summary>
        public RelayCommand BeginSlideshowCommand
        {
            get
            {
                if (beginSlideshowCommand == null)
                {
                    beginSlideshowCommand = new RelayCommand((e) => SlideshowBegin());
                }
                return beginSlideshowCommand;
            }
        }

        /// <summary>
        /// On ToggleButton Pause.
        /// </summary>
        private RelayCommand endSlideshowCommand;

        /// <summary>
        /// On ToggleButton Pause.
        /// </summary>
        public RelayCommand EndSlideshowCommand
        {
            get
            {
                if (endSlideshowCommand == null)
                {
                    endSlideshowCommand = new RelayCommand((e) => SlideshowEnd());
                }
                return endSlideshowCommand;
            }
        }

        /// <summary>
        /// Collection of images of an album.
        /// </summary>
        private ObservableCollection<ImageItem> imageItems;

        /// <summary>
        /// Collection of images of an album.
        /// </summary>
        public ObservableCollection<ImageItem> ImageItems
        {
            get
            {
                return imageItems;
            }
            private set
            {
                imageItems = value;
                OnPropertyChanged("ImageItems");
            }
        }

        /// <summary>
        /// Flag which is TRUE when the request is processed completely.And the imageItems are loaded.
        /// </summary>
        private bool isLoadingComplete;

        /// <summary>
        /// Flag which is TRUE when the request is processed completely.And the imageItems are loaded.
        /// </summary>
        public bool IsLoadingComplete
        {
            get { return isLoadingComplete; }
            set
            {
                isLoadingComplete = value;
                OnPropertyChanged("IsLoadingComplete");
            }
        }

        /// <summary>
        /// Name of the Album.
        /// </summary>
        private string albumName;

        /// <summary>
        /// Name of the Album.
        /// </summary>
        public string AlbumName
        {
            get { return albumName; }
            set
            {
                albumName = value;
                OnPropertyChanged("AlbumName");
            }
        }

        /// <summary>
        /// Collection of Album.
        /// </summary>
        public ObservableCollection<ImageAlbum> ImageAlbumCollection
        {
            get;
            private set;
        }

        /// <summary>
        /// Request for getting Image of selected album.
        /// </summary>
        private ImageItemRequest imageListRequest = null;

        /// <summary>
        /// Initializes a new instance of ImageAlbumViewModel class.
        /// </summary>
        /// <param name="album">The album whose images has to be fetched.</param>
        /// <param name="albumList">The list of available albums.</param>
        public ImageAlbumViewModel(ImageAlbum album, ObservableCollection<ImageAlbum> albumList)
        {
            if ((null != album) && (null != albumList))
            {


                ImageAlbumViewModelReload(album);
                this.ImageAlbumCollection = albumList;
            }
        }

        /// <summary>
        /// Reload the View Model with the selected Album.
        /// </summary>
        /// <param name="album">The album whose images has to be fetched.</param>
        /// <param name="albumList">The list of available albums.</param>
        public void ImageAlbumViewModelReload(ImageAlbum album)
        {
            if (null != album)
            {

                IsNewImage = false; //For ImageDesc Up/Down
                IsTouched = false; //For ImageDesc Up/Down
                imageListRequest = null;
                this.IsLoadingComplete = false;
                imageListRequest = new ImageItemRequest(album);
                ProcessRequest(imageListRequest, LoadResponse);
                this.AlbumName = album.AlbumTitle;
            }
        }

        /// <summary>
        /// Loads the content after it has been retrieved.
        /// </summary>
        /// <param name="imageItemRequest">The Response object.</param>
        private void LoadResponse(Response imageItemRequest)
        {
            if (imageItemRequest.GetType() == typeof(ImageItemResponse))
            {
                this.ImageItems = new ObservableCollection<ImageItem>(((ImageItemResponse)imageItemRequest).ImageItemCollection);
            }

            this.IsLoadingComplete = true;
            ApplicationData.TotalImagesOfSelectedAlbum = new ObservableCollection<int>();
            ApplicationData.TotalImagesOfSelectedAlbum.Add(imageItems.Count);
            this.CurrentImageAgainstTotalImagesText = string.Format(CultureInfo.CurrentCulture, Resources.CurrentPhotoAgainstTotalPhotosIncompleteText, (this.SelectedImageIndex + 1), this.ImageItems.Count);



        }

        /// <summary>
        /// Command on forward click.
        /// </summary>
        private void OnForwardButtonClick()
        {
            if (ApplicationData.IsApplicationOnline)
            {
                if (this.PlayPauseSlideshowIsChecked)
                {
                    this.PlayPauseSlideshowIsChecked = false;
                }
                this.SelectedImageIndex++;
                IsTouched = !IsTouched; //For ImageDesc Up/Down
                IsNewImage = false; //For ImageDesc Up/Down
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(SlateAppProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Command on back click.
        /// </summary>
        private void OnBackButtonClick()
        {
            if (ApplicationData.IsApplicationOnline)
            {
                if (this.PlayPauseSlideshowIsChecked)
                {
                    this.PlayPauseSlideshowIsChecked = false;
                }
                this.SelectedImageIndex--;
                IsTouched = !IsTouched; //For ImageDesc Up/Down
                IsNewImage = false; //For ImageDesc Up/Down
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(SlateAppProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Command on selection change.
        /// </summary>
        /// <param name="selectedImage">Selected image item.</param>
        private void OnImageSelectionChanged()
        {
            if (ApplicationData.IsApplicationOnline)
            {
                if (this.SelectedImageIndex == -1)
                {
                    this.SelectedImageIndex++;
                }

                IsNewImage = true; //For ImageDesc Up/Down
                IsTouched = true; //For ImageDesc Up/Down
                if (this.ImageItems != null)
                {
                    this.CurrentImageAgainstTotalImagesText = string.Format(CultureInfo.CurrentCulture, Resources.CurrentPhotoAgainstTotalPhotosIncompleteText, (this.SelectedImageIndex + 1), this.ImageItems.Count);
                }

            }
            else
            {
                (App.Current as App).DisplayErrorMessage(SlateAppProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
            RaisSelectionChangeEventHandler();
        }

        /// <summary>
        /// Command on Pause ToggleButton(Unchecked).
        /// </summary>
        private void SlideshowEnd()
        {
            timer.Stop();
            timer.Dispose();
        }

        /// <summary>
        /// Times for slide show.
        /// </summary>
        private System.Timers.Timer timer = null;

        /// <summary>
        /// Command on Play ToggleButton(checked).
        /// </summary>
        private void SlideshowBegin()
        {
            timer = new System.Timers.Timer(2000);
            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerEvent);
        }

        /// <summary>
        /// Event which gets fired after 2 mins.
        /// </summary>
        /// <param name="sender">The Sender object.</param>
        /// <param name="e">timer Elapsed Event Args.</param>
        private void TimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                if (this.SelectedImageIndex == ImageItems.Count - 1)
                {
                    this.PlayPauseSlideshowIsChecked = false;
                    SlideshowEnd();
                }
                else
                {
                    this.SelectedImageIndex++;
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(SlateAppProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
                this.PlayPauseSlideshowIsChecked = true;
            }
        }

        /// <summary>
        /// Disposes the objects in the view model
        /// </summary>
        protected override void DisposeResources()
        {
            this.backButtonClickCommand = null;
            this.beginSlideshowCommand = null;
            this.endSlideshowCommand = null;
            this.forwardButtonClickCommand = null;
            this.ImageAlbumCollection = null;
            this.imageItems = null;
            this.onImageChangeCommand = null;
            this.timer = null;
        }
    }
}