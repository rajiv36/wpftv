using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using NDTV.Entities;
using NDTV.SlateApp.ViewModel;
using NDTV.Controller;

namespace NDTV.SlateApp.View
{
    /// <summary>
    ///  Ad Banner user control 
    /// </summary>
    public partial class AdBannerControl : UserControl, IDisposable
    {
        private AdBannerViewModel adViewModel;
        private Boolean showAdvertisement;        

        #region CONSTRUCTOR

        /// <summary>
        /// Default contructor.
        /// </summary>
        public AdBannerControl()
        {
            
            InitializeComponent();
            
            /* 
             * Here Private variable is initialized and not property to prevent web browser control 
             * to be visible intentionally 
             */

            this.adViewModel = new AdBannerViewModel();
            showAdvertisement = true;

            /* Wire up online and offline event from application */
            App application = App.Current as App;
            if (null != application)
            {
                application.OnApplicationOffline += new EventHandler(OnConnectionLoss);
                application.OnApplicationOnline += new EventHandler(OnConnectionReceived);
            }
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Get or set the Current Page.
        /// </summary>
        public Pages CurrentPage
        {
            get
            {
                return adViewModel.CurrentPage;
            }
            set
            {
                this.adViewModel.CurrentPage = value;
                this.RefreshAdBanner();
            }
        }
        
        /// <summary>
        /// Determines visibility of ad banner. 
        /// </summary>
        public Boolean ShowAdvertisement
        {
            get 
            {
                return showAdvertisement;
            }
            set 
            {
                showAdvertisement = value;
                this.Visibility = (true == value ? Visibility.Visible: Visibility.Hidden );
            }
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// This function, updates AdBanner control with the height, width,
        /// and content correspoding to the current page. 
        /// </summary>
        public void RefreshAdBanner()
        {
            this.PreLoader.IsBusy = true;
            if (null != adViewModel)
            {
                /* Set visiblity, height and width */
                this.AdBannerContainer.Visibility = Visibility.Hidden;
                this.AdBannerContainer.Width = adViewModel.AdBanner.AdWidth;
                this.AdBannerContainer.Height = adViewModel.AdBanner.AdHeight;
                /*
                * If control is already loaded, it can start navigating 
                * Otherwise wait for control to be loaded and navigate in OnLoaded event.
                */
                if (true == this.ShowAdvertisement)
                {
                    if (this.AdBannerContainer.IsLoaded)
                    {
                        this.AdBannerContainer.NavigateToString(adViewModel.AdBanner.AdContent);
                    }
                }
            }
            else
            {
                this.PreLoader.IsBusy = false;
            }
        }

        /// <summary>
        /// Set the visiblity to visible once all the content is downloaded 
        /// into web browser control.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event argument object.</param>
        private void AdBannerContainerLoadCompleted(object sender, NavigationEventArgs e)
        {
            double x = ApplicationData.DpiX;

            if (true == this.ShowAdvertisement)
            {
                this.AdBannerContainer.Visibility = Visibility.Visible;
            }
            else
            {
                this.AdBannerContainer.Visibility = Visibility.Hidden;
            }
            this.PreLoader.IsBusy = false;
        }

        /// <summary>
        ///  Ad starts loading, once the webbrowser control is loaded.
        /// </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Routed event args. parameter </param>
        private void AdBannerContainerLoaded(object sender, RoutedEventArgs e)
        {
            if (null != adViewModel)
            {
                /* User Control gets visible automatically once the content is loaded */
                this.AdBannerContainer.NavigateToString(adViewModel.AdBanner.AdContent);
            }
        }

        /// <summary>
        /// Responds when application goes offline.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="arguments">Event args Argument </param>
        private void OnConnectionLoss(Object sender, EventArgs arguments)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                this.PreLoader.IsBusy = false;
            }));

        }

        /// <summary>
        /// Responds when application goes offline.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="arguments">Event args Argument </param>
        private void OnConnectionReceived(Object sender, EventArgs arguments)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                this.RefreshAdBanner();
            }));

        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Disposing user control resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        #endregion

        #region PROTECTED METHODS
        /// <summary>
        /// The bulk of the clean-up code is implemented in Dispose(bool)
        /// </summary>
        /// <param name="disposing"> To indicate if dispose required </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                /* free managed resources */
                if (null != this.adViewModel)
                {
                    this.adViewModel.Dispose();
                    this.adViewModel = null;
                }
            }
        }
        #endregion



    }
}
