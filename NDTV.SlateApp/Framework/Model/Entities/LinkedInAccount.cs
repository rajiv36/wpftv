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
    public class LinkedInAccount : Account
    {
        /// <summary>
        /// Event that gets raised when logout is completed.
        /// </summary>
        public override event EventHandler<EventArgs> AuthenticationCompleted;
      
        /// <summary>
        /// Gets the social user details.
        /// </summary>
        private LinkedInUser linkedInUser;
        public LinkedInUser LinkedInUser
        {
            get
            {
                return this.linkedInUser;
            }
        }

        /// <summary>
        /// The social share dialog.
        /// </summary>
        private SocialShareDialog socialShareDialog;

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
        /// The user data request link.
        /// </summary>
        private string requestUserDatatLink;

        /// <summary>
        /// The login control.
        /// </summary>        
        private SocialLogin loginControl;      

        /// <summary>
        /// Gets the linkedIn consumer key.
        /// </summary>
        public override string ConsumerKey
        {
            get
            {
                return Utility.GetSocialData("LinkedInConsumerKey");
            }
        }

        /// <summary>
        /// Gets the linkedIn consumer secret key.
        /// </summary>
        public override string ConsumerSecret
        {
            get
            {
                return Utility.GetSocialData("LinkedInConsumerSecretKey");
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
                this.loginControl.ShowInTaskbar = false;
                this.loginControl.Closed += LoginControlClosed;
                this.loginControl.ShowDialog();
            }), null);

            string outUrl = string.Empty;
            string querystring = string.Empty;
            string nonce = OAuthHelper.GenerateNonce();
            string timeStamp = OAuthHelper.GenerateTimestamp();

            // Generate Signature for request token.
            string sig = OAuthHelper.GenerateSignature(
                new Uri(Constants.LinkedInConstants.RequestTokenLink),
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
            
            LinkedInTokenRequest tokenRequest = new LinkedInTokenRequest(outUrl, header);

            ApplicationData.Controller.ProcessRequest(tokenRequest, GetRequestTokenComplete, this.HandleError);
        }
       
        /// <summary>
        /// Event hanlder to capture the close of the pop up.
        /// </summary>
        /// <param name="sender">Login control</param>
        /// <param name="e">Event arguments</param>
        private void LoginControlClosed(object sender, EventArgs e)
        {
            ApplicationData.IsPopUpOpen = false;
            if (null != this.loginControl && null != this.loginControl.WebBrowser)
            {
                this.loginControl.WebBrowser.Navigated -= WebBrowserNavigated;
                this.loginControl.WebBrowser.LoadCompleted -= WebBrowserLoadCompleted;
            }
        }

        /// <summary>
        /// Browser load complete.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Navigation event arguments</param>
        private void WebBrowserLoadCompleted(object sender, NavigationEventArgs e)
        {
            if (null != this.loginControl && null != this.loginControl.WebBrowser.Source)
            {
                if (false == this.loginControl.WebBrowser.Source.AbsoluteUri.Contains(Utility.GetSocialData("CallbackUrl")))
                {
                    this.loginControl.preLoader.Visibility = Visibility.Collapsed;
                    this.loginControl.WebBrowser.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Handles the navigated method of the web browser.
        /// </summary>
        /// <param name="sender">sender - web browser</param>
        /// <param name="e">Navigation event arguments</param>
        private void WebBrowserNavigated(object sender, NavigationEventArgs e)
        {
            if (null != this.loginControl && null != this.loginControl.WebBrowser.Source)
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
                            new Uri(Constants.LinkedInConstants.AccessTokenUrl),
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
                        
                        LinkedInAuthorizeTokenRequest request = new LinkedInAuthorizeTokenRequest(outUrl, header);
                        ApplicationData.Controller.ProcessRequest(request, this.GetAuthorizeTokenComplete, this.HandleError);
                    }
                }
            }
        }

        /// <summary>
        /// This method obtains the response of the linkedIn 
        /// authorize token request.
        /// </summary>
        /// <param name="response">Response</param>
        private void GetAuthorizeTokenComplete(Response response)
        {
            LinkedInAuthorizeTokenResponse linkedInResponse = response as LinkedInAuthorizeTokenResponse;
            this.authToken = linkedInResponse.AuthToken;
            this.authTokenSecretKey = linkedInResponse.AuthTokenSecret;

            string outUrl = string.Empty;
            string querystring = string.Empty;
            string nonce = OAuthHelper.GenerateNonce();
            string timeStamp = OAuthHelper.GenerateTimestamp();

            // Generate Signature
            string sig = OAuthHelper.GenerateSignature(
                new Uri(Constants.LinkedInConstants.AccessCredentialsUrl),
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
            LinkedInUserDataRequest request = new LinkedInUserDataRequest(this.requestUserDatatLink);
            ApplicationData.Controller.ProcessRequest(request, this.GetUserDetailsComplete, this.HandleError);
        }

        /// <summary>
        /// This method obtains the response of the linkedIn 
        /// user data request.
        /// </summary>
        /// <param name="response">Response</param>
        private void GetUserDetailsComplete(Response response)
        {
            LinkedInUserDataResponse linkedInResponse = response as LinkedInUserDataResponse;
            this.linkedInUser = linkedInResponse.LinkedInUser;
            this.isLogged = true;

            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.ClosePopup();
            }));
            
            if (null != this.AuthenticationCompleted)
            {
                this.AuthenticationCompleted(null, null);
            }
        }

        /// <summary>
        /// This method obtains the response of the linkedIn 
        /// request token.
        /// </summary>
        /// <param name="response">Response</param>
        private void GetRequestTokenComplete(Response response)
        {
            LinkedInTokenResponse linkedInResponse = response as LinkedInTokenResponse;

            //// Use the oauth token to authorize the user,i.e redirect him to the login page of linkedIn.
            string url = string.Format(CultureInfo.InvariantCulture, Constants.LinkedInConstants.AuthorizeUrl, linkedInResponse.AuthToken);

            this.authToken = linkedInResponse.AuthToken;
            this.authTokenSecretKey = linkedInResponse.AuthTokenSecret;

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
        public void Share()
        {
            SocialShareViewModel socialShareViewModel = new SocialShareViewModel();
            socialShareViewModel.ImageLink = Constants.LinkedInConstants.LinkedInImageLink;
            socialShareViewModel.PostMessage = GetShareMessage();
            socialShareViewModel.ShareTitle = String.Format(CultureInfo.InvariantCulture, Resources.SocialShareTitle,
                new object[] { ApplicationData.CurrentItem.ShareType.ToString() });
            socialShareViewModel.CancelShare += CloseShareView;
            socialShareViewModel.SocialShareType = SocialInteractionType.LinkedIn;

            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                socialShareDialog = new SocialShareDialog();
                socialShareDialog.DataContext = socialShareViewModel;
                socialShareDialog.Owner = App.Current.MainWindow;
                socialShareDialog.ShowInTaskbar = false;
                socialShareDialog.ShowDialog();
            }));
        }

        /// <summary>
        /// Gets the share message based on the type of content
        /// to be shared.
        /// </summary>
        private static string GetShareMessage()
        {
            string shareMessage = string.Empty;
            switch (ApplicationData.CurrentItem.ShareType)
            {
                case ShareMediaType.Article:
                    shareMessage = String.Format(CultureInfo.InvariantCulture, Resources.LinkedInArticleShareMessage,
                        new object[] { ApplicationData.CurrentItem.Caption });
                    break;
                case ShareMediaType.Video:
                    shareMessage = String.Format(CultureInfo.InvariantCulture, Resources.LinkedInVideoShareMessage,
                       new object[] { ApplicationData.CurrentItem.Caption });
                    break;
                case ShareMediaType.PhotoSet:
                    shareMessage = String.Format(CultureInfo.InvariantCulture, Resources.LinkedInPhotoSetShareMessage,
                       new object[] { ApplicationData.CurrentItem.Caption });
                    break;
                case ShareMediaType.ScorecardLiveMatch:
                    shareMessage = String.Format(CultureInfo.InvariantCulture, Resources.LinkedInCricketLiveMatchShareMessage,
                        new object[] { ApplicationData.CurrentItem.Caption });
                    break;
                case ShareMediaType.ScorecardRecentMatch:
                    shareMessage = String.Format(CultureInfo.InvariantCulture, Resources.LinkedInCricketRecentMatchShareMessage,
                        new object[] { ApplicationData.CurrentItem.Caption });
                    break;
                case ShareMediaType.RecentMatches:
                    shareMessage = Resources.LinkedInRecentMatchesShare;
                    break;
                case ShareMediaType.UpcomingMatches:
                    shareMessage = Resources.LinkedInUpcomingMatchesShare;
                    break;
                case ShareMediaType.Weather:
                    shareMessage = Resources.LinkedInWeatherShareMessage;
                    break;

            }
            return shareMessage;
        }


        /// <summary>
        ///  Formating Data in predefine xml format. 
        /// </summary>
        /// <param name="comment"> Description. </param>
        /// <param name="title"> Title or Caption tobe displayed. </param>
        /// <param name="link"> URL of the content to be shared. </param>
        /// <param name="imageLink"> URL of the Image to be displayed in share.</param>
        /// <param name="visibility"> Visiblity (anyone or connections-only) of share. </param>
        /// <returns></returns>
        private static string EncodeShareInXml(string comment, string title, string link, string imageLink, string visibility)
        {
            string encodedString = string.Empty;

            encodedString = string.Format(CultureInfo.InvariantCulture,
                                          Constants.LinkedInConstants.ShareFormat,
                                          comment, title, link, imageLink, visibility);

            return encodedString;
        }

        /// <summary>
        /// Share item.
        /// </summary>
        /// <param name="message">Message</param>
        public void ShareMediaItem(string message)
        {
            this.CloseSocialShare();

            string outUrl = string.Empty;
            string querystring = string.Empty;
            string nonce = OAuthHelper.GenerateNonce();
            string timeStamp = OAuthHelper.GenerateTimestamp();
            
            string shareXml = EncodeShareInXml(message, ApplicationData.CurrentItem.Caption, ApplicationData.CurrentItem.Link, ApplicationData.CurrentItem.ImageLink,
                Constants.LinkedInConstants.LinkedInShareVisibilityType.AnyOne);
            
            string sig = OAuthHelper.GenerateSignature(
                new Uri(Constants.LinkedInConstants.ShareUrl),
                this.ConsumerKey,
                this.ConsumerSecret,
                this.authToken,
                this.authTokenSecretKey,
                string.Empty,
                string.Empty,
                HttpOperation.Post.ToString(),
                timeStamp,
                nonce,
                out outUrl,
                out querystring);         

            //// Create a header for status request and call the serivice to post the status.
            string header = OAuthHelper.GenerateOAuthHeader(                        
                        OAuthHelper.OAuthTokenKey, this.authToken,
                        OAuthHelper.OAuthConsumerKeyKey, this.ConsumerKey,
                        OAuthHelper.OAuthNonceKey, nonce,
                        OAuthHelper.OAuthSignatureMethodKey, OAuthHelper.HMACSHA1SignatureType,
                        OAuthHelper.OAuthSignatureKey, OAuthHelper.EncodeLink(sig),                        
                        OAuthHelper.OAuthVersionKey, OAuthHelper.OAuthVersion,
                        OAuthHelper.OAuthTimestampKey, timeStamp);

            LinkedInShareRequest request = new LinkedInShareRequest(shareXml, header);
            ApplicationData.Controller.ProcessRequest(request, this.ShareComplete, this.HandleError);          
        }
      
        /// <summary>
        /// Share complete.
        /// </summary>
        /// <param name="response">Response</param>
        private void ShareComplete(Response response)
        {
            LinkedInShareResponse linkedInResponse = response as LinkedInShareResponse;
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
        /// This will show the error message
        /// </summary>  
        /// <param name="exception">Exception</param>
        private void HandleError(Exception exception)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (false == this.isLogged)
                {
                    (App.Current as App).DisplayErrorMessage(String.Format(CultureInfo.InvariantCulture, Resources.ErrorLogIn,
                        SocialInteractionType.LinkedIn.ToString()), string.Empty, false, exception);
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
    }
}
