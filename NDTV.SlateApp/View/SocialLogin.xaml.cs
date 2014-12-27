using System.Windows;
using System.Windows.Controls;
using NDTV.Controller;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for SocialLogin.xaml
    /// </summary>
    public partial class SocialLogin : Window
    {        
        /// <summary>
        /// Gets or sets the web browser control.
        /// </summary>
        public WebBrowser WebBrowser
        {
            get
            {
                return this.webBrowserControl;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SocialLogin()
        {
            InitializeComponent();
            ApplicationData.IsPopUpOpen = true;
        }

        /// <summary>
        /// The close button click event.
        /// </summary>
        /// <param name="sender">Close button</param>
        /// <param name="e">Event arguments</param>
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
            ApplicationData.IsPopUpOpen = false;
        }
    }
}
