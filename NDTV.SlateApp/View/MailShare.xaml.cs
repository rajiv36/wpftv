using System;
using System.Windows;
using NDTV.Controller;
using NDTV.SlateApp.ViewModel;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for MailShare.xaml
    /// </summary>
    public partial class MailShare : Window
    {
        public MailShare()
        {
            InitializeComponent();
            
            this.SenderName.MaxLength = Convert.ToInt32( Properties.Resources.MailSenderNameLimit);
            this.ReceiversName.MaxLength = Convert.ToInt32(Properties.Resources.MailReceiverNameLimit);
            this.MailBodyText.MaxLength = Convert.ToInt32(Properties.Resources.MailBodyLimit);

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

        /// <summary>
        /// The send button handler.
        /// </summary>
        /// <param name="sender">Send button</param>
        /// <param name="e">Routed event arguments</param>
        private void SendButtonClick(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MailShareViewModel).SenderEmail = this.senderMail.Text;
            (this.DataContext as MailShareViewModel).ReceiverEmail = this.receiverMail.Text;
        }
    }
}
