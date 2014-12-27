using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Navigation;
using NDTV.Controller;
using NDTV.SlateApp;
using NDTV.SlateApp.Properties;
using NDTV.SlateApp.View;
using NDTV.SlateApp.ViewModel;
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class TwitterAccount : Account
    {
        /// <summary>
        /// Event that gets raised when athentication is completed.
        /// </summary>
        public override event EventHandler<EventArgs> AuthenticationCompleted;

        /// <summary>
        /// Event that gets raised when logout is completed.
        /// </summary>
        public event EventHandler<EventArgs> LogOffCompleted;

        /// Dialog closed event.
        /// </summary>
        public event EventHandler DialogClosed;
    
        /// <summary>
        /// Gets the social user details.
        /// </summary>
        private TwitterUser twitterUser;
        public TwitterUser TwitterUser
        {
            get
            {
                return this.twitterUser;
            }
        }

        /// <summary>
        /// The authorization token.
        /// </summary>
        private string authToken;       

        /// <summary>
        /// The authorization secret key.
        /// </summary>
        private string authTokenSecretKey;        

        /// <summary>
        /// The authorization token verifier.
        /// </summary>
        private string authTokenVerifier;

        /// <summary>
        /// Indicates if the pop up is closed.
        /// </summary>
        private bool isPopUpClosed;

        /// <summary>
        /// The login control.
        /// </summary>        
        private SocialLogin loginControl;

        /// <summary>
        /// The social share dialog.
        /// </summary>
        private SocialShareDialog socialShareDialog;

        /// <summary>
        /// Gets the twitter consumer key.
        /// </summary>
        public override string ConsumerKey
        {
            get 
            {
                return Utility.GetSocialData("TwitterConsumerKey");
            }
        }

        /// <summary>
        /// Gets the twitter consumer secret key.
        /// </summary>
        public override string ConsumerSecret
        {
            get
            {
                return Utility.GetSocialData("TwitterConsumerSecretKey");
            }
        }

        /// <summary>
        /// Indicates if the user is logged in.
        /// </summary>
        private bool isLogged;
        public override bool IsLogged
        {
            get
            {
                return this.isLogged;
            }
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        private string accessToken;
        public override string AccessToken
        {
            get
            {
                return this.accessToken;
            }
        }

        /// <summary>
        /// Gets the token request link.
        /// </summary>
        private string tokenRequestLink;
        public string TokenRequestLink
        {
            get
            {
                return this.tokenRequestLink;
            }
        }

        /// <summary>
        /// Gets the user data request link.
        /// </summary>
        private string requestUserDatatLink;
        public string RequestUserDataLink
        {
            get
            {
                return this.requestUserDatatLink;
            }
        }

        /// <summary>
        /// Gets the token access link.
        /// </summary>
        private string tokenAccessLink;
        public string TokenAccessLink
        {
            get
            {
                return this.tokenAccessLink;
            }
        }            
    
        /// <summary>
        /// Authenticates the user.
        /// </summary>
        public override void Authenticate()
        {
            this.isPopUpClosed = false;
            
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {                
                this.loginControl = new SocialLogin();
                this.loginControl.WebBrowser.Navigated += WebBrowserNavigated;
                this.loginControl.Owner = App.Current.MainWindow;
                this.loginControl.WebBrowser.LoadCompleted += WebBrowserLoadCompleted;
                this.loginControl.Closed += LoginControlClosed;
                this.loginControl.ShowInTaskbar = false;
                this.loginControl.ShowDialog();                
            }), null);
           
            string outUrl = string.Empty;
            string querystring = string.Empty;
            string nonce = OAuthHelper.GenerateNonce();
            string timeStamp = OAuthHelper.GenerateTimestamp();

            // Generate Signature for request token.
            string sig = OAuthHelper.GenerateSignature(
                new Uri(Constants.TwitterConstants.RequestTokenLink),
                this.ConsumerKey,
                this.ConsumerSecret,
                string.Empty,
                string.Empty,
                string.Empty,
                Utility.GetSocialData("CallbackUrl"),
                HttpOperation.Post.ToString(),
                timeStamp,
                nonce,
                out outUrl,
                out querystring);

            // Generate the header for request token.
            string header = OAuthHelper.GenerateOAuthHeader(OAuthHelper.OAuthNonceKey, nonce, OAuthHelper.OAuthSignatureMethodKey,
                OAuthHelper.HMACSHA1SignatureType, OAuthHelper.OAuthTimestampKey, timeStamp,
                OAuthHelper.OAuthConsumerKeyKey, this.ConsumerKey, OAuthHelper.OAuthCallbackKey,
                OAuthHelper.EncodeLink(Utility.GetSocialData("CallbackUrl")), OAuthHelper.OAuthSignatureKey,
                OAuthHelper.EncodeLink(sig), OAuthHelper.OAuthVersionKey, OAuthHelper.OAuthVersion);

            this.tokenRequestLink = outUrl;

            if (false == this.isPopUpClosed)
            {
                TwitterTokenRequest tokenRequest = new TwitterTokenRequest(header);
                ApplicationData.Controller.ProcessRequest(tokenRequest, GetRequestTokenComplete, this.HandleError);
            }
            else if (null != this.AuthenticationCompleted)
            {
                this.AuthenticationCompleted(null, null);
            }
        }       

        /// <summary>
        /// Browser load complete.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Navigation event arguments</param>
        private void WebBrowserLoadCompleted(object sender, NavigationEventArgs e)
        {
            if (null != this.loginControl.WebBrowser.Source && 
                false == this.loginControl.WebBrowser.Source.AbsoluteUri.Contains(Utility.GetSocialData("CallbackUrl")))
            {
                this.loginControl.preLoader.Visibility = Visibility.Collapsed;
                this.loginControl.WebBrowser.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Handles the navigated method of the web browser.
        /// </summary>
        /// <param name="sender">sender - web browser</param>
        /// <param name="e">Navigation event arguments</param>
        private void WebBrowserNavigated(object sender, NavigationEventArgs e)
        {
            if (null != this.loginControl && this.isPopUpClosed == false)
            {
                if (false == String.IsNullOrEmpty(this.loginControl.WebBrowser.Source.AbsoluteUri))
                { 
                    Dictionary<string, string> result = Utility.GetQueryParameters(this.loginControl.WebBrowser.Source.AbsoluteUri);
                    if (result != null && result.Count > 0 && result.ContainsKey(OAuthHelper.OAuthTokenKey) && result.ContainsKey(OAuthHelper.OAuthVerifier))
                    {
                        this.loginControl.preLoader.Visibility = Visibility.Visible;
                        this.loginControl.webBrowserControl.Visibility = Visibility.Collapsed;
                        this.authToken = result[OAuthHelper.OAuthTokenKey];
                        this.authTokenVerifier = result[OAuthHelper.OAuthVerifier];

                        string outUrl = string.Empty;
                        string querystring = string.Empty;

                        string nonce = OAuthHelper.GenerateNonce();
                        string timeStamp = OAuthHelper.GenerateTimestamp();

                        // Generate Signature to retrieve the access token in exchange for the authorize token.
                        string sig = OAuthHelper.GenerateSignature(
                            new Uri(Constants.TwitterConstants.AccessTokenUrl),
                            this.ConsumerKey,
                            this.ConsumerSecret,
                            this.authToken,
                            this.authTokenSecretKey,
                            this.authTokenVerifier,
                            string.Empty,
                            HttpOperation.Post.ToString(),
                            timeStamp,
                            nonce,
                            out outUrl,
                            out querystring);

                        // Create a header and call for exchanging the access token using the authorize token (the token + verifier).
                        string header = OAuthHelper.GenerateOAuthHeader(OAuthHelper.OAuthNonceKey, nonce, OAuthHelper.OAuthSignatureMethodKey,
                            OAuthHelper.HMACSHA1SignatureType, OAuthHelper.OAuthTimestampKey, timeStamp, OAuthHelper.OAuthConsumerKeyKey,
                            this.ConsumerKey, OAuthHelper.OAuthTokenKey, this.authToken, OAuthHelper.OAuthVerifier, this.authTokenVerifier,
                            OAuthHelper.OAuthSignatureKey, OAuthHelper.EncodeLink(sig), OAuthHelper.OAuthVersionKey, OAuthHelper.OAuthVersion);                        
                        this.tokenAccessLink = outUrl;

                        if (this.isPopUpClosed == false)
                        {
                            TwitterAuthorizeTokenRequest request = new TwitterAuthorizeTokenRequest(header);
                            ApplicationData.Controller.ProcessRequest(request, GetAuthorizeTokenComplete, this.HandleError);
                        }
                        else if (null != this.AuthenticationCompleted)
                        {
                            this.AuthenticationCompleted(null, null);
                        }
                    }
                    else if (result.Count > 0 && result.ContainsKey("denied") && result["denied"] == this.authToken)
                    {
                        this.ClosePopup();
                    }
                }
            }
        }

        /// <summary>
        /// This method obtains the response of the twitter 
        /// authorize token request.
        /// </summary>
        /// <param name="response">Response</param>
        private void GetAuthorizeTokenComplete(Response response)
        {
            TwitterAuthorizeTokenResponse twitterResponse = response as TwitterAuthorizeTokenResponse;
            this.authToken = twitterResponse.AuthToken; 
            this.authTokenSecretKey = twitterResponse.AuthTokenSecret;
            string outUrl = string.Empty;
            string querystring = string.Empty;

            string nonce = OAuthHelper.GenerateNonce();
            string timeStamp = OAuthHelper.GenerateTimestamp();

            // Generate Signature
            string sig = OAuthHelper.GenerateSignature(
                new Uri(Constants.TwitterConstants.AccessCredentialsUrl),
                this.ConsumerKey,
                this.ConsumerSecret,
                this.authToken,
                this.authTokenSecretKey,
                string.Empty,
                string.Empty,
                HttpOperation.Get.ToString(),
                timeStamp,
                nonce,
                out outUrl,
                out querystring);

            querystring += "&oauth_signature=" + OAuthHelper.EncodeLink(sig);

            this.requestUserDatatLink = outUrl + "?" + querystring;

            if (false == this.isPopUpClosed)
            {
                TwitterUserDataRequest request = new TwitterUserDataRequest();
                ApplicationData.Controller.ProcessRequest(request, GetUserDetailsComplete, this.HandleError);
            }
            else if (null != this.AuthenticationCompleted)
            {
                this.AuthenticationCompleted(null, null);
            }
        }

        /// <summary>
        /// This method obtains the response of the twitter 
        /// user data request.
        /// </summary>
        /// <param name="response">Response</param>
        private void GetUserDetailsComplete(Response response)
        {
            TwitterUserDataResponse twitterResponse = response as TwitterUserDataResponse;
            this.twitterUser = twitterResponse.TwitterUser;
            this.isLogged = true;

            App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.ClosePopup();
                }
            ));
            
            if (null != this.AuthenticationCompleted)
            {
                this.AuthenticationCompleted(null, null);
            }
        }

        /// <summary>
        /// This method obtains the response of the twitter 
        /// request token.
        /// </summary>
        /// <param name="response">Response</param>
        private void GetRequestTokenComplete(Response response)
        {                        
            TwitterTokenResponse twitterResponse = response as TwitterTokenResponse;
            
            //// Use the oauth token to authorize the user,i.e redirect him to the login page of twitter.
            string url = string.Format(CultureInfo.InvariantCulture, Constants.TwitterConstants.AuthorizeUrl, twitterResponse.AuthToken);

            this.authToken = twitterResponse.AuthToken;
            this.authTokenSecretKey = twitterResponse.AuthTokenSecret;

            if (this.loginControl != null)
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.loginControl.WebBrowser.Navigate(new Uri(url, UriKind.Absolute));
                }), null);
            }
        }

        /// <summary>
        /// Share item.
        /// </summary>
        /// <param name="message">Message</param>
        public void ShareMediaItem(string shortenedLink)
        {
            int twitterTitleLimit = Convert.ToInt32(Resources.TwitterTitleLimit);
            ShareMediaType shareType = ApplicationData.CurrentItem.ShareType;
            string shareMessage = string.Empty;

            SocialShareViewModel socialShareViewModel = new SocialShareViewModel();
            socialShareViewModel.ImageLink = Constants.TwitterConstants.TwitterImageLink;


            if (shareType == ShareMediaType.RecentMatches)
            {
                shareMessage = Resources.TwitterRecentMatchesShare;
            }
            else if (shareType == ShareMediaType.UpcomingMatches)
            {
                shareMessage = Resources.TwitterUpcomingMatchesShare;
            }
            else 
            {
                shareMessage = ApplicationData.CurrentItem.Caption;
            }

            socialShareViewModel.PostMessage = String.Format(CultureInfo.InvariantCulture, "'{0}' {1}",
                new object[] { Utility.TruncateString(shareMessage, twitterTitleLimit), shortenedLink });

            socialShareViewModel.ShareTitle = String.Format(CultureInfo.InvariantCulture, Resources.SocialShareTitle,
                new object[] { ApplicationData.CurrentItem.ShareType.ToString() });

            socialShareViewModel.CancelShare += CloseShareView;
            socialShareViewModel.SocialShareType = SocialInteractionType.Twitter;

            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                socialShareDialog = new SocialShareDialog();
                socialShareDialog.PostMessageText.MaxLength = 140;
                socialShareDialog.DataContext = socialShareViewModel;
                socialShareDialog.Owner = App.Current.MainWindow;
                socialShareDialog.ShowInTaskbar = false;
                socialShareDialog.ShowDialog();
            }));
        }

        /// <summary>
        /// Share a message on twitter
        /// </summary>
        /// <param name="message">Message</param>
        public void ShareOnTwitter(string message)
        {
            this.CloseSocialShare();

            string outUrl = string.Empty;
            string querystring = string.Empty;
            string nonce = OAuthHelper.GenerateNonce();
            string timeStamp = OAuthHelper.GenerateTimestamp();

            // Generate Signature using consumer secret and token secret as the user has to be logged in.
            string sig = OAuthHelper.GenerateSignature(
                new Uri(Constants.TwitterConstants.ShareUrl),
                this.ConsumerKey,
                this.ConsumerSecret,
                this.authToken,
                this.authTokenSecretKey,
                string.Empty,
                string.Empty,
                HttpOperation.Post.ToString(),
                timeStamp,
                nonce,
                "status",
                message,
                out outUrl,
                out querystring);

            message = string.Concat("status=" + OAuthHelper.EncodeLink(message));

            //// Create a header for status request and call the serivice to post the status.
            string header = OAuthHelper.GenerateOAuthHeader(OAuthHelper.OAuthNonceKey, nonce, OAuthHelper.OAuthSignatureMethodKey,
                OAuthHelper.HMACSHA1SignatureType, OAuthHelper.OAuthTimestampKey, timeStamp,
                OAuthHelper.OAuthConsumerKeyKey, this.ConsumerKey, OAuthHelper.OAuthTokenKey, this.authToken, OAuthHelper.OAuthSignatureKey,
                OAuthHelper.EncodeLink(sig), OAuthHelper.OAuthVersionKey, OAuthHelper.OAuthVersion);

            TwitterShareRequest request = new TwitterShareRequest(message, header);
            ApplicationData.Controller.ProcessRequest(request, this.ShareComplete, this.HandleError);

        }

        /// <summary>
        /// Share complete.
        /// </summary>
        /// <param name="response">Response</param>
        private void ShareComplete(Response response)
        {
        }

         /// <summary>
        /// This will show the error message
        /// </summary>  
        /// <param name="exception">Exception</param>
        private void HandleError(Exception exception)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (false == this.IsLogged)
                {
                    (App.Current as App).DisplayErrorMessage(String.Format(CultureInfo.InvariantCulture, Resources.ErrorLogIn,
                        SocialInteractionType.Twitter.ToString()), string.Empty, false, exception);
                }
                else
                {
                    (App.Current as App).DisplayErrorMessage(String.Format(CultureInfo.InvariantCulture, Resources.ErrorShare,
                        ApplicationData.CurrentItem.ShareType), string.Empty, false, exception);
                }
                this.LoginFailed();
            }), null);
        }

        /// <summary>
        /// Handle the login fail when user has denied access.
        /// </summary>
        private void LoginFailed()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (false == this.isPopUpClosed)
                {
                    this.ClosePopup();
                }
            }
            ), null);
        }

        /// <summary>
        /// Closes the popup after null check
        /// </summary>
        private void ClosePopup()
        {
            if (this.loginControl != null)
            {
                this.isPopUpClosed = true;
                this.loginControl.Close();
            }
            ApplicationData.IsPopUpOpen = false;
        }

        /// <summary>
        /// Event hanlder to capture the close of the pop up.
        /// </summary>
        /// <param name="sender">Login control</param>
        /// <param name="e">Event arguments</param>
        private void LoginControlClosed(object sender, EventArgs e)
        {
            this.loginControl.Closed -= LoginControlClosed;
            if (null != this.loginControl.WebBrowser)
            {
                this.loginControl.WebBrowser.Navigated -= WebBrowserNavigated;
                this.loginControl.WebBrowser.LoadCompleted -= WebBrowserLoadCompleted;
            }

            if (false == this.isPopUpClosed)
            {
                this.isPopUpClosed = true;
            }
            ApplicationData.IsPopUpOpen = false;
            if (null != this.DialogClosed)
            {
                this.DialogClosed(null, null);
            }
        }

        /// <summary>
        /// Handler invoked on cancel button click
        /// of the share view.
        /// </summary>
        /// <param name="sender">Share view</param>
        /// <param name="e">Event arguments</param>
        private void CloseShareView(object sender, EventArgs e)
        {
            this.CloseSocialShare();
        }

        /// <summary>
        /// Closes the popup after null check
        /// </summary>
        private void CloseSocialShare()
        {
            if (this.socialShareDialog != null)
            {
                this.socialShareDialog.Close();
                ApplicationData.IsPopUpOpen = false;
            }
        }

        /// <summary>
        /// Navigating log off url.
        /// </summary>
        /// <param name="sender"> Sender Object. </param>
        /// <param name="e">Event argument. </param>
        private void LogOffWebBrowserNavigated(object sender, NavigationEventArgs e)
        {
            if (null != this.loginControl && this.isPopUpClosed == false)
            {
                string link = this.loginControl.WebBrowser.Source.AbsoluteUri.ToString();

                if (this.isLogged && link.Contains(Constants.TwitterConstants.LogOffConfirmationText))
                {
                    /* log out confirmed link */
                    this.isLogged = false;
                    App.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.loginControl.Close();
                    }));
                }
                else if ((link.Equals(Constants.TwitterConstants.CancelLink)))
                {
                    /* cancel link */
                    App.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.loginControl.Close();
                    }));
                }
                else if (false == (true == link.Equals(Constants.TwitterConstants.LogOffLink) ||
                                    true == link.Equals(Constants.TwitterConstants.LogOffSecureLink)))
                {
                    /* Link other than logout */
                    App.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.loginControl.Close();
                    }));
                }
            }
        }

        /// <summary>
        ///   Load Completed.
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e"> Navigation Event Argument</param>
        private void LogOffBrowserLoadCompleted(object sender, NavigationEventArgs e)
        {

            if (null != this.loginControl && this.isPopUpClosed == false)
            {
                this.loginControl.preLoader.Visibility = Visibility.Collapsed;
                this.loginControl.WebBrowser.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Closing Log off pop.
        /// </summary>
        /// <param name="sender"> Sender Object </param>
        /// <param name="e"> Event argument. </param>
        private void OnLogOffPopupClose(object sender, EventArgs e)
        {
            this.loginControl.Closed -= OnLogOffPopupClose;
            this.loginControl.WebBrowser.Navigated -= LogOffWebBrowserNavigated;
            this.loginControl.WebBrowser.LoadCompleted -= LogOffBrowserLoadCompleted;

            if (false == this.isLogged)
            {
                this.accessToken = string.Empty;
                
                ApplicationData.TwitterAccount = null;
                ApplicationData.IsPopUpOpen = false;

                if (null != this.LogOffCompleted)
                {
                    this.LogOffCompleted.Invoke(this, null);
                }
            }
            this.isPopUpClosed = true;
            ApplicationData.IsPopUpOpen = false;
            if (null != this.DialogClosed)
            {
                this.DialogClosed(null, null);
            }
        }

        /// <summary>
        /// Log off from google. 
        /// </summary>
        public void LogOff()
        {
            this.isPopUpClosed = false;
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.loginControl = new SocialLogin();
                this.loginControl.SocialLoginWindow.HeaderText = Resources.LogoutText;
                this.loginControl.Owner = App.Current.MainWindow;
                this.loginControl.Closed += OnLogOffPopupClose;
                this.loginControl.WebBrowser.Navigate(new Uri(Constants.TwitterConstants.LogOffSecureLink, UriKind.Absolute));
                this.loginControl.WebBrowser.Navigated += LogOffWebBrowserNavigated;
                this.loginControl.WebBrowser.LoadCompleted += LogOffBrowserLoadCompleted;
                this.loginControl.ShowInTaskbar = false;
                this.loginControl.ShowDialog();

            }));
        }
    }
}
