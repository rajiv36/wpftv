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
    public class YahooAccount : Account
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
        /// The authorization token guid.
        /// </summary>
        private string authTokenGuid;

        /// <summary>
        /// The login control.
        /// </summary>        
        private SocialLogin loginControl;       

        /// <summary>
        /// The authentication completed event.
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
                return Utility.GetSocialData("YahooConsumerKey");
            }
        }

        /// <summary>
        /// Gets the google consumer secret key.
        /// </summary>
        public override string ConsumerSecret
        {
            get
            {
                return Utility.GetSocialData("YahooConsumerSecret");
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
        /// Gets the yahoo user.
        /// </summary>
        public YahooUser YahooUser
        {
            get;
            private set;
        }

        /// <summary>
        /// Authenticates a user.
        /// </summary>
        public override void Authenticate()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.loginControl = new SocialLogin();
                this.loginControl.WebBrowser.Navigated += WebBrowserNavigated;
                this.loginControl.Owner = App.Current.MainWindow;
                this.loginControl.Closed += LoginControlClosed;
                this.loginControl.WebBrowser.LoadCompleted += WebBrowserLoadCompleted;
                this.loginControl.ShowInTaskbar = false;
                this.loginControl.ShowDialog();
            }), null);

            string outUrl = string.Empty;
            string querystring = string.Empty;

            string nonce = OAuthHelper.GenerateNonce();
            string timeStamp = OAuthHelper.GenerateTimestamp();

            //// Generate Signature for request token.
            string sig = OAuthHelper.GenerateSignature(
                new Uri(Constants.YahooConstants.RequestTokenUrl),
                this.ConsumerKey,
                this.ConsumerSecret,
                string.Empty,
                string.Empty,
                string.Empty,
                Utility.GetSocialData("CallbackUrl"),
                HttpOperation.Get.ToString(),
                timeStamp,
                nonce,
                out outUrl,
                out querystring);
            querystring += "&oauth_signature=" + OAuthHelper.EncodeLink(sig);
            outUrl += "?" + querystring;

            YahooTokenRequest yahooTokenRequest = new YahooTokenRequest(outUrl);
            ApplicationData.Controller.ProcessRequest(yahooTokenRequest, this.GetRequestTokenComplete, this.HandleError);
        }

        /// <summary>
        /// Closed handler.
        /// </summary>
        /// <param name="sender">Login control</param>
        /// <param name="e">Event arguments</param>
        private void LoginControlClosed(object sender, EventArgs e)
        {
            this.loginControl.Closed -= LoginControlClosed;
            if (null != this.loginControl.webBrowserControl)
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
            YahooTokenResponse yahooResponse = response as YahooTokenResponse;
            this.authToken = yahooResponse.AuthToken;
            this.authTokenSecretKey = yahooResponse.AuthTokenSecret;

            //// Use the oauth token to authorize the user,i.e redirect him to the login page of twitter.
            string url = string.Format(System.Globalization.CultureInfo.InvariantCulture, Constants.YahooConstants.AuthorizeUrl, this.authToken);

            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (this.loginControl != null)
                {
                    this.loginControl.webBrowserControl.Navigate(new Uri(url, UriKind.Absolute));
                }
            }));
        }

        /// <summary>
        /// Handles the navigated method of the web browser.
        /// </summary>
        /// <param name="sender">sender - web browser</param>
        /// <param name="e">Navigation event arguments</param>
        private void WebBrowserNavigated(object sender, NavigationEventArgs e)
        {
            if (this.loginControl.webBrowserControl.Source.AbsoluteUri.Contains(OAuthHelper.OAuthVerifier))
            {
                Dictionary<string, string> result = Utility.GetQueryParameters(this.loginControl.webBrowserControl.Source.AbsoluteUri);

                if (null != result && result.Count > 0)
                {
                    this.authTokenVerifier = result[OAuthHelper.OAuthVerifier];
                    App.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.loginControl.webBrowserControl.Visibility = Visibility.Collapsed;
                        this.loginControl.preLoader.Visibility = Visibility.Visible;
                    }));
                }

                string outUrl = string.Empty;
                string querystring = string.Empty;

                string nonce = OAuthHelper.GenerateNonce();
                string timeStamp = OAuthHelper.GenerateTimestamp();

                //// Generate Signature to retrieve the access token in exchange for the authorize token.
                string sig = OAuthHelper.GenerateSignature(
                    new Uri(Constants.YahooConstants.AccessTokenUrl),
                    this.ConsumerKey,
                    this.ConsumerSecret,
                    this.authToken,
                    this.authTokenSecretKey,
                    this.authTokenVerifier,
                    string.Empty,
                    HttpOperation.Get.ToString(),
                    timeStamp,
                    nonce,
                    out outUrl,
                    out querystring);

                querystring += "&oauth_signature=" + OAuthHelper.EncodeLink(sig);
                outUrl += "?" + querystring;

                YahooAuthorizeTokenRequest request = new YahooAuthorizeTokenRequest(outUrl);
                ApplicationData.Controller.ProcessRequest(request, this.GetAuthorizeTokenComplete, this.HandleError);
            }
        }

        /// <summary>
        /// This method obtains the response of the twitter 
        /// request token.
        /// </summary>
        /// <param name="response">Response</param>
        private void GetAuthorizeTokenComplete(Response response)
        {
            YahooAuthorizeTokenResponse yahooResponse = response as YahooAuthorizeTokenResponse;
            this.authToken = yahooResponse.AuthToken;
            this.authTokenSecretKey = yahooResponse.AuthTokenSecret;
            this.authTokenGuid = yahooResponse.AuthYahooGuid;

            string outUrl = string.Empty;
            string querystring = string.Empty;

            string nonce = OAuthHelper.GenerateNonce();
            string timeStamp = OAuthHelper.GenerateTimestamp();

            string url = string.Format(System.Globalization.CultureInfo.InvariantCulture, Constants.YahooConstants.GetCredentialsUrl, this.authTokenGuid);

            // Generate Signature
            string sig = OAuthHelper.GenerateSignature(
                new Uri(url),
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
            outUrl += "?" + querystring;

            YahooUserDataRequest request = new YahooUserDataRequest(outUrl);

            ApplicationData.Controller.ProcessRequest(request, this.GetUserDetailsComplete, this.HandleError);
        }

        /// <summary>
        /// This method obtains the response of the twitter 
        /// request token.
        /// </summary>
        /// <param name="response">Response</param>
        private void GetUserDetailsComplete(Response response)
        {
            YahooUserDataResponse yahooResponse = response as YahooUserDataResponse;
            this.YahooUser = yahooResponse.YahooUser;
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
        /// This will show the error message
        /// </summary>  
        /// <param name="exception">Exception</param>
        private void HandleError(Exception exception)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
           {
               (App.Current as App).DisplayErrorMessage(String.Format(CultureInfo.InvariantCulture, Resources.ErrorLogIn,
                           LoginMode.Yahoo.ToString()), string.Empty, false, exception);
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
        /// Browser load complete.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Navigation event arguments</param>
        private void WebBrowserLoadCompleted(object sender, NavigationEventArgs e)
        {
            if (null != this.loginControl )
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
            if (null != this.loginControl )
            {
                this.loginControl.WebBrowser.LoadCompleted -= LogOffBrowserLoadCompleted;
                this.accessToken = string.Empty;
                this.isLogged = false;

                ApplicationData.YahooAccount = null;
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
        /// Log off from google. 
        /// </summary>
        public void LogOff()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.loginControl = new SocialLogin();
                this.loginControl.SocialLoginWindow.HeaderText = Resources.LogoutText;
                this.loginControl.Owner = App.Current.MainWindow;
                this.loginControl.Closed += LoginControlClosed;
                this.loginControl.WebBrowser.Navigate(new Uri(Constants.YahooConstants.LogOffLink, UriKind.Absolute));
                this.loginControl.WebBrowser.LoadCompleted += LogOffBrowserLoadCompleted;
                this.loginControl.ShowInTaskbar = false;
                this.loginControl.ShowDialog();
            }));
        }
    }
}
