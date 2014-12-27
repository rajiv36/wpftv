using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;
using NDTV.Controller;

namespace NDTV.SlateApp
{    
    /// <summary>
    /// Base class for the application windows. This handles the orientation change events
    /// </summary>
    public class SlateWindow : Window, IDisposable
    {
        /// <summary>
        /// Constants Potriat and Landscape which are used in TAGS of the framework element in most of the cases.
        /// </summary>
        protected const string Portrait = "Potriat";
        protected const string Landscape = "Landscape";

        /// <summary>
        /// Switches page to landscape view
        /// </summary>
        protected virtual void SwitchToLandscapeView()
        {
        }

        /// <summary>
        /// Switches page to portrait view
        /// </summary>
        protected virtual void SwitchToPortraitView()
        {

        }

        /// <summary>
        /// Sets the size of the window before setting the window location of it.
        /// </summary>
        protected virtual void SetSize()
        {

        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public SlateWindow()
        {            
            SystemEvents.DisplaySettingsChanged += DisplaySettingsChanged;
            this.ShowInTaskbar = false;
            InitializeCurrentOrientation();
        }


        /// <summary>
        /// This method sets the current orientation while opening the main window
        /// </summary>
        private void InitializeCurrentOrientation()
        {
            ApplicationData.IsLandscapeOrientation = (Screen.PrimaryScreen.Bounds.Height < Screen.PrimaryScreen.Bounds.Width);
        }

        /// <summary>
        /// Display setting changed event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void DisplaySettingsChanged(object sender, EventArgs e)
        {                       
            if (Screen.PrimaryScreen.Bounds.Height > Screen.PrimaryScreen.Bounds.Width)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    //Run the application in portrait
                    ApplicationData.IsLandscapeOrientation = false; 
                    SwitchToPortraitView();                    
                }));
            }
            else
            {               
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => 
                {
                    //Run the application in landscape
                    ApplicationData.IsLandscapeOrientation = true;
                    SwitchToLandscapeView();               
                }));
            }
        }

        /// <summary>
        /// The Method which opens up a new PopUp 
        /// </summary>
        /// <param name="popUpChildWindow">The View which needs to be opened</param>
        /// <param name="window">The Parent View which runa in the background</param>
        /// <param name="modalBGParent">The Main Window Rectangle UIElement to which a new style has to be applied</param>
        protected void OpenPopUpWindow(SlateWindow popUpChildWindow,SlateWindow window,Rectangle modalBGParent)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                modalBGParent.Visibility = Visibility.Visible;
                popUpChildWindow.SetSize();
                popUpChildWindow.Owner = window;
                popUpChildWindow.ShowDialog();
                modalBGParent.Visibility = Visibility.Collapsed;
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// The Method which opens up a new PopUp 
        /// </summary>
        /// <param name="popupChildWindow">The View which needs to be opened</param>
        /// <param name="window">The Parent View which runa in the background</param>
        /// <param name="modalBGParent">The Main Window Rectangle UIElement to which a new style has to be applied</param>
        /// <param name="handleOtherControlVisibility">Method to handle other control visibility</param>
        protected void OpenPopupWindow(SlateWindow popupChildWindow, SlateWindow window, Rectangle modalBGParent, Action<bool> handleOtherControlVisibility)
        {
            try
            {
                if (ApplicationData.IsApplicationOnline)
                {
                    if (null != handleOtherControlVisibility)
                    {
                        handleOtherControlVisibility(false);
                    }
                    modalBGParent.Visibility = Visibility.Visible;
                    popupChildWindow.SetSize();
                    popupChildWindow.Owner = window;
                    popupChildWindow.ShowDialog();
                    modalBGParent.Visibility = Visibility.Collapsed;
                    if (null != handleOtherControlVisibility)
                    {
                        handleOtherControlVisibility(true);
                    }
                }
                else
                {
                    (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
                }
            }
            catch (Exception)
            {
                //Close the popup window and re-launch the owner
                popupChildWindow.Close();
                modalBGParent.Visibility = Visibility.Collapsed;
                if (null != handleOtherControlVisibility)
                {
                    handleOtherControlVisibility(true);
                }                
            }
        }

        /// <summary>
        /// Dispose method to dispose all the properties used by the view. This is to be implemented in all view models
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Individual dispose method for each of the view 
        /// </summary>
        /// <param name="disposing">flag denoting whether the disposing of the managed resources is to be done or not</param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeResources();
                SystemEvents.DisplaySettingsChanged -= DisplaySettingsChanged;    
            }
        }

        /// <summary>
        /// Virtual method which is to be implemented in every view. 
        /// This is virtual as it might not be required in all the views. 
        /// </summary>
        protected virtual void DisposeResources()
        {          
        }
    }
}
