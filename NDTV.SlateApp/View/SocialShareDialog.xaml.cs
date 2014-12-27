using System.Windows;
using NDTV.Controller;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for SocialShareDialog.xaml
    /// </summary>
    public partial class SocialShareDialog : Window
    {
        public SocialShareDialog()
        {
            InitializeComponent();
            ApplicationData.IsPopUpOpen = true;
        }

        /// <summary>
        /// The close button clicked event handler.
        /// </summary>
        /// <param name="sender">Close button</param>
        /// <param name="e">Routed event arguments</param>
        private void CloseButtonClicked(object sender, RoutedEventArgs e)
        {            
            this.Close();
            ApplicationData.IsPopUpOpen = false;
        }
    }
}
