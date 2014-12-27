using System.Windows;
using NDTV.Controller;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for CommentsControl.xaml
    /// </summary>
    public partial class CommentsControl : Window
    {
        public CommentsControl()
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
