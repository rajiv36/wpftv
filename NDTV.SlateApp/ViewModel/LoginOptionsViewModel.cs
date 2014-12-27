
using System;
using System.Globalization;
using NDTV.Controller;
using NDTV.Entities;

namespace NDTV.SlateApp.ViewModel
{
    public class LoginOptionsViewModel : ViewModelBase
    {
        /// <summary>
        /// The event handler to indicate user initialization
        /// complete.
        /// </summary>
        public event EventHandler UserInstanceObtained;

        /// <summary>
        /// The event handler to indicate the user
        /// choice to cancel login.
        /// </summary>
        public event EventHandler CancelLogin;

        /// <summary>        
        /// Gets the login command.
        /// </summary>
        private RelayCommand login;
        public RelayCommand Login
        {
            get
            {
                if (null == login)
                {
                    login = new RelayCommand(HandleLogin);
                }
                return login;
            }
        }

        /// <summary>        
        /// Gets the cancel command.
        /// </summary>
        private RelayCommand cancel;
        public RelayCommand Cancel
        {
            get
            {
                if (null == cancel)
                {
                    cancel = new RelayCommand(HandleCancel);
                }
                return cancel;
            }
        }

        /// <summary>
        /// Handles cancel.
        /// </summary>
        /// <param name="parameter">Parameter</param>
        private void HandleCancel(object parameter)
        {
            if (null != this.CancelLogin)
            {
                this.CancelLogin(null, null);
            }
        }

        /// <summary>
        /// Handles login.
        /// </summary>
        /// <param name="parameter">Parameter</param>
        private void HandleLogin(object parameter)
        {
            LoginMode loginMode = GetInteractionType(parameter);
            switch (loginMode)
            {
                case LoginMode.Google:
                    ApplicationData.GoogleAccount = new GoogleAccount();
                    (ApplicationData.GoogleAccount as GoogleAccount).DialogClosed += LoginOptionsViewModelDialogClosed;
                    ApplicationData.GoogleAccount.AuthenticationCompleted += GoogleAuthenticationCompleted;
                    ApplicationData.GoogleAccount.Authenticate();
                    break;
                case LoginMode.Yahoo:
                    ApplicationData.YahooAccount = new YahooAccount();
                    (ApplicationData.YahooAccount as YahooAccount).DialogClosed += LoginOptionsViewModelDialogClosed;
                    ApplicationData.YahooAccount.AuthenticationCompleted += YahooAuthenticationCompleted;
                    ApplicationData.YahooAccount.Authenticate();
                    break;
                case LoginMode.Facebook:
                    ApplicationData.FacebookAccount = new FacebookAccount();                    
                    (ApplicationData.FacebookAccount as FacebookAccount).DialogClosed += LoginOptionsViewModelDialogClosed;
                    ApplicationData.FacebookAccount.AuthenticationCompleted += FacebookAccountAuthenticationCompleted;
                    ApplicationData.FacebookAccount.Authenticate();
                    break;
                case LoginMode.Twitter:
                    ApplicationData.TwitterAccount = new TwitterAccount();                    
                    (ApplicationData.TwitterAccount as TwitterAccount).DialogClosed += LoginOptionsViewModelDialogClosed;
                    ApplicationData.TwitterAccount.AuthenticationCompleted += TwitterAccountAuthenticationCompleted;
                    ApplicationData.TwitterAccount.Authenticate();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Dialog closed event handler.
        /// </summary>
        /// <param name="sender">Account</param>
        /// <param name="e">Event arguments</param>
        private void LoginOptionsViewModelDialogClosed(object sender, EventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ApplicationData.IsPopUpOpen = false;
            }));
        }

        /// <summary>
        /// This method is invoked on google authentication complete.
        /// </summary>
        /// <param name="sender">Google account</param>
        /// <param name="e">Event arguments</param>
        private void GoogleAuthenticationCompleted(object sender, EventArgs e)
        {            
            ApplicationData.GoogleAccount.AuthenticationCompleted -= GoogleAuthenticationCompleted;
            if (ApplicationData.FirstLoggedOn == LoginMode.None && (ApplicationData.GoogleAccount as GoogleAccount).IsLogged)
            {
                ApplicationData.FirstLoggedOn = LoginMode.Google;
                this.RegisterUser();
            }
        }

         /// <summary>
        /// This method is invoked on yahoo authentication complete.
        /// </summary>
        /// <param name="sender">Yahoo account</param>
        /// <param name="e">Event arguments</param>
        private void YahooAuthenticationCompleted(object sender, EventArgs e)
        {            
            ApplicationData.YahooAccount.AuthenticationCompleted -= YahooAuthenticationCompleted;
            if (ApplicationData.FirstLoggedOn == LoginMode.None && (ApplicationData.YahooAccount as YahooAccount).IsLogged)
            {
                ApplicationData.FirstLoggedOn = LoginMode.Yahoo;
                this.RegisterUser();
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
            if (ApplicationData.FirstLoggedOn == LoginMode.None && (ApplicationData.FacebookAccount as FacebookAccount).IsLogged)
            {
                ApplicationData.FirstLoggedOn = LoginMode.Facebook;
                this.RegisterUser();
            }
        }

        /// <summary>
        /// The event handler indicates authentication complete.
        /// </summary>
        /// <param name="sender">Twitter account</param>
        /// <param name="e">Event arguments</param>
        private void TwitterAccountAuthenticationCompleted(object sender, EventArgs e)
        {
            ApplicationData.TwitterAccount.AuthenticationCompleted -= TwitterAccountAuthenticationCompleted;
            if (ApplicationData.FirstLoggedOn == LoginMode.None && (ApplicationData.TwitterAccount as TwitterAccount).IsLogged)
            {
                ApplicationData.FirstLoggedOn = LoginMode.Twitter;
                this.RegisterUser();
            }
        }

        /// <summary>
        /// Gets the type of account.
        /// </summary>
        /// <param name="info">The object.</param>
        /// <returns>Returns the interaction type.</returns>
        private static LoginMode GetInteractionType(object info)
        {
            LoginMode loginMode = LoginMode.None;
            Enum.TryParse(info.ToString(), out loginMode);
            return loginMode;
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

                    case LoginMode.Google:
                        GoogleAccount googleAccount = ApplicationData.GoogleAccount as GoogleAccount;

                        ApplicationData.UserInstance.ProfileLink = googleAccount.GoogleUser.Details.PhotoLink;
                        ApplicationData.UserInstance.LoginMode = LoginMode.Google;
                        ApplicationData.UserInstance.DisplayName = googleAccount.GoogleUser.Details.Name.Short;

                        postData = String.Format(CultureInfo.InvariantCulture, Constants.RegisterUserConstants.RegisterUserData,
                            new object[] {googleAccount.GoogleUser.Details.Id, ApplicationData.UserInstance.DisplayName, 
                                ApplicationData.UserInstance.ProfileLink, "Google" });
                        break;

                    case LoginMode.Yahoo:
                        YahooAccount yahooAccount = ApplicationData.YahooAccount as YahooAccount;

                        ApplicationData.UserInstance.ProfileLink = yahooAccount.YahooUser.Details.ImageDetails.PhotoLink;
                        ApplicationData.UserInstance.LoginMode = LoginMode.Yahoo;
                        ApplicationData.UserInstance.DisplayName = yahooAccount.YahooUser.Details.Name;

                        postData = String.Format(CultureInfo.InvariantCulture, Constants.RegisterUserConstants.RegisterUserData,
                            new object[] {yahooAccount.YahooUser.Details.Id, ApplicationData.UserInstance.DisplayName, 
                                ApplicationData.UserInstance.ProfileLink, "Yahoo" });
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
        
            if (null != this.UserInstanceObtained)
            {
                this.UserInstanceObtained(null, null);
            }
        }
    }
}
