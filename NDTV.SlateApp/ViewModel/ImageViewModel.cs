using System.Globalization;
using System.Linq;
using NDTV.Controller;
using NDTV.SlateApp.Properties;
using SlateAppProperties = NDTV.SlateApp.Properties;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// Class ViewModel for fullScreen Image Gallery View.
    /// </summary>
    public class ImageViewModel : ViewModelBase
    {
        /// <summary>
        /// Gives the current image number displayed against the Total number of images available.. eg 3/20.
        /// </summary>
        private string currentImageAgainstTotalImagesText;

        /// <summary>
        /// Gives the current image number displayed against the Total number of images available.. eg 3/20.
        /// </summary>
        public string CurrentImageAgainstTotalImagesText
        {
            get 
            { 
                return currentImageAgainstTotalImagesText; 
            }
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
            get { return selectedImageIndex; }
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

        /// <summary>
        /// On Image Change Command.Selection change.
        /// </summary>
        private RelayCommand onImageChangeCommand;

        /// <summary>
        /// On Image Change Command.Selection change.
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
        /// Initializes the new instance of ImageViewModel.(Constructor)
        /// </summary>
        public ImageViewModel()
        {

        }

        /// <summary>
        /// Command on forward click.
        /// </summary>
        private void OnForwardButtonClick()
        {
            if (ApplicationData.IsApplicationOnline)
            {
                this.SelectedImageIndex++;
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
                this.SelectedImageIndex--;
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
                if (null != ApplicationData.TotalImagesOfSelectedAlbum)
                {
                    this.CurrentImageAgainstTotalImagesText = string.Format(CultureInfo.CurrentCulture, Resources.CurrentPhotoAgainstTotalPhotosIncompleteText, (selectedImageIndex + 1), ApplicationData.TotalImagesOfSelectedAlbum.First());
                }

                IsNewImage = true; //For ImageDesc Up/Down
                IsTouched = false;//For ImageDesc Up/Down
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(SlateAppProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
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
                if (this.SelectedImageIndex == ApplicationData.TotalImagesOfSelectedAlbum.First() - 1)
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
            this.forwardButtonClickCommand = null;
            this.onImageChangeCommand = null;
        }

    }
}
