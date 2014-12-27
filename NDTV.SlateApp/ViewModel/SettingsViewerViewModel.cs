using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDTV.Controller;
using NDTV.Entities;
using SlateProperties = NDTV.SlateApp.Properties;
using NDTV.Utilities;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// ViewModel for SettingsViewer.
    /// </summary>
    public class SettingsViewerViewModel:ViewModelBase
    {
        /// <summary>
        /// List of Indian cities.
        /// </summary>
        public List<string> IndianCities
        {
            get;
            set;
        }

        /// <summary>
        /// List of foreign cities.
        /// </summary>
        public List<string> ForeignCities
        {
            get;
            set;
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
        /// constructor.
        /// </summary>
        public SettingsViewerViewModel()
        {
            this.IndianCities = new List<string>();
            this.ForeignCities = new List<string>();
            this.IndianCities = ApplicationData.IndianCities;
            this.ForeignCities = ApplicationData.ForeignCities;
        }

        MailShareRequest request = null;
        public void SendFeedBack(string sendersEmail, string FeedbackMessage)
        {
            if (string.IsNullOrWhiteSpace(sendersEmail))
            {
                request = new MailShareRequest(Utility.GetFeedback(Constants.UserFeedBack.DefaultSendersEmail), Utility.GetFeedback(Constants.UserFeedBack.ReceiversEmail), Constants.UserFeedBack.Subject, FeedbackMessage);
            }
            else
            {
                request = new MailShareRequest(sendersEmail, Utility.GetFeedback(Constants.UserFeedBack.ReceiversEmail), Utility.GetFeedback(Constants.UserFeedBack.Subject), FeedbackMessage);
            }
            ProcessRequest(request, ProcessReportErrorResponse, null, false);
        }

        /// <summary>
        /// This method processes the report error response
        /// </summary>
        /// <param name="response">Response object</param>
        private void ProcessReportErrorResponse(Response response)
        {
            if (response.GetType() == typeof(MailShareResponse))
            {
                MailShareResponse mailShareResponse = response as MailShareResponse;
                ReportErrorStatus = mailShareResponse.Success ? SlateProperties.Resources.ReportErrorSuccess : SlateProperties.Resources.ReportErrorFailure;
            }
        }

        
    }
}
