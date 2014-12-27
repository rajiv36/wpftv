
using System;
using NDTV.Controller;
using NDTV.Entities;

namespace NDTV.SlateApp.ViewModel
{
    public class SocialShareViewModel : ViewModelBase
    {
        public event EventHandler CancelShare;

        /// <summary>
        /// Gets or sets the post command.
        /// </summary>
        private RelayCommand postCommand;
        public RelayCommand PostCommand
        {
            get
            {
                if (null == postCommand)
                {
                    postCommand = new RelayCommand(HandleShare);
                }
                return postCommand;
            }
        }

        /// <summary>
        /// Gets or sets the cancel command.
        /// </summary>
        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                if (null == cancelCommand)
                {
                    cancelCommand = new RelayCommand(HandleCancel);
                }
                return cancelCommand;
            }
        }

        /// <summary>
        /// Gets or sets the share title.
        /// </summary>
        private string shareTitle;
        public string ShareTitle
        {
            get
            {
                return this.shareTitle;
            }
            set
            {
                this.shareTitle = value;
                OnPropertyChanged("ShareTitle");
            }
        }

        /// <summary>
        /// Gets or sets the post message.
        /// </summary>
        private string postMessage;
        public string PostMessage
        {
            get
            {
                return this.postMessage;
            }
            set
            {
                this.postMessage = value;
                if (string.IsNullOrEmpty(value))
                {
                    this.IsPostEnabled = false;
                }
                else
                {
                    this.IsPostEnabled = true;
                }
                OnPropertyChanged("PostMessage");
            }
        }

        /// <summary>
        /// Gets or sets the image link.
        /// </summary>
        private string imageLink;
        public string ImageLink
        {
            get
            {
                return this.imageLink;
            }
            set
            {
                this.imageLink = value;
                OnPropertyChanged("ImageLink");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the post button 
        /// is enabled.
        /// </summary>
        private bool isPostEnabled;
        public bool IsPostEnabled
        {
            get
            {
                return this.isPostEnabled;
            }
            set
            {
                this.isPostEnabled = value;
                OnPropertyChanged("IsPostEnabled");
            }
        }

        /// <summary>
        /// Gets or sets the social share type.
        /// </summary>
        public SocialInteractionType SocialShareType
        {
            get;
            set;
        }

        /// <summary>
        /// Handles post on the appropriate social media.
        /// </summary>
        /// <param name="parameter">Parameter</param>
        private void HandleShare(object parameter)
        {
            switch (this.SocialShareType)
            {
                case SocialInteractionType.Facebook:
                    (ApplicationData.FacebookAccount as FacebookAccount).ShareMediaItem(this.PostMessage, ApplicationData.CurrentItem.Link,
                        ApplicationData.CurrentItem.ImageLink, ApplicationData.CurrentItem.Description, ApplicationData.CurrentItem.Caption);
                    break;
                case SocialInteractionType.Twitter:
                    (ApplicationData.TwitterAccount as TwitterAccount).ShareOnTwitter(this.PostMessage);
                    break;
                case SocialInteractionType.LinkedIn:
                    (ApplicationData.LinkedInAccount as LinkedInAccount).ShareMediaItem(this.PostMessage);
                    break;
            }
        }

        /// <summary>
        /// Handles the cancel functionality.
        /// </summary>
        /// <param name="parameter">Parameter</param>
        private void HandleCancel(object parameter)
        {
            if (null != this.CancelShare)
            {
                this.CancelShare(null, null);
            }
        }
    }
}
