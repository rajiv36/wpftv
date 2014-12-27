
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Navigation;
using NDTV.Controller;
using NDTV.SlateApp;
using NDTV.SlateApp.Properties;
using NDTV.SlateApp.View;
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class GoogleAccount : Account
    {
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
        /// Authentication completed handler.
        /// </summary>
        public override event EventHandler<EventArgs> AuthenticationCompleted;

        /// <summary>
        /// Event that gets raised when logout is completed.
        /// </summary>
        public event EventHandler<EventArgs> LogOffCompleted;

        /// <summary>
        /// Dialog closed event.
        /// </summary>
        public event EventHandler DialogClosed;

        /// <summary>
        /// Gets the google consumer key.
        /// </summary>
        public override string ConsumerKey
        {
            get
            {
                return Utility.GetSocialData("GoogleConsumerKey");
            }
        }

        /// <summary>
        /// Gets the google consumer secret key.
        /// </summary>
        public override string ConsumerSecret
        {
            get
            {
                return Utility.GetSocialData("GoogleConsumerSecret");
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
        /// The login control.
        /// </summary>        
        private SocialLogin loginControl;       

        /// <summary>
        /// Gets the google user details.
        /// </summary>
        public GoogleUser GoogleUser
        {
            get;
            private set;
        }

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        public override void Authenticate()
        {
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

            string postdata = Constants.GoogleConstants.RequestTokenPostData;

            // Generate Signature for request token.
            string sig = OAuthHelper.GenerateSignature(new Uri(Constants.GoogleConstants.RequestTokenUrl),
                this.ConsumerKey,
                this.ConsumerSecret,
                string.Empty,
                string.Empty,
                string.Empty,
                Utility.GetSocialData("CallbackUrl"),
                HttpOperation.Post.ToString(),
                timeStamp,
                nonce,
                "scope",
                postdata,
                out outUrl,
                out querystring);

            postdata = "scope=" + postdata;

            // Generate the header for request token.
            string header = OAuthHelper.GenerateOAuthHeader(OAuthHelper.OAuthNonceKey, nonce, OAuthHelper.OAuthSignatureMethodKey, OAuthHelper.HMACSHA1SignatureType, 
                OAuthHelper.OAuthTimestampKey, timeStamp, OAuthHelper.OAuthConsumerKeyKey, 
                this.ConsumerKey, OAuthHelper.OAuthCallbackKey, Uri.EscapeUriString(Utility.GetSocialData("CallbackUrl")), 
                OAuthHelper.OAuthSignatureKey, OAuthHelper.EncodeLink(sig), OAuthHelper.OAuthVersionKey, OAuthHelper.OAuthVersion);

            GoogleTokenRequest request = new GoogleTokenRequest(header, postdata);
            ApplicationData.Controller.ProcessRequest(request, this.GetRequestTokenComplete, this.HandleError);

        }

        /// <summary>
        /// Closed handler.
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
            if (null != this.DialogClosed)
            {
                this.DialogClosed(null, null);
                ApplicationData.IsPopUpOpen = false;
            }
        }

        /// <summary>
        /// This method obtains the response of the twitter 
        /// request token.
        /// </summary>
        /// <param name="response">Response</param>
        private void GetRequestTokenComplete(Response response)
        {
            GoogleTokenResponse googleResponse = response as GoogleTokenResponse;
            this.authToken = googleResponse.AuthToken;
            this.authTokenSecretKey = googleResponse.AuthTokenSecret;


            //// Use the oauth token to authorize the user,i.e redirect him to the login page of Google.
            string url = string.Format(System.Globalization.CultureInfo.InvariantCulture, Constants.GoogleConstants.AuthorizeUrl, this.authToken);

            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (this.loginControl != null)
                {
                    this.loginControl.webBrowserControl.Navigate(new Uri(url, UriKind.Absolute));
                }
            }));
            
        }

        /// <summary>
        /// This will show the error message
        /// </summary>  
        /// <param name="exception">Exception</param>
        private void HandleError(Exception exception)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                (App.Current as App).DisplayErrorMessage(String.Format(CultureInfo.InvariantCulture, Resources.ErrorLogIn,
                            LoginMode.Google.ToString()), string.Empty, false, exception);
                this.LoginFailed();
            })); 
        }

        /// <summary>
        /// Handle the login fail when user has denied access.
        /// </summary>
        private void LoginFailed()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.ClosePopup();
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
                this.loginControl.Close();
            }
            ApplicationData.IsPopUpOpen = false;
        }

         /// <summary>
        /// Handles the navigated method of the web browser.
        /// </summary>
        /// <param name="sender">sender - web browser</param>
        /// <param name="e">Navigation event arguments</param>
        private void WebBrowserNavigated(object sender, NavigationEventArgs e)
        {
            if (null != this.loginControl && this.loginControl.webBrowserControl.Source.AbsoluteUri.Contains(OAuthHelper.OAuthVerifier))
            {
                Dictionary<string, string> result = Utility.GetQueryParameters(this.loginControl.webBrowserControl.Source.AbsoluteUri);
                if (result != null && result.Count > 0)
                {
                    this.authTokenVerifier = result[OAuthHelper.OAuthVerifier];

                    App.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.loginControl.preLoader.Visibility = Visibility.Visible;
                            this.loginControl.webBrowserControl.Visibility = Visibility.Collapsed;                            
                        }));

                    string outUrl = string.Empty;
                    string querystring = string.Empty;

                    string nonce = OAuthHelper.GenerateNonce();
                    string timeStamp = OAuthHelper.GenerateTimestamp();

                    //// Generate Signature to retrieve the access token in exchange for the authorize token.
                    string sig = OAuthHelper.GenerateSignature(
                        new Uri(Constants.GoogleConstants.AccessTokenUrl),
                        this.ConsumerKey,
                        this.ConsumerSecret,
                        this.authToken,
                        Uri.UnescapeDataString(this.authTokenSecretKey),
                        this.authTokenVerifier,
                        string.Empty,
                        HttpOperation.Post.ToString(),
                        timeStamp,
                        nonce,
                        out outUrl,
                        out querystring);

                    //// Create a header and call for exchanging the access token using the authorize token (the token + verifier).
                    string header = OAuthHelper.GenerateOAuthHeader(OAuthHelper.OAuthNonceKey, nonce, OAuthHelper.OAuthSignatureMethodKey, 
                        OAuthHelper.HMACSHA1SignatureType, OAuthHelper.OAuthTimestampKey, timeStamp, 
                        OAuthHelper.OAuthConsumerKeyKey, this.ConsumerKey, OAuthHelper.OAuthTokenKey, 
                        this.authToken, OAuthHelper.OAuthVerifier, this.authTokenVerifier, OAuthHelper.OAuthSignatureKey, 
                        OAuthHelper.EncodeLink(sig), OAuthHelper.OAuthVersionKey, OAuthHelper.OAuthVersion);

                    GoogleAuthorizeTokenRequest request = new GoogleAuthorizeTokenRequest(header);
                    ApplicationData.Controller.ProcessRequest(request, this.GetAuthorizeTokenComplete, this.HandleError);
                }
            }
        }

        /// <summary>
        /// This method obtains the response of the twitter 
        /// request token.
        /// </summary>
        /// <param name="response">Response</param>
        private void GetAuthorizeTokenComplete(Response response)
        {
            GoogleAuthorizeTokenResponse googleResponse = response as GoogleAuthorizeTokenResponse;
            this.authToken = googleResponse.AuthToken;
            this.authTokenSecretKey = googleResponse.AuthTokenSecret;

            string outUrl = string.Empty;
            string querystring = string.Empty;

            string nonce = OAuthHelper.GenerateNonce();
            string timeStamp = OAuthHelper.GenerateTimestamp();

            string sig = OAuthHelper.GenerateSignature(
            new Uri(Constants.GoogleConstants.GetCredentialsUrl),
            this.ConsumerKey,
            this.ConsumerSecret,
            this.authToken,
            Uri.UnescapeDataString(this.authTokenSecretKey),
            string.Empty,
            string.Empty,
            HttpOperation.Get.ToString(),
            timeStamp,
            nonce,
            out outUrl,
            out querystring);
            
            // Create a header and call for exchanging the access token using the authorize token (the token + verifier).                            
            string header = OAuthHelper.GenerateOAuthHeader(OAuthHelper.OAuthVersionKey, OAuthHelper.OAuthVersion, OAuthHelper.OAuthNonceKey, nonce, 
                OAuthHelper.OAuthTimestampKey, timeStamp, OAuthHelper.OAuthConsumerKeyKey, this.ConsumerKey, OAuthHelper.OAuthTokenKey, 
                this.authToken, OAuthHelper.OAuthSignatureMethodKey, OAuthHelper.HMACSHA1SignatureType, OAuthHelper.OAuthSignatureKey, OAuthHelper.EncodeLink(sig));

            GoogleUserDetailsRequest request = new GoogleUserDetailsRequest(header);
            ApplicationData.Controller.ProcessRequest(request, this.GetUserDetailsComplete, this.HandleError);
        }

        /// <summary>
        /// This method obtains the response of the twitter 
        /// request token.
        /// </summary>
        /// <param name="response">Response</param>
        private void GetUserDetailsComplete(Response response)
        {
            GoogleUserDetailsResponse googleResponse = response as GoogleUserDetailsResponse;
            this.GoogleUser = googleResponse.User;
            this.isLogged = true;

            App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.loginControl.Close();
                }));

            if (null != this.AuthenticationCompleted)
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
            if (null != this.loginControl)
            {
                if (false == this.loginControl.WebBrowser.Source.AbsoluteUri.Contains(Utility.GetSocialData("CallbackUrl")))
                {
                    this.loginControl.preLoader.Visibility = Visibility.Collapsed;
                    this.loginControl.WebBrowser.Visibility = Visibility.Visible;
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
            
            if (null != this.loginControl)
            {
                this.loginControl.WebBrowser.LoadCompleted -= LogOffBrowserLoadCompleted;

                this.accessToken = string.Empty;
                this.isLogged = false;

                ApplicationData.GoogleAccount = null;
                ApplicationData.IsPopUpOpen = false;

                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.loginControl.Close();
                }));

                if (null != this.LogOffCompleted)
                {
                    this.LogOffCompleted.Invoke(this, null);
                }
            }
        }

        /// <summary>
        ///  Log off from google. 
        /// </summary>
        public void LogOff()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.loginControl = new SocialLogin();
                this.loginControl.SocialLoginWindow.HeaderText = Resources.LogoutText;
                this.loginControl.Owner = App.Current.MainWindow;
                this.loginControl.Closed += LoginControlClosed;
                this.loginControl.WebBrowser.Navigate(new Uri(Constants.GoogleConstants.LogOffLink, UriKind.Absolute));
                this.loginControl.WebBrowser.LoadCompleted += LogOffBrowserLoadCompleted;
                this.loginControl.ShowInTaskbar = false;
                this.loginControl.ShowDialog();
            }));
        }
    }
}
