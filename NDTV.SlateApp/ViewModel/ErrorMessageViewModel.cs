using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using NDTV.Entities;
using NDTV.Utilities;
using SlateProperties = NDTV.SlateApp.Properties;
using System.Globalization;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// Error message view model class
    /// </summary>
    public class ErrorMessageViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets the current exception
        /// </summary>
        public Exception CurrentException
        {
            get;
            private set;
        }

        private string reportErrorStatus;        

        /// <summary>
        /// Gets and sets report error status
        ///// </summary>
        public string ReportErrorStatus
        {
            get
            {
                return reportErrorStatus;
            }
            set
            {
                reportErrorStatus = value;
                OnPropertyChanged("ReportErrorStatus");
            }
        }

        /// <summary>
        /// Gets the report error command
        /// </summary>
        public ICommand ReportErrorCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exception">Exception which is to be sent anonymously</param>
        public ErrorMessageViewModel(Exception exception)
        {
            CurrentException = exception;
            ReportErrorCommand = new RelayCommand(ExecuteReportErrorCommand);
            ReportErrorStatus = string.Empty;
        }

        /// <summary>
        /// This method executes the report error command
        /// </summary>
        /// <param name="commandObject">Command object</param>
        private void ExecuteReportErrorCommand(object commandObject)
        {
            ReportErrorStatus = string.Empty;
            if (commandObject.GetType() == typeof(Exception))
            {
                CurrentException = commandObject as Exception;
            }
            string exceptionString = CurrentException.Message.ToString(CultureInfo.InvariantCulture);
            if(null != CurrentException.StackTrace)
            {
                exceptionString += CurrentException.StackTrace;
            }
            //if(null != CurrentException.InnerException)
            //{
            //    exceptionString += Constants.Constant.PipeSeparator + CurrentException.InnerException.Message.ToString(CultureInfo.InvariantCulture);
            //}
            if (exceptionString.Length > 700)
            {
                exceptionString = exceptionString.Substring(0, 700);
                exceptionString = exceptionString.Substring(0, exceptionString.LastIndexOf(".")+1);
            }
            MailShareRequest request = new MailShareRequest(Utility.GetReportErrorEntries(Constants.ReportErrorKeys.FromEmailAddress), Utility.GetReportErrorEntries(Constants.ReportErrorKeys.ToEmailAddress), SlateProperties.Resources.ReportErrorSubject, exceptionString);
            this.showOfflineErrorMessage = false;
            ProcessRequest(request, ProcessReportErrorResponse,null,false);
        }

        /// <summary>
        /// This method processes the report error response
        /// </summary>
        /// <param name="response">Response object</param>
        private void ProcessReportErrorResponse(Response response)
        {
            if(response.GetType() == typeof(MailShareResponse))
            {
                MailShareResponse mailShareResponse = response as MailShareResponse;
                ReportErrorStatus = mailShareResponse.Success ? SlateProperties.Resources.ReportErrorSuccess : SlateProperties.Resources.ReportErrorFailure;
            }            
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        protected override void DisposeResources()
        {
            CurrentException = null;
        }
    }
}
