using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.View;
using NDTV.SlateApp.ViewModel;
using NDTV.Utilities;
using SlateAppProperties = NDTV.SlateApp.Properties;

namespace NDTV.SlateApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string errorText;
        private string helpText;
        private bool isAnonymousFeedbackRequired;
        private Exception exception;
        private SplashScreenWindow splashScreen;

        private const string splashScreenImageName = "SplashScreen.png";

        /// <summary>
        /// Event raised when application is online
        /// </summary>
        public EventHandler OnApplicationOnline
        {
            get;
            set;
        }

        /// <summary>
        /// Event raised when application is offline
        /// </summary>
        public EventHandler OnApplicationOffline
        {
            get;
            set;
        }       

        /// <summary>
        /// Application constructor
        /// </summary>
        public App()
        {
            splashScreen = new SplashScreenWindow();            
            Dispatcher.CurrentDispatcher.UnhandledException += HandleException;
            NetworkChange.NetworkAddressChanged += SetNetworkChanges;
            ApplicationData.IsApplicationOnline = NetworkInterface.GetIsNetworkAvailable();
            if (ApplicationData.IsApplicationOnline)
            {              
                AppViewModel applicationViewModel = new AppViewModel();
                applicationViewModel.LoadData(ApplicationDataLoadComplete,ApplicationDataLoadError);
                splashScreen.Show();
            }
            else
            {
                this.MainWindow = new ErrorMessagePopup(SlateAppProperties.Resources.ApplicationLoadFailureMessage, string.Empty, false, null, true);
                this.MainWindow.ShowActivated = true;                
                this.MainWindow.Show();
            }
        }

        /// <summary>
        /// This method is executed once the application data load is complete
        /// </summary>
        private void ApplicationDataLoadComplete()
        {
            Dispatcher.BeginInvoke (DispatcherPriority.Send, new  Action(() => 
                {                    
            this.MainWindow = new MainWindow();
            this.MainWindow.ShowActivated = true;            
            this.MainWindow.Show();
            splashScreen.Close();
                }));
        }

        /// <summary>
        /// This method is executed in case there are issues loading application data
        /// </summary>
        private void ApplicationDataLoadError()
        {
            DisplayErrorMessage(SlateAppProperties.Resources.ApplicationLoadFailureMessage,string.Empty,true, new Exception("Unable to load application data"));             
        }

        /// <summary>
        /// This event is fired when an unhandled exception occurs
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void HandleException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ApplicationData.ErrorLogger.Log(e.Exception);
            this.DisplayErrorMessage(SlateAppProperties.Resources.GeneralFailureMessage, string.Empty, true,e.Exception);
        }

        /// <summary>
        /// Sets a value in application data if the user is online or offline.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void SetNetworkChanges(object sender, EventArgs e)
        {
            ApplicationData.IsApplicationOnline = NetworkInterface.GetIsNetworkAvailable();
            if (false == ApplicationData.IsApplicationOnline)
            {
                this.DisplayErrorMessage(SlateAppProperties.Resources.GeneralFailureMessage, string.Empty, false, null);
                if (null != OnApplicationOffline)
                {
                    OnApplicationOffline(sender, e);
                }
            }
            else
            {
                if (null != OnApplicationOnline)
                {
                    OnApplicationOnline(sender, e);
                }
            }
        }

        /// <summary>
        /// This method displays the error message
        /// </summary>
        ///<param name="errorText">Error text</param>
        /// <param name="helpText">Help text</param>
        /// <param name="isAnonymousFeedbackRequired">Flag denoting whether this exception needs to be given as an anonymous feedback</param>   
        /// <param name="exception">Exception</param>        
        public void DisplayErrorMessage(string errorText, string helpText, bool isAnonymousFeedbackRequired, Exception exception)
        {
            if (false == ApplicationData.IsErrorMessagePopupVisible)
            {
                this.errorText = errorText;
                this.helpText = helpText;
                this.isAnonymousFeedbackRequired = isAnonymousFeedbackRequired;
                this.exception = exception;                
                //ShowErrorMessagePopup();                
                var thread = new Thread(new ThreadStart(ShowErrorMessagePopup));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();                        
            }
        }

        /// <summary>
        /// This method shows the error message popup
        /// </summary>        
        private void ShowErrorMessagePopup()
        {
            if (false == ApplicationData.IsErrorMessagePopupVisible)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action( () =>
                {
                    ErrorMessagePopup errorMessage = new ErrorMessagePopup(errorText, helpText, isAnonymousFeedbackRequired, exception,false);
                    errorMessage.Owner = this.MainWindow;
                    bool? dialogResult = errorMessage.ShowDialog();
                    if (dialogResult.HasValue && dialogResult.Value)
                    {
                        errorMessage = null;
                    } 
                }
                ));
            }
        }

        /// <summary>
        /// Event on exit of application
        /// </summary>
        /// <param name="e">Exit event argument</param>
        protected override void OnExit(ExitEventArgs e)
        {            
            Utility.SaveSettings(ApplicationData.Settings);
            NetworkChange.NetworkAddressChanged -= SetNetworkChanges;
            base.OnExit(e);
        }                   
    }
}
