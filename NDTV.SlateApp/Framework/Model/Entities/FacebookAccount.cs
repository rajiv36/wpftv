using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using NDTV.Controller;
using NDTV.SlateApp;
using NDTV.SlateApp.Framework.Utilities;
using NDTV.SlateApp.Properties;
using NDTV.SlateApp.View;
using NDTV.SlateApp.ViewModel;
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class FacebookAccount : Account
    {
        /// <summary>
        /// The login control.
        /// </summary>        
        private SocialLogin loginControl;

        /// <summary>
        /// The social share dialog.
        /// </summary>
        private SocialShareDialog socialShareDialog;

        /// <summary>
        /// The access token link.
        /// </summary>
        private string accessTokenLink;

        /// <summary>
        /// The credentials link.
        /// </summary>
        private string credentialsLink;

        /// <summary>
        /// Indicates if the pop up is closed.
        /// </summary>
        private bool isPopUpClosed;

        /// <summary>
        /// Event that gets raised when logout is completed.
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
        /// Indicates when the token expires.
        /// </summary>
        private string expires;

        /// <summary>
        /// Gets the consumer key.
        /// </summary>        
        public override string ConsumerKey
        {
            get
            {
                return Utility.GetSocialData("FacebookConsumerKey");
            }
        }

        /// <summary>
        /// Gets the consumer secret key.
        /// </summary>        
        public override string ConsumerSecret
        {
            get
            {
                return Utility.GetSocialData("FacebookConsumerSecretKey");
            }
        }

        /// <summary>
        /// Gets logged in state.
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
        /// Gets the social user details.
        /// </summary>
        private FacebookUser facebookUser;
        public FacebookUser FacebookUser
        {
            get
            {
                return this.facebookUser;
            }
        }

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        public override void Authenticate()
        {
            this.isPopUpClosed = false;

            string url = string.Format(CultureInfo.InvariantCulture, Constants.FacebookConstants.AuthorizeUrl,
                new object[] { this.ConsumerKey, Utility.GetSocialData("CallbackUrl") });

            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.loginControl = new SocialLogin();
                this.loginControl.Owner = App.Current.MainWindow;
                this.loginControl.WebBrowser.Navigated += WebBrowserNavigated;
                this.loginControl.webBrowserControl.LoadCompleted += WebBrowserLoadCompleted;
                this.loginControl.Closed += LoginControlClosed;
                this.loginControl.WebBrowser.Navigate(url);
                this.loginControl.ShowInTaskbar = false;
                this.loginControl.ShowDialog();
            }));
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
            if (null != loginControl.WebBrowser.Source && loginControl.WebBrowser.Source.AbsoluteUri.Contains("code") && this.isPopUpClosed == false)
            {
                Dictionary<string, string> result = Utility.GetQueryParameters(loginControl.WebBrowser.Source.AbsoluteUri);
                if (result != null && result.Count > 0)
                {
                    if (result.ContainsKey("code"))
                    {
                        App.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.loginControl.preLoader.Visibility = Visibility.Visible;
                            this.loginControl.webBrowserControl.Visibility = Visibility.Collapsed;
                        }));

                        this.accessTokenLink = string.Format(CultureInfo.InvariantCulture, Constants.FacebookConstants.AccessTokenUrl,
                            new object[] { this.ConsumerKey, Utility.GetSocialData("CallbackUrl"), this.ConsumerSecret, result["code"] });

                        if (false == this.isPopUpClosed)
                        {
                            FacebookAccessTokenRequest tokenRequest = new FacebookAccessTokenRequest(this.accessTokenLink);
                            ApplicationData.Controller.ProcessRequest(tokenRequest, this.ObtainAccessTokenData, this.HandleError);
                        }
                        else if (null != this.AuthenticationCompleted)
                        {
                            this.AuthenticationCompleted(null, null);
                        }
                    }
                }
            }
            else if (null != loginControl.WebBrowser.Source && loginControl.WebBrowser.Source.AbsoluteUri.Contains("client_id")
                && loginControl.WebBrowser.Source.AbsoluteUri.Contains("redirect_uri") && this.isPopUpClosed == false)
            {
                if (null != this.AuthenticationCompleted)
                {
                    App.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.HandleError(null);
                    }));
                    this.AuthenticationCompleted(null, null);
                }
            }

        }

        /// <summary>
        /// This method obtains the response of the facebook 
        /// access token request.
        /// </summary>
        /// <param name="response">Response</param>
        private void ObtainAccessTokenData(Response response)
        {
            if (false == this.isPopUpClosed)
            {
                FacebookAccessTokenResponse tokenReponse = response as FacebookAccessTokenResponse;
                this.credentialsLink = string.Format(CultureInfo.InvariantCulture, Constants.FacebookConstants.AccessCredentialsUrl,
                    new object[] { tokenReponse.AccessToken, tokenReponse.Expires });
                this.expires = tokenReponse.Expires;
                this.accessToken = tokenReponse.AccessToken;
                FacebookUserDataRequest userDataRequest = new FacebookUserDataRequest(this.credentialsLink);
                ApplicationData.Controller.ProcessRequest(userDataRequest, this.ObtainUserData, this.HandleError);
            }
            else if (null != this.AuthenticationCompleted)
            {
                this.AuthenticationCompleted(null, null);
            }
        }

        /// <summary>
        /// This method obtains the response of the facebook 
        /// user data request.
        /// </summary>
        /// <param name="response">Response</param>
        private void ObtainUserData(Response response)
        {
            if (false == this.isPopUpClosed)
            {
                FacebookUserDataResponse userDataReponse = response as FacebookUserDataResponse;
                this.facebookUser = userDataReponse.FacebookUser;
                this.isLogged = true;
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.ClosePopup();
                }));
            }

            if (null != this.AuthenticationCompleted)
            {
                AuthenticationCompleted(null, null);
            }
        }

        /// <summary>
        /// Event hanlder to capture the close of the pop up.
        /// </summary>
        /// <param name="sender">Login control</param>
        /// <param name="e">Event arguments</param>
        private void LoginControlClosed(object sender, EventArgs e)
        {
            if (null != this.loginControl.WebBrowser)
            {
                this.loginControl.WebBrowser.Navigated -= WebBrowserNavigated;
                this.loginControl.webBrowserControl.LoadCompleted -= WebBrowserLoadCompleted;
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
        /// Shares the specified message.
        /// </summary>        
        public void ShareMediaItem()
        {
            SocialShareViewModel socialShareViewModel = new SocialShareViewModel();
            socialShareViewModel.ImageLink = Constants.FacebookConstants.FacebookImageLink;
            socialShareViewModel.PostMessage = GetShareMessage();
            socialShareViewModel.ShareTitle = String.Format(CultureInfo.InvariantCulture, Resources.SocialShareTitle,
                new object[] { ApplicationData.CurrentItem.ShareType.ToString() });
            socialShareViewModel.CancelShare += CloseShareView;
            socialShareViewModel.SocialShareType = SocialInteractionType.Facebook;

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
                    shareMessage = String.Format(CultureInfo.InvariantCulture, Resources.FacebookArticleShareMessage,
                        new object[] { ApplicationData.CurrentItem.Caption });
                    break;
                case ShareMediaType.Video:
                    shareMessage = String.Format(CultureInfo.InvariantCulture, Resources.FacebookVideoShareMessage,
                       new object[] { ApplicationData.CurrentItem.Caption });
                    break;
                case ShareMediaType.PhotoSet:
                    shareMessage = String.Format(CultureInfo.InvariantCulture, Resources.FacebookPhotoSetShareMessage,
                       new object[] { ApplicationData.CurrentItem.Caption });
                    break;
                case ShareMediaType.ScorecardLiveMatch:
                    shareMessage = String.Format(CultureInfo.InvariantCulture, Resources.FacebookCricketLiveMatchShareMessage,
                        new object[] { ApplicationData.CurrentItem.Caption });
                    break;
                case ShareMediaType.ScorecardRecentMatch:
                    shareMessage = String.Format(CultureInfo.InvariantCulture, Resources.FacebookCricketRecentMatchShareMessage,
                        new object[] { ApplicationData.CurrentItem.Caption });
                    break;
                case ShareMediaType.RecentMatches:
                    shareMessage = Resources.FacebookRecentMatchesShare;
                    break;
                case ShareMediaType.UpcomingMatches:
                    shareMessage = Resources.FacebookUpcomingMatchesShare;
                    break;
                case ShareMediaType.Weather:
                    shareMessage = Resources.FacebookWeather;
                    break;
            }
            return shareMessage;
        }

        /// <summary>
        /// This method formats the share data and then makes a call to share the 
        /// same on facebook.
        /// </summary>
        /// <param name="message">Share message</param>
        /// <param name="link">Media item link</param>
        /// <param name="imageLink">Image link</param>
        /// <param name="description">Description</param>
        /// <param name="caption">Caption</param>
        public void ShareMediaItem(string message, string link, string imageLink, string description, string caption)
        {
            this.CloseSocialShare();

            StringBuilder postdata = new StringBuilder();
            if (!string.IsNullOrEmpty(link))
            {
                postdata.AppendFormat(CultureInfo.InvariantCulture, "link={0}", Uri.EscapeUriString(link));
            }
            if (!string.IsNullOrEmpty(imageLink))
            {
                postdata.AppendFormat(CultureInfo.InvariantCulture, "&picture={0}", imageLink);
            }
            if (!string.IsNullOrEmpty(description))
            {
                postdata.AppendFormat(CultureInfo.InvariantCulture, "&description={0}", Uri.EscapeUriString(description));
            }
            if (!string.IsNullOrEmpty(caption))
            {
                postdata.AppendFormat(CultureInfo.InvariantCulture, "&caption={0}", Uri.EscapeUriString(caption));
            }
            if (!string.IsNullOrEmpty(message))
            {
                postdata.AppendFormat(CultureInfo.InvariantCulture, "&message={0}", Uri.EscapeUriString(message));
            }

            this.Share(postdata.ToString());
        }

        /// <summary>
        /// This method shares data on facebook.
        /// </summary>
        /// <param name="status">Status message to share</param>
        private void Share(string status)
        {
            // Edit the status to postdata format if it is not in fb postdata format: message = status.
            if (!string.IsNullOrEmpty(status))
            {
                if (!status.Contains("message="))
                {
                    status = string.Concat("message=", Uri.EscapeUriString(status));
                }

                // Post comment only is status is not null or empty.                
                string shareLink = string.Format(CultureInfo.InvariantCulture, Constants.FacebookConstants.ShareUrl, this.facebookUser.Id,
                    this.accessToken, this.expires);

                FacebookShareRequest shareRequest = new FacebookShareRequest(shareLink, status);
                ApplicationData.Controller.ProcessRequest(shareRequest, ObtainShareResponse, this.HandleError);
            }
        }

        /// <summary>
        /// Response recieved on share.
        /// </summary>
        /// <param name="response">Response</param>
        private void ObtainShareResponse(Response response)
        {
            FacebookShareResponse shareResponse = response as FacebookShareResponse;
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
                        SocialInteractionType.Facebook.ToString()), string.Empty, false, exception);
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
                this.isPopUpClosed = true;
                this.loginControl.Close();
            }
            ApplicationData.IsPopUpOpen = false;
        }

        // TODO: Code Cleanup for logout.
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
            }
            ApplicationData.IsPopUpOpen = false;
        }

        /// <summary>
        ///  Log off from facebook. 
        /// </summary>
        /// <param name="sender"> Sender Object </param>
        /// <param name="e"> Navigation event argument</param>
        private void LogOffWebBrowserLoadCompleted(object sender, NavigationEventArgs e)
        {
            if (null != this.loginControl && this.isLogged)
            {
                try
                {
                    string javaScriptFunction = String.Format(CultureInfo.InvariantCulture, Constants.FacebookConstants.JavaScriptFunction,
                                    new object[] {  Utility.GetSocialData("FacebookLogoutDivId") });
                    JavaScriptInterOp.InjectJavaScriptFunction(this.loginControl.WebBrowser, javaScriptFunction);
                    this.loginControl.WebBrowser.InvokeScript("forceLogout");
                    this.isLogged = false;
                }
                catch (COMException exception)
                {
                    ApplicationData.ErrorLogger.Log(exception);
                }
            }
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.loginControl.Close();
            }));
        }

        /// <summary>
        /// Handler called after the user has logged out and closed the popup.
        /// </summary>
        /// <param name="sender">Popup</param>
        /// <param name="e">EventArgs</param>
        private void LogOffPopupClose(object sender, EventArgs e)
        {
            this.loginControl.Closed -= this.LogOffPopupClose;
            this.loginControl.WebBrowser.LoadCompleted -= LogOffWebBrowserLoadCompleted;

            if (false == this.isLogged)
            {
                this.accessToken = string.Empty;
                ApplicationData.FacebookAccount = null;
            }

            if (null != this.DialogClosed)
            {
                this.DialogClosed(null, null);
            }

            ApplicationData.IsPopUpOpen = false;
            if (null != this.LogOffCompleted)
            {
                this.LogOffCompleted.Invoke(this, null);
            }
        }

        /// <summary>
        /// This method logs the user out of the facebook API.
        /// </summary>
        public void LogOff()
        {
            try
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.loginControl = new SocialLogin();
                    this.loginControl.SocialLoginWindow.HeaderText = Resources.LogoutText;
                    this.loginControl.Owner = App.Current.MainWindow;
                    this.loginControl.Closed += LogOffPopupClose;
                    this.loginControl.WebBrowser.Navigate(new Uri(Constants.FacebookConstants.FacebookHomePageLink, UriKind.Absolute));
                    this.loginControl.WebBrowser.LoadCompleted += LogOffWebBrowserLoadCompleted;
                    this.loginControl.ShowInTaskbar = false;
                    this.loginControl.ShowDialog();

                }));
            }
            catch (SecurityException exception)
            {
                ApplicationData.ErrorLogger.Log(exception);
            }
        }
    }
}
