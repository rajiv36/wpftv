using System.Windows;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for LoginOptions.xaml
    /// </summary>
    public partial class LoginOptions : Window
    {
        public LoginOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The close button clicked event handler.
        /// </summary>
        /// <param name="sender">Close button</param>
        /// <param name="e">Routed event arguments</param>
        private void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
