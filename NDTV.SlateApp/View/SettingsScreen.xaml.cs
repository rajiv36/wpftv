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
	/// Interaction logic for SettingsScreen.xaml
	/// </summary>
	public partial class SettingsScreen : SlateWindow
	{
        /// <summary>
        /// Login options View model for social interaction buttons.
        /// </summary>
        LoginOptionsViewModel loginOptionsViewModel = null;

        /// <summary>
        /// Settings Viewer View Model.
        /// </summary>
        SettingsViewerViewModel settingsViewer = null;

		public SettingsScreen()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
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
            AboutAditi.Text = Constants.StaticText.AboutAditiText;
            AboutNdtv.Text = Constants.StaticText.AboutNdtvDefaultText;
            AboutApplication.Text = Constants.StaticText.AboutApplicationText;

            loginOptionsViewModel = new LoginOptionsViewModel();
            SocialMediaInteractionGrid.DataContext = loginOptionsViewModel;
            ShowLogoutButtons();
            ApplicationData.IsPopUpOpenValueChanged += IsPopUpOpenValueChanged;
		}

        /// <summary>
        /// Shows the logout buttons depending on their login logout status.
        /// </summary>
        private void ShowLogoutButtons()
        {
            if (null != ApplicationData.GoogleAccount && ApplicationData.GoogleAccount.IsLogged == true)
            {
                this.GoogleLoginButton.Visibility = Visibility.Hidden;
                this.GoogleLogOffButton.Visibility = Visibility.Visible;
            }
            if (null != ApplicationData.YahooAccount && ApplicationData.YahooAccount.IsLogged == true)
            {
                this.YahooLoginButton.Visibility = Visibility.Hidden;
                this.YahooLogOffButton.Visibility = Visibility.Visible;
            }
            if (null != ApplicationData.TwitterAccount && ApplicationData.TwitterAccount.IsLogged == true)
            {
                this.TwitterLoginButton.Visibility = Visibility.Hidden;
                this.TwitterLogOffButton.Visibility = Visibility.Visible;
            }
            if (null != ApplicationData.FacebookAccount && ApplicationData.FacebookAccount.IsLogged == true)
            {
                this.FacebookLoginButton.Visibility = Visibility.Hidden;
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

            //if (ApplicationData.IsPopUpOpen)
            //{
            //    ModalPopup.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    ModalPopup.Visibility = Visibility.Collapsed;
            //}
        }

        private void IndianCitiesChecked(object sender, RoutedEventArgs e)
        {
            if ((null != IndianCitiesListBox) && (null != IndianCitiesListBox))
            {
                ForeignCitiesListBox.Visibility = Visibility.Collapsed;
                IndianCitiesListBox.Visibility = Visibility.Visible;
            }
        }

        private void WorldCitiesChecked(object sender, RoutedEventArgs e)
        {
            IndianCitiesListBox.Visibility = Visibility.Collapsed;
            ForeignCitiesListBox.Visibility = Visibility.Visible;
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
                if (false == string.IsNullOrWhiteSpace(TextBoxUserName.Text))
                {
                    string userFeedBackWithUserName = "FeedBack From UserName: " + TextBoxUserName.Text + "-->  " + textBoxFeedbackContainer.Text;
                    
                    settingsViewer.SendFeedBack(textBoxEmailContainer.Text, Helper.RemoveHtmlTags(userFeedBackWithUserName));
                }
                else
                {
                    settingsViewer.SendFeedBack(textBoxEmailContainer.Text, Helper.RemoveHtmlTags(textBoxFeedbackContainer.Text));
                }
            }

            textBoxFeedbackContainer.Text = string.Empty;
            textBoxEmailContainer.Text = string.Empty;
            TextBoxUserName.Text = string.Empty;
        }

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
            this.GoogleLoginButton.Visibility = Visibility.Visible;
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
            this.FacebookLoginButton.Visibility = Visibility.Visible;
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
            this.TwitterLoginButton.Visibility = Visibility.Visible;
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
            this.YahooLoginButton.Visibility = Visibility.Visible;
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


	}
}