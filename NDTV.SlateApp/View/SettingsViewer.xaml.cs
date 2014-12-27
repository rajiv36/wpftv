using System.Windows;
using System.Windows.Controls;
using NDTV.Controller;
using NDTV.SlateApp.ViewModel;
using SlateAppProperties = NDTV.SlateApp.Properties;
using NDTV.Entities;
using System;
using NDTV.Utilities;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for SettingsViewer.xaml
    /// </summary>
    public partial class SettingsViewer : SlateWindow
    {
        /// <summary>
        /// Login options View model for social interaction buttons.
        /// </summary>
        LoginOptionsViewModel loginOptionsViewModel = null;

        /// <summary>
        /// Settings Viewer View Model.
        /// </summary>
        SettingsViewerViewModel settingsViewer = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SettingsViewer()
        {
            InitializeComponent();
            settingsViewer = new SettingsViewerViewModel();
            this.DataContext = settingsViewer;

            if (false == string.IsNullOrWhiteSpace(ApplicationData.Settings.WeatherCity))
            {
                textBlockCityNameHolder.Text = ApplicationData.Settings.WeatherCity;
            }
            else
            {
                textBlockCityNameHolder.Text = SlateAppProperties.Resources.DefaultWeatherCity;
            }

            if (null != ApplicationData.Settings.UnsavedFeedback)
            {
                textBoxFeedbackContainer.Text = ApplicationData.Settings.UnsavedFeedback;
            }
            textBlockResult.Text = string.Empty;

            loginOptionsViewModel = new LoginOptionsViewModel();
            SocialMediaInteractionGrid.DataContext = loginOptionsViewModel;
            ShowLogoutButtons();
            ApplicationData.IsPopUpOpenValueChanged += IsPopUpOpenValueChanged;
            AboutAditi.Text = Constants.StaticText.AboutAditiText;
            AboutNdtv.Text = Constants.StaticText.AboutNdtvDefaultText;
            AboutApplication.Text = Constants.StaticText.AboutApplicationText;
        }

        /// <summary>
        /// Shows the logout buttons depending on their login logout status.
        /// </summary>
        private void ShowLogoutButtons()
        {
            if (null != ApplicationData.GoogleAccount && ApplicationData.GoogleAccount.IsLogged == true)
            {
                this.GoogleLogOffButton.Visibility = Visibility.Visible;
            }
            if (null != ApplicationData.YahooAccount && ApplicationData.YahooAccount.IsLogged == true)
            {
                this.YahooLogOffButton.Visibility = Visibility.Visible;
            }
            if (null != ApplicationData.TwitterAccount && ApplicationData.TwitterAccount.IsLogged == true)
            {
                this.TwitterLogOffButton.Visibility = Visibility.Visible;
            }
            if (null != ApplicationData.FacebookAccount && ApplicationData.FacebookAccount.IsLogged == true)
            {
                this.FacebookLogOffButton.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Event fired when login popup webbrowser for login social screen invoked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs</param>
        private void IsPopUpOpenValueChanged(object sender, EventArgs e)
        {
            if (false == ApplicationData.IsPopUpOpen)
            {
                ShowLogoutButtons();
            }

            if (ApplicationData.IsPopUpOpen)
            {
                ModalPopup.Visibility = Visibility.Visible;
            }
            else
            {
                ModalPopup.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// close feedback form.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void CloseGalleryImage(object sender, RoutedEventArgs e)
        {
            ApplicationData.IsPopUpOpenValueChanged -= IsPopUpOpenValueChanged;
            this.Close();
        }

        /// <summary>
        /// On selection change of Foreign city list.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        private void SelectedForeignCityChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationData.Settings.WeatherCity = ForeignCitiesListBox.SelectedItem as string;
            ApplicationData.Settings.WeatherCountry = SlateAppProperties.Resources.World;
            textBlockCityNameHolder.Text = ForeignCitiesListBox.SelectedItem as string;
        }

        /// <summary>
        /// On selection change of Indian city list.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        private void SelectedIndianCityChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationData.Settings.WeatherCity = IndianCitiesListBox.SelectedItem as string;
            ApplicationData.Settings.WeatherCountry = SlateAppProperties.Resources.Indian; ;
            textBlockCityNameHolder.Text = IndianCitiesListBox.SelectedItem as string;
        }

        /// <summary>
        /// Submit feedback click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Routed Event Args</param>
        private void SubmitFeedBackClick(object sender, RoutedEventArgs e)
        {
            if (false == string.IsNullOrWhiteSpace(textBoxFeedbackContainer.Text))
            {
                settingsViewer.SendFeedBack(textBoxEmailContainer.Text, Helper.RemoveHtmlTags(textBoxFeedbackContainer.Text));
                ApplicationData.Settings.UnsavedFeedback = string.Empty;
            }
        }

        ///// <summary>
        ///// Clear feedback text box click.
        ///// </summary>
        ///// <param name="sender">The sender.</param>
        ///// <param name="e">Routed Event Args</param>
        //private void ClearFeedbackClick(object sender, RoutedEventArgs e)
        //{
        //    textBoxFeedbackContainer.Text = string.Empty;
        //    textBlockResult.Text = string.Empty;
        //}

        ///// <summary>
        ///// OnSaveButtonClick.
        ///// </summary>
        ///// <param name="sender">The sender.</param>
        ///// <param name="e">Routed Event Args</param>
        //private void SaveButtonClick(object sender, RoutedEventArgs e)
        //{
        //    ApplicationData.Settings.UnsavedFeedback = textBoxFeedbackContainer.Text;
        //    textBlockResult.Text = SlateAppProperties.Resources.MessageSaved;
        //}

        /// <summary>
        /// Google Account log off.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Routed Event Args</param>
        private void GoogleLogoff(object sender, RoutedEventArgs e)
        {
            GoogleAccount googleAccount = ApplicationData.GoogleAccount as GoogleAccount;

            if (null != googleAccount)
            {
                googleAccount.LogOff();
                googleAccount.LogOffCompleted += GoogleAccountLogOffCompleted;
            }          
        }

        /// <summary>
        /// Facebook LogOff click.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void FacebookLogoff(object sender, RoutedEventArgs e)
        {
            FacebookAccount facebookAccount = ApplicationData.FacebookAccount as FacebookAccount;

            if (null != facebookAccount)
            {
                facebookAccount.LogOff();
                facebookAccount.LogOffCompleted += FacebookAccountLogOffCompleted;
            }
        }

        /// <summary>
        /// Twitter logoff click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Routed Event Args</param>
        private void TwitterLogOff(object sender, RoutedEventArgs e)
        {

            TwitterAccount twitterAccount = ApplicationData.TwitterAccount as TwitterAccount;

            if (null != twitterAccount)
            {
                twitterAccount.LogOff();
                twitterAccount.LogOffCompleted += TwitterAccountLogOffCompleted;
            }
        }

        /// <summary>
        /// Yahoo logoff click.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Routed Event Args</param>
        private void YahooLogOff(object sender, RoutedEventArgs e)
        {
            YahooAccount yahooAccount = ApplicationData.YahooAccount as YahooAccount;

            if (null != yahooAccount)
            {
                yahooAccount.LogOff();
                yahooAccount.LogOffCompleted += YahooAccountLogOffCompleted;
            }
        }

        /// <summary>
        /// Google Account LogOff completed.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void GoogleAccountLogOffCompleted(object sender, EventArgs e)
        {
            this.GoogleLogOffButton.Visibility = Visibility.Hidden;
            if (ApplicationData.FirstLoggedOn == LoginMode.Google)
            {
                SetFirstLoggedOn(LoginMode.Google);
            }
        }

        /// <summary>
        /// Facebook Account LogOff completed.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void FacebookAccountLogOffCompleted(object sender, EventArgs e)
        {
            this.FacebookLogOffButton.Visibility = Visibility.Hidden;
            if (ApplicationData.FirstLoggedOn == LoginMode.Facebook)
            {
                SetFirstLoggedOn(LoginMode.Facebook);
            }
        }

        /// <summary>
        /// Twitter Account LogOff completed.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void TwitterAccountLogOffCompleted(object sender, EventArgs e)
        {
            this.TwitterLogOffButton.Visibility = Visibility.Hidden;
            if (ApplicationData.FirstLoggedOn == LoginMode.Twitter)
            {
                SetFirstLoggedOn(LoginMode.Twitter);
            }
        }

        /// <summary>
        /// Yahoo Account LogOff completed.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void YahooAccountLogOffCompleted(object sender, EventArgs e)
        {
            this.YahooLogOffButton.Visibility = Visibility.Hidden;
            if (ApplicationData.FirstLoggedOn == LoginMode.Yahoo)
            {
                SetFirstLoggedOn(LoginMode.Yahoo);
            }
        }

        /// <summary>
        /// Sets first logged on mode
        /// </summary>
        /// <param name="currentLogin">Current login</param>
        private void SetFirstLoggedOn(LoginMode currentLogin)
        {
            if (null != ApplicationData.FacebookAccount 
                && ApplicationData.FacebookAccount.IsLogged 
                && currentLogin != LoginMode.Facebook)
            {
                ApplicationData.FirstLoggedOn = LoginMode.Facebook;
            }
            else if (null != ApplicationData.TwitterAccount
                && ApplicationData.TwitterAccount.IsLogged
                && currentLogin != LoginMode.Twitter)
            {
                ApplicationData.FirstLoggedOn = LoginMode.Twitter;
            }
            else if (null != ApplicationData.GoogleAccount
                && ApplicationData.GoogleAccount.IsLogged
                && currentLogin != LoginMode.Google)
            {
                ApplicationData.FirstLoggedOn = LoginMode.Google;
            }
            else if (null != ApplicationData.YahooAccount
                && ApplicationData.YahooAccount.IsLogged
                && currentLogin != LoginMode.Yahoo)
            {
                ApplicationData.FirstLoggedOn = LoginMode.Yahoo;
            }
            else
            {
                ApplicationData.FirstLoggedOn = LoginMode.None;
            }
        }

        /// <summary>
        /// Text box feedback got focus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeedBackGotFocus(object sender, RoutedEventArgs e)
        {
            textBlockResult.Text = string.Empty;
        }

        /// <summary>
        /// Helper function for SwitchToLandscapeView.
        /// </summary>
        protected override void SwitchToLandscapeView()
        {
            this.Width = 950;
            this.Height = 550;
            SetWindowPosition();
        }

        /// <summary>
        /// Helper function for SwitchToPortraitView.
        /// </summary>
        protected override void SwitchToPortraitView()
        {
            this.Width = 550;
            this.Height = 950;
            SetWindowPosition();
        }

        /// <summary>
        /// The overriden method which sets the size of the window based on whether it is in landscape or potriat version
        /// </summary>
        protected override void SetSize()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                this.SetWindowPosition();
                SwitchToLandscapeView();
            }
            else
            {
                this.SetWindowPosition();
                SwitchToPortraitView();
            }
        }

        /// <summary>
        /// Sets the window position calculating the resolution of the window.
        /// </summary>
        private void SetWindowPosition()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            Left = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
            Top = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;
        }

    }
}
