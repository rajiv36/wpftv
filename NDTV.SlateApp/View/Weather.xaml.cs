using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using NDTV.Controller;
using NDTV.SlateApp.Framework.Utilities;
using NDTV.SlateApp.ViewModel;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for Weather.xaml
    /// </summary>
    public partial class Weather : SlateWindow
    {
        /// <summary>
        /// Weather view model.
        /// </summary>
        private WeatherViewModel weather = null;
        /// <summary>
        /// Sets the starting position of the Landscape and portrait view.
        /// </summary>
        private bool isSetSizeCalledFromPortraitView;

        /// <summary>
        /// true wen web browser is loaded fully.
        /// </summary>
        private bool isMaploaded;

        /// <summary>
        /// used to avoid the is online function network delay.
        /// </summary>
        private bool isApplicationOnlineOnce;

        public Weather()
        {
            InitializeComponent();
            JavaScriptInterOp.HideScriptErrors(this.WeatherBrowserControl, true);
            (App.Current as App).OnApplicationOnline += OnApplicationOnline;
            (App.Current as App).OnApplicationOffline += OnApplicationOffline;
            weather = new WeatherViewModel();
            this.WeatherBrowserControl.Navigate(weather.WeatherScreenLink);
            isSetSizeCalledFromPortraitView = false;
            isMaploaded = false;
            ApplicationData.IsPopUpOpenValueChanged += IsPopUpOpenValueChanged;
        }

        /// <summary>
        /// Event handler indicating changed in pop up visibility.
        /// </summary>
        /// <param name="sender">Application Data</param>
        /// <param name="e">Event arguments</param>
        private void IsPopUpOpenValueChanged(object sender, EventArgs e)
        {
            if (ApplicationData.IsPopUpOpen)
            {
                ModalPopup.Visibility = Visibility.Visible;
                WeatherBrowserControl.Visibility = Visibility.Collapsed;                
            }
            else
            {
                ModalPopup.Visibility = Visibility.Collapsed;
                WeatherBrowserControl.Visibility = Visibility.Visible;                
            }
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
                isSetSizeCalledFromPortraitView = true;
                SwitchToPortraitView();
                this.SetWindowPosition();
            }
            
        }

         ///<summary>
         ///Event fires when application comes online.
         ///</summary>
         ///<param name="sender">The sender object.</param>
         ///<param name="e">The event args.</param>
        private void OnApplicationOffline(object sender, EventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                if (preLoader.IsBusy.HasValue && preLoader.IsBusy.Value)
                {
                    preLoader.IsBusy = false;
                }
                this.WeatherBrowserControl.IsEnabled = false;
                isApplicationOnlineOnce = false;
            }));
        }

        /// <summary>
        /// Event fires when application comes online.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        private void OnApplicationOnline(object sender, EventArgs e)
        {
            
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                if (isApplicationOnlineOnce)
                {
                    isMaploaded = false;
                    if (ApplicationData.IsLandscapeOrientation)
                    {
                        SwitchToLandscapeView();
                    }
                    else
                    {
                        SwitchToPortraitView();
                    }
                    JavaScriptInterOp.HideScriptErrors(this.WeatherBrowserControl, true);
                    preLoader.IsBusy = true;
                    this.WeatherBrowserControl.IsEnabled = true;
                    this.WeatherContainer.Visibility = Visibility.Hidden;
                    this.WeatherBrowserControl.Navigate(weather.WeatherScreenLink);
                    this.WeatherBrowserControl.Visibility = Visibility.Hidden;
                    
                }
                isApplicationOnlineOnce = true;
            }));
        }

        /// <summary>
        /// Browser on load call event.
        /// </summary>
        /// <param name="sender">The sender Object</param>
        /// <param name="e">The NavigationEventArgs.</param>
        private void WeatherBrowserControlLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            string style = "body { scrollbar-base-color:black; }";

            string removeTools = @"var div = document.getElementById('socialTools'); " +
                                 @"div.parentNode.removeChild(div); " +
                                 @"var div1 = document.getElementById('main_body').style.visibility = 'hidden'; " +
                                 @"var div2 = document.getElementById('logo').style.visibility = 'hidden';" +
                                 @"var divs = document.getElementsByTagName('div'); var div; " +
                                 @"var i = divs.length; while (i--) " +
                                 @"{ div = divs[i]; if (div.getAttribute('class') == 'infoHolder') " +
                                 @"{div.parentNode.removeChild(div);}}";

            string findDivByClass = @"function findDivByClass (classname) " +
                                    @"{ " +
                                    @"var divs = document.getElementsByTagName('div'); " +
                                    @"var div; " +
                                    @"var i = divs.length;" +
                                    @"while (i--) " +
                                    @"{ " +
                                    @"   div = divs[i]; " +
                                    @"   if (div.getAttribute('class') == classname) " +
                                    @"   {" +
                                    @"     break; " +
                                    @"   }" +
                                    @"}" +
                                    @" return div;}";
            JavaScriptInterOp.DisableJavaScriptError(WeatherBrowserControl);
            JavaScriptInterOp.InjectJavaScriptFunction(WeatherBrowserControl, findDivByClass);
            JavaScriptInterOp.InjectJavaScriptFunction(WeatherBrowserControl, removeTools, style);
            InjectHeaderTool();

            if (ApplicationData.IsLandscapeOrientation)
            {
                try
                {
                    WeatherBrowserControl.InvokeScript("arrangeHeaderToolLandScape");
                }
                catch (COMException comException)
                {
                    ApplicationData.ErrorLogger.Log(comException);
                }
            }

            else
            {
                try
                {
                    WeatherBrowserControl.InvokeScript("arrangeHeaderToolPortrait");
                }
                catch (COMException comException)
                {
                    ApplicationData.ErrorLogger.Log(comException);
                }

            }
            if (this.WeatherBrowserControl.Visibility == Visibility.Hidden)
            {
                this.WeatherBrowserControl.Visibility = Visibility.Visible;
            }

            WeatherContainer.Visibility = Visibility.Visible;
            preLoader.IsBusy = false;
            isMaploaded = true;
            this.PopupBaseControl.FooterEnabled = true;
        }
      
        /// <summary>
        /// Method adjusts the alignment of "Pin your city" search box.
        /// </summary>
        private void InjectHeaderTool()
        {
            string arrangeHeaderToolLandScape = @"function arrangeHeaderToolLandScape () { " +
                                    @"var ret_div = findDivByClass('comment_cont'); " +
                                    @"ret_div.style.position='absolute';" +
                                    @"ret_div.style.bottom='20px';" +
                                    @"ret_div.style.left='0px' ; } ";

            string arrangeHeaderToolPortrait = @"function arrangeHeaderToolPortrait () { " +
                                    @"var ret_div = findDivByClass('comment_cont'); " +
                                    @"ret_div.style.position='absolute';" +
                                    @"ret_div.style.bottom='20px';" +
                                    @"ret_div.style.left='20px' ; } ";

            JavaScriptInterOp.InjectJavaScriptFunction(WeatherBrowserControl, arrangeHeaderToolLandScape);
            JavaScriptInterOp.InjectJavaScriptFunction(WeatherBrowserControl, arrangeHeaderToolPortrait);
        }

        /// <summary>
        /// On click of close button.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">Routed Event Args</param>
        private void CloseWeatherPage(object sender, RoutedEventArgs e)
        {
            ApplicationData.IsPopUpOpenValueChanged -= IsPopUpOpenValueChanged;
            this.Dispose();
            this.Close();
        }

        /// <summary>
        /// Helper function for SwitchToLandscapeView.
        /// </summary>
        protected override void SwitchToLandscapeView()
        {
            this.Width = 950;
            this.Height = 550;
            browserColumn.Width = new GridLength(1, GridUnitType.Star);
            browserRow.Height = new GridLength(1, GridUnitType.Star);
            if (isMaploaded)
            {
                try
                {
                    WeatherBrowserControl.InvokeScript("arrangeHeaderToolLandScape");
                }
                catch (COMException comException)
                {
                    ApplicationData.ErrorLogger.Log(comException);
                }
            }

            SetWindowPosition();
            isSetSizeCalledFromPortraitView = false;
        }

        /// <summary>
        /// Helper function for SwitchToPortraitView.
        /// </summary>
        protected override void SwitchToPortraitView()
        {
            this.Width = 550;
            this.Height = 950;
            if (isSetSizeCalledFromPortraitView)
            {
                browserColumn.Width = new GridLength(565, GridUnitType.Pixel);
            }
            else
            {
                browserColumn.Width = new GridLength(1, GridUnitType.Star);
                browserRow.Height = new GridLength(1, GridUnitType.Star);
            }
            if (isMaploaded)
            {
                try
                {
                    WeatherBrowserControl.InvokeScript("arrangeHeaderToolPortrait");
                }
                catch (COMException comException)
                {
                    ApplicationData.ErrorLogger.Log(comException);
                }
            }

            SetWindowPosition();
            isSetSizeCalledFromPortraitView = false;
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

        /// <summary>
        /// Dispose view.
        /// </summary>
        protected override void DisposeResources()
        {
            WeatherBrowserControl.Dispose();
            (App.Current as App).OnApplicationOnline -= OnApplicationOnline;
            (App.Current as App).OnApplicationOffline -= OnApplicationOffline;
        }
    }
}
