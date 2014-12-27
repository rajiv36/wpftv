using System;
using System.Windows;
using System.Windows.Media.Imaging;
using NDTV.Controller;
using NDTV.SlateApp.ViewModel;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for ErrorMessagePopup.xaml
    /// </summary>
    public partial class ErrorMessagePopup : Window
    {
        private Exception exception;
        private bool shutdownApplicationOnClose;
        private ErrorMessageViewModel errorMessageViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <param name="helpMessage">Help message</param>
        /// <param name="isAnonymousFeedbackRequired">Flag to denote whether anonymous feedback is required or not</param>     
        /// <param name="exception">Exception which has caused this error</param>
        /// <param name="shutdownApplicationOnClose">Flag to denote whether application is to be shutdown on close</param>
        public ErrorMessagePopup(string errorMessage, string helpMessage, bool isAnonymousFeedbackRequired, Exception exception, bool shutdownApplicationOnClose)
        {            
            InitializeComponent();
            errorMessageViewModel = new ErrorMessageViewModel(exception);
            errorTypeImage.Source = new BitmapImage(new Uri("../Resources/Images/Error.png", UriKind.Relative));
            ErrorText.Text = errorMessage;
            this.exception = exception;
            this.shutdownApplicationOnClose = shutdownApplicationOnClose;
            if (false == string.IsNullOrEmpty(helpMessage))
            {
                HelpText.Text = helpMessage;
            }            
            SendAnonymousFeedbackButton.Visibility = isAnonymousFeedbackRequired ? Visibility.Visible : Visibility.Collapsed;
            ApplicationData.IsErrorMessagePopupVisible = true;
            this.Activate();
            this.DataContext = errorMessageViewModel;
        }

        /// <summary>
        /// Event for ok button clicked
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event args</param>
        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            ApplicationData.IsErrorMessagePopupVisible = false;
            if (shutdownApplicationOnClose)
            {
                App.Current.Shutdown();
            }
            else
            {
                this.Close();
            }            
        }

        /// <summary>
        /// Event for send anonymous feedback button clicked
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event args</param>
        private void SendAnonymousFeedbackButtonClicked(object sender, RoutedEventArgs e)
        {
            //TODO : Call method to send anonymous feedback
            errorMessageViewModel.ReportErrorCommand.Execute(exception);
            while (true)
            {
                if (false == string.IsNullOrEmpty(errorMessageViewModel.ReportErrorStatus))
                {
                    break;
                }
            }
            SendAnonymousFeedbackButton.Visibility = Visibility.Collapsed;
        }
    }
}
