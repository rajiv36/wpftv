
using System;
using System.Globalization;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.Utilities;
using NDTV.SlateApp.Properties;

namespace NDTV.SlateApp.ViewModel
{
    public class SocialMediaInteractionViewModel : ViewModelBase
    {
        private SocialInteractionType socialInteractionType;
        private bool hasFunctionStarted;

        /// <summary>
        /// Gets or sets the number of comments.
        /// </summary>
        private string numberOfComments;
        public string NumberOfComments
        {
            get
            {
                return this.numberOfComments;
            }
            set
            {
                this.numberOfComments = value;
                OnPropertyChanged("NumberOfComments");
            }
        }

        /// <summary>
        /// Gets the social interaction command.
        /// </summary>
        private RelayCommand socialInteraction;
        public RelayCommand SocialInteraction
        {
            get
            {
                if (null == socialInteraction)
                {
                    socialInteraction = new RelayCommand(HandleSocialInteraction);
                }
                return socialInteraction;
            }
        }

        /// <summary>
        /// The comments view model.
        /// </summary>
        private CommentsViewModel commentsViewModel;        

        /// <summary>
        /// Constructor.
        /// </summary>
        public SocialMediaInteractionViewModel()
        {
            // Handler indicating when the current item is updated.
            ApplicationData.CurrenItemUpdated += CurrenItemUpdated;

            if (null != ApplicationData.CurrentItem && (ApplicationData.CurrentItem.ShareType == ShareMediaType.Article
                || ApplicationData.CurrentItem.ShareType == ShareMediaType.Video) && null != ApplicationData.CurrentItem.Guid)
            {
                this.commentsViewModel = new CommentsViewModel();
                this.commentsViewModel.CommentsObtained -= CommentsObtained;
                this.commentsViewModel.CommentsObtained += CommentsObtained;
            }
        }

        /// <summary>
        /// Handler indicating when the current item is updated.
        /// </summary>
        /// <param name="sender">Application Data</param>
        /// <param name="e">Event arguments</param>
        private void CurrenItemUpdated(object sender, EventArgs e)
        {
            if (null != ApplicationData.CurrentItem && (ApplicationData.CurrentItem.ShareType == ShareMediaType.Article 
                || ApplicationData.CurrentItem.ShareType == ShareMediaType.Video) && null != ApplicationData.CurrentItem.Guid)
            {
                this.commentsViewModel = new CommentsViewModel();
                this.commentsViewModel.CommentsObtained -= CommentsObtained;
                this.commentsViewModel.CommentsObtained += CommentsObtained;
            }
        }

        /// <summary>
        /// Indicates when the comments are obtained.
        /// </summary>
        /// <param name="sender">Comments view model</param>
        /// <param name="e">Event arguments</param>
        private void CommentsObtained(object sender, EventArgs e)
        {
            this.NumberOfComments = this.commentsViewModel.TotalComments.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Handles the appropriate social interaction.
        /// </summary>
        /// <param name="parameter">Parameter</param>
        private void HandleSocialInteraction(object parameter)
        {
            // Convert the parameter to the interaction type enumeration
            SocialInteractionType accountType = GetInteractionType(parameter);
            socialInteractionType = accountType;
            switch (accountType)
            {
                case SocialInteractionType.Facebook:
                    {
                        if (null == ApplicationData.FacebookAccount)
                        {
                            ApplicationData.FacebookAccount = new FacebookAccount();
                        }
                        if (false == hasFunctionStarted)
                        {
                            if (ApplicationData.FacebookAccount.IsLogged)
                            {
                                ShareOnFacebook();
                            }
                            else
                            {
                                this.LoginToFacebook();
                            }
                        }
                        break; 
                    }
                case SocialInteractionType.Twitter:
                    {
                        if (null == ApplicationData.TwitterAccount)
                        {
                            ApplicationData.TwitterAccount = new TwitterAccount();
                        }

                        if (false == hasFunctionStarted)
                        {
                            if (ApplicationData.TwitterAccount.IsLogged)
                            {
                                this.ShareOnTwitter();
                            }
                            else
                            {
                                this.LoginToTwitter();
                            }
                        }
                        break;
                    }
                case SocialInteractionType.LinkedIn:
                    {
                        if (null == ApplicationData.LinkedInAccount)
                        {
                            ApplicationData.LinkedInAccount = new LinkedInAccount();
                        }

                        if (false == hasFunctionStarted)
                        {
                            if (ApplicationData.LinkedInAccount.IsLogged)
                            {
                                ShareOnLinkedIn();
                            }
                            else
                            {
                                this.LoginToLinkedIn();
                            }
                        }
                        break;
                    }               
                case SocialInteractionType.Mail:
                    {
                        if (false == hasFunctionStarted)
                        {
                            HandleMailFeature();
                        }
                        break;
                    }
                case SocialInteractionType.Comments:
                    {
                        if (false == hasFunctionStarted)
                        {
                            this.HandleComments();
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets the type of account.
        /// </summary>
        /// <param name="info">The object.</param>
        /// <returns>Returns the interaction type.</returns>
        private static SocialInteractionType GetInteractionType(object info)
        {
            SocialInteractionType accountType = SocialInteractionType.None;
            Enum.TryParse(info.ToString(), out accountType);
            return accountType;
        }

        #region Facebook Functions
        /// <summary>
        /// Handles login to facebook.
        /// </summary>
        private void LoginToFacebook()
        {
            ApplicationData.FacebookAccount.AuthenticationCompleted += FacebookAccountAuthenticationCompleted;            
            ApplicationData.FacebookAccount.Authenticate();
        }

        /// <summary>
        /// This method shares the media data on facebook.
        /// </summary>
        private static void ShareOnFacebook()
        {
            if (null != ApplicationData.CurrentItem)
            {
                (ApplicationData.FacebookAccount as FacebookAccount).ShareMediaItem();
            }
        }

        /// <summary>
        /// The event handler indicates authentication complete.
        /// </summary>
        /// <param name="sender">Facebook account</param>
        /// <param name="e">Event arguments</param>
        private void FacebookAccountAuthenticationCompleted(object sender, EventArgs e)
        {
            ApplicationData.FacebookAccount.AuthenticationCompleted -= FacebookAccountAuthenticationCompleted;

            if (ApplicationData.FirstLoggedOn == LoginMode.None && ApplicationData.FacebookAccount.IsLogged)
            {
                ApplicationData.FirstLoggedOn = LoginMode.Facebook;
                this.RegisterUser();
            }

            if (ApplicationData.FacebookAccount.IsLogged && null != ApplicationData.CurrentItem && false == hasFunctionStarted)
            {
                ShareOnFacebook();
            }
        }
        #endregion Facebook Functions

        #region Twitter Functions
        /// <summary>
        /// Handles login to facebook.
        /// </summary>
        private void LoginToTwitter()
        {            
            // Detach any previous handler if attached.
            ApplicationData.TwitterAccount.AuthenticationCompleted -= TwitterAccountAuthenticationCompleted;            
            ApplicationData.TwitterAccount.AuthenticationCompleted += TwitterAccountAuthenticationCompleted;
            ApplicationData.TwitterAccount.Authenticate();
        }

        /// <summary>
        /// This method shares the data on twitter.
        /// </summary>
        private void ShareOnTwitter()
        {            
            if (null != ApplicationData.CurrentItem)
            {
                hasFunctionStarted = true;
                string url = string.Format(CultureInfo.InvariantCulture, Constants.TwitterConstants.BitLYShortenUrl, Utility.GetSocialData("BitLyUserName"),
                    Utility.GetSocialData("BitLyApiKey"), Uri.EscapeUriString(ApplicationData.CurrentItem.Link.ToString()));
                LinkShortenRequest request = new LinkShortenRequest(url);
                ApplicationData.Controller.ProcessRequest(request, OnLinkShortenComplete, this.HandleError);   
            }
        }

        /// <summary>
        /// Callback function for link shorten request.
        /// </summary>
        /// <param name="response">Response</param>
        private void OnLinkShortenComplete(Response response)
        {
            LinkShortenResponse shortenResponse = response as LinkShortenResponse;
            (ApplicationData.TwitterAccount as TwitterAccount).ShareMediaItem(shortenResponse.bitLYLink);
            hasFunctionStarted = false;
        }

        /// <summary>
        /// The event handler indicates authentication complete.
        /// </summary>
        /// <param name="sender">Twitter account</param>
        /// <param name="e">Event arguments</param>
        private void TwitterAccountAuthenticationCompleted(object sender, EventArgs e)
        {
            ApplicationData.TwitterAccount.AuthenticationCompleted -= TwitterAccountAuthenticationCompleted;

            if (ApplicationData.FirstLoggedOn == LoginMode.None && ApplicationData.TwitterAccount.IsLogged)
            {
                ApplicationData.FirstLoggedOn = LoginMode.Twitter;
                this.RegisterUser();
            }

            if (ApplicationData.TwitterAccount.IsLogged && null != ApplicationData.CurrentItem && false == hasFunctionStarted)
            {
                this.ShareOnTwitter();
            }
            
        }
        #endregion Twitter Functions

        #region LinkedIn Functions

        /// <summary>
        /// Handles login to linked in.
        /// </summary>
        private void LoginToLinkedIn()
        {            
            ApplicationData.LinkedInAccount.AuthenticationCompleted += LinkedInAccountAuthenticationCompleted;
            ApplicationData.LinkedInAccount.Authenticate();
        }

        /// <summary>
        /// This method shares the data on linked in.
        /// </summary>
        private static void ShareOnLinkedIn()
        {
            if (null != ApplicationData.CurrentItem)
            {                
                (ApplicationData.LinkedInAccount as LinkedInAccount).Share();
            }
        }
       
        /// <summary>
        /// The event handler indicates authentication complete.
        /// </summary>
        /// <param name="sender">Twitter account</param>
        /// <param name="e">Event arguments</param>
        private void LinkedInAccountAuthenticationCompleted(object sender, EventArgs e)
        {
            ApplicationData.LinkedInAccount.AuthenticationCompleted -= LinkedInAccountAuthenticationCompleted;

            if (ApplicationData.LinkedInAccount.IsLogged && null != ApplicationData.CurrentItem && false == hasFunctionStarted)
            {                
                ShareOnLinkedIn();
            }

        }
        #endregion 

        /// <summary>
        /// Sends out a mail based on the current item selected.
        /// </summary>
        private void HandleMailFeature()
        {
            if (null != ApplicationData.CurrentItem)
            {
                if (false == string.IsNullOrEmpty(ApplicationData.CurrentItem.Link))
                {
                    hasFunctionStarted = true;
                    //Short both links
                    string url = string.Format(CultureInfo.InvariantCulture, Constants.TwitterConstants.BitLYShortenUrl, Utility.GetSocialData("BitLyUserName"),
                    Utility.GetSocialData("BitLyApiKey"), Uri.EscapeUriString(ApplicationData.CurrentItem.Link));
                    LinkShortenRequest request = new LinkShortenRequest(url);
                    ApplicationData.Controller.ProcessRequest(request, this.OnMainLinkShortenComplete, this.HandleError);
                }
                else
                {
                    MailShareViewModel mailShareModel = new MailShareViewModel();
                    mailShareModel.SendMailCommand.Execute(ApplicationData.CurrentItem.ShareType);
                }
            }
        }

        /// <summary>
        /// This will show the error message
        /// </summary>  
        /// <param name="exception">Exception</param>
        private void HandleError(Exception exception)
        {
            string errorMessage = string.Empty;
            switch (socialInteractionType)
            { 
                case SocialInteractionType.Mail:
                    errorMessage = Resources.MailShareError;
                    break;        
                case SocialInteractionType.Twitter:
                    errorMessage = String.Format(CultureInfo.InvariantCulture, Resources.ErrorShare, ApplicationData.CurrentItem.ShareType.ToString());
                    break;
                default:
                    break;
            }
            (App.Current as App).DisplayErrorMessage(errorMessage, string.Empty, true, exception);
            hasFunctionStarted = false;
        }

        /// <summary>
        ///  Start sharing mail when main link is shorten.
        /// </summary>
        /// <param name="response"></param>
        private void OnMainLinkShortenComplete(Response response)
        {
            LinkShortenResponse shortenResponse = response as LinkShortenResponse;

            MailShareViewModel mailShareModel = new MailShareViewModel();
            mailShareModel.ShortLink = shortenResponse.bitLYLink;
            mailShareModel.SendMailCommand.Execute(ApplicationData.CurrentItem.ShareType);
            hasFunctionStarted = false;
        }

        /// <summary>
        /// Handles the comments functionality.
        /// </summary>
        private void HandleComments()
        {
            this.commentsViewModel.DisplayCommentsUI();            
        }

        /// <summary>
        /// Registers the user via the NDTV API.
        /// </summary>
        private void RegisterUser()
        {
            string postData = string.Empty;

            if (null == ApplicationData.UserInstance)
            {
                ApplicationData.UserInstance = new UserData();
                switch (ApplicationData.FirstLoggedOn)
                {
                    case LoginMode.Facebook:
                        FacebookAccount fbAccount = ApplicationData.FacebookAccount as FacebookAccount;
                        
                        ApplicationData.UserInstance.ProfileLink = fbAccount.FacebookUser.PhotoLink;
                        ApplicationData.UserInstance.LoginMode = LoginMode.Facebook;
                        ApplicationData.UserInstance.DisplayName = fbAccount.FacebookUser.Name;

                        postData = String.Format(CultureInfo.InvariantCulture, Constants.RegisterUserConstants.RegisterUserData,
                    new object[] { fbAccount.FacebookUser.Id, fbAccount.FacebookUser.Name, fbAccount.FacebookUser.PhotoLink, "Facebook" });
                        break;

                    case LoginMode.Twitter:
                        TwitterAccount twitterAccount = ApplicationData.TwitterAccount as TwitterAccount;

                        ApplicationData.UserInstance.ProfileLink = twitterAccount.TwitterUser.PhotoLink;
                        ApplicationData.UserInstance.LoginMode = LoginMode.Twitter;
                        ApplicationData.UserInstance.DisplayName = twitterAccount.TwitterUser.Name;

                        postData = String.Format(CultureInfo.InvariantCulture, Constants.RegisterUserConstants.RegisterUserData,
                            new object[] { twitterAccount.TwitterUser.Id, twitterAccount.TwitterUser.Name, twitterAccount.TwitterUser.PhotoLink, "Twitter" });
                        break;
                }
            }

            RegisterUserRequest request = new RegisterUserRequest(postData);
            ApplicationData.Controller.ProcessRequest(request, this.RegisterUserComplete, this.HandleError);
        }

        /// <summary>
        /// Registers user complete.
        /// </summary>
        /// <param name="response">Response</param>
        private void RegisterUserComplete(Response response)
        {
            ApplicationData.UserInstance.UserId = (response as RegisterUserResponse).UserData.Id;
        }
    }
}
