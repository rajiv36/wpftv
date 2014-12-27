
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.Properties;
using NDTV.SlateApp.View;
using NDTV.Utilities;

namespace NDTV.SlateApp.ViewModel
{
    public class MailShareViewModel : ViewModelBase
    {
        /// <summary>
        /// Sender name
        /// </summary>
        private string senderName = string.Empty;

        /// <summary>
        /// Sender email
        /// </summary>
        private string senderEmail = string.Empty;

        /// <summary>
        /// Receiver name
        /// </summary>
        private string receiverName = string.Empty;

        /// <summary>
        /// Receiver email address
        /// </summary>
        private string receiverEmail = string.Empty;

        /// <summary>
        /// Mail body
        /// </summary>
        private string mailBody = string.Empty;

        /// <summary>
        /// Sender mandatory
        /// </summary>
        private bool isSenderMandatory = false;

        /// <summary>
        /// Receiver mandatory
        /// </summary>
        private bool isReceiverMandatory = false;

        /// <summary>
        /// The mail header.
        /// </summary>
        private string mailHeader = string.Empty;

        /// <summary>
        /// Email regular expression validation
        /// </summary>
        private const string EmailValidationExp = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                 + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
		                        		[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                 + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
		                        		[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                 + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        /// <summary>
        /// Mail share view.
        /// </summary>
        private MailShare mailShare;

        /// <summary>
        /// Gets or sets the sender name.
        /// </summary>
        public string SenderName
        {
            get
            {
                return this.senderName;
            }
            set
            {
                this.senderName = value;
                OnPropertyChanged("SenderName");
            }
        }

        /// <summary>
        /// Gets or sets the sender email.
        /// </summary>
        public string SenderEmail
        {
            get
            {
                return this.senderEmail;
            }
            set
            {
                this.senderEmail = value;
                // Check for validity of the sender email id.
                if (this.ValidateEmailAddress(value.Trim(), false))
                {
                    OnPropertyChanged("SenderEmail");
                    this.IsSenderMandatory = false;
                }
                else
                {
                    this.IsSenderMandatory = true;
                }
                this.CheckIsSendEnabled();
            }
        }

        /// <summary>
        /// Gets or sets the receiver name.
        /// </summary>
        public string ReceiverName
        {
            get
            {
                return this.receiverName;
            }
            set
            {
                this.receiverName = value;
                OnPropertyChanged("ReceiverName");
            }
        }

        /// <summary>
        /// Gets or sets the receiver email.
        /// </summary>
        public string ReceiverEmail
        {
            get
            {
                return this.receiverEmail;
            }
            set
            {
                this.receiverEmail = value;
                // Check for validity of the receivers email ids.                
                if (this.ValidateEmailAddress(value.Trim(), true))
                {
                    OnPropertyChanged("SenderEmail");
                    this.IsReceiverMandatory = false;
                }
                else
                {
                    // Set the error message.                    
                    this.IsReceiverMandatory = true;
                }
                this.CheckIsSendEnabled();
            }
        }

        /// <summary>
        /// Gets or sets the mail body.
        /// </summary>
        public string MailBody
        {
            get
            {
                return this.mailBody;
            }
            set
            {
                this.mailBody = value;
                OnPropertyChanged("MailBody");
                this.CheckIsSendEnabled();
            }
        }

        /// <summary>
        /// Is sender mandatory
        /// </summary>
        public bool IsSenderMandatory
        {
            get
            {
                return isSenderMandatory;
            }
            set
            {
                this.isSenderMandatory = value;
                OnPropertyChanged("IsSenderMandatory");
            }
        }

        /// <summary>
        /// Is receiver mandatory
        /// </summary>
        public bool IsReceiverMandatory
        {
            get
            {
                return isReceiverMandatory;
            }
            set
            {
                this.isReceiverMandatory = value;
                OnPropertyChanged("IsReceiverMandatory");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if 
        /// the send command should be enabled.
        /// </summary>
        private bool isSendEnabled;
        public bool IsSendEnabled
        {
            get
            {
                return this.isSendEnabled;
            }
            set
            {
                this.isSendEnabled = value;
                OnPropertyChanged("IsSendEnabled");
            }
        }

        /// <summary>
        /// Holds the formatted reciever email id.
        /// </summary>
        public string FormattedReceiverEmail
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the send mail.
        /// </summary>
        private RelayCommand sendMail;
        public RelayCommand SendMail
        {
            get
            {
                if (null == this.sendMail)
                {
                    this.sendMail = new RelayCommand(SendShareMail);
                }
                return this.sendMail;
            }
        }

        /// <summary>
        /// Gets the send mail command.
        /// </summary>
        private RelayCommand sendMailCommand;
        public RelayCommand SendMailCommand
        {
            get
            {
                if (null == this.sendMailCommand)
                {
                    this.sendMailCommand = new RelayCommand(HandleSendMail);
                }
                return this.sendMailCommand;
            }
        }

        /// <summary>
        /// Gets the send mail command.
        /// </summary>
        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                if (null == this.cancelCommand)
                {
                    this.cancelCommand = new RelayCommand(HandleCancel);
                }
                return this.cancelCommand;
            }
        }

        /// <summary>
        ///  Get or Set Shorten Link
        /// </summary>
        public string ShortLink
        {
            get;
            set;
        }

        /// <summary>
        /// Handles the send mail feature.
        /// </summary>
        /// <param name="parameter">Parameter</param>
        private void HandleSendMail(object parameter)
        {
            if (ApplicationData.IsApplicationOnline && null != parameter)
            {
                this.senderName = string.Empty;
                this.receiverName = string.Empty;
                this.senderEmail = string.Empty;
                this.receiverEmail = string.Empty;

                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.mailShare = new MailShare();

                        this.SetMailVideoBody();
                        this.mailShare.Closed += MailShareClosed;
                        this.mailShare.Owner = App.Current.MainWindow;
                        this.mailShare.DataContext = this;
                        this.mailShare.ShowDialog();
                    }));
            }
        }

        /// <summary>
        /// Mail share closed.
        /// </summary>
        /// <param name="sender">Mail share dialog</param>
        /// <param name="e">Event arguments</param>
        private void MailShareClosed(object sender, EventArgs e)
        {
            ApplicationData.IsPopUpOpen = false;   
        }

        /// <summary>
        /// Sets the mail body based on the share type.
        /// </summary>
        private void SetMailVideoBody()
        {
            int subjectLineLimit = Convert.ToInt32( Resources.MailHeaderTitleLimit);
            int titleLimit = Convert.ToInt32(Resources.MailCaptionLimit);
            int descriptionLimit = Convert.ToInt32(Resources.MailDescriptionLimit);

            switch (ApplicationData.CurrentItem.ShareType)
            {
                case ShareMediaType.Article:
                    this.mailBody = String.Format(CultureInfo.InvariantCulture, Resources.MailBodyArticle,
                                    new object[] { Utility.TruncateString(ApplicationData.CurrentItem.Caption, titleLimit), 
                                        this.ShortLink,
                                        Utility.TruncateString(ApplicationData.CurrentItem.Description, descriptionLimit) });

                    this.mailHeader = String.Format(CultureInfo.InvariantCulture, Resources.MailHeaderArticle,
                                    new object[] { Utility.TruncateString(ApplicationData.CurrentItem.Caption, subjectLineLimit) });
                    break;

                case ShareMediaType.Video:
                    this.mailBody = String.Format(CultureInfo.InvariantCulture, Resources.MailBodyVideo,
                                    new object[] { Utility.TruncateString(ApplicationData.CurrentItem.Caption, titleLimit), 
                                        this.ShortLink,
                                        Utility.TruncateString(ApplicationData.CurrentItem.Description, descriptionLimit) });

                    this.mailHeader = String.Format(CultureInfo.InvariantCulture, Resources.MailHeaderVideo,
                                      new object[] { Utility.TruncateString(ApplicationData.CurrentItem.Caption, subjectLineLimit) });
                    break;

                case ShareMediaType.PhotoSet:
                    this.mailBody = String.Format(CultureInfo.InvariantCulture, Resources.MailBodyPhotoSet,
                                    new object[] { Utility.TruncateString(ApplicationData.CurrentItem.Caption, titleLimit), 
                                        this.ShortLink,
                                        Utility.TruncateString(ApplicationData.CurrentItem.Description, descriptionLimit) });

                    this.mailHeader = String.Format(CultureInfo.InvariantCulture, Resources.MailHeaderPhotoSet,
                                      new object[] { Utility.TruncateString(ApplicationData.CurrentItem.Caption, subjectLineLimit) });
                    break;

                case ShareMediaType.ScorecardLiveMatch:
                    this.mailBody = String.Format(CultureInfo.InvariantCulture, Resources.MailBodyCricketLiveScorecard,
                                    new object[] {Utility.TruncateString(ApplicationData.CurrentItem.Caption, titleLimit), 
                                        this.ShortLink,
                                        Utility.TruncateString(ApplicationData.CurrentItem.Description, descriptionLimit) });

                    this.mailHeader = String.Format(CultureInfo.InvariantCulture, Resources.MailHeaderCricketLiveScorecard,
                                      new object[] { Utility.TruncateString(ApplicationData.CurrentItem.Caption, subjectLineLimit) });
                    break;

                case ShareMediaType.ScorecardRecentMatch:
                    this.mailBody = String.Format(CultureInfo.InvariantCulture, Resources.MailBodyCricketRecentScorecard,
                                    new object[] { Utility.TruncateString(ApplicationData.CurrentItem.Caption, titleLimit), 
                                        this.ShortLink,
                                        Utility.TruncateString(ApplicationData.CurrentItem.Description, descriptionLimit) });
                    this.mailHeader = Resources.MailHeaderCricketRecentScorecard;
                    break;

                case ShareMediaType.LiveCommentary:
                     this.mailBody = String.Format(CultureInfo.InvariantCulture, Resources.MailBodyCricketLiveCommentary,
                                     new object[] { Utility.TruncateString(ApplicationData.CurrentItem.Caption, titleLimit), 
                                     this.ShortLink,
                                     Utility.TruncateString(ApplicationData.CurrentItem.Description, descriptionLimit) });
                    this.mailHeader = String.Format(CultureInfo.InvariantCulture, Resources.MailHeaderCricketLiveCommentary,
                                    new object[] { Utility.TruncateString(ApplicationData.CurrentItem.Caption, subjectLineLimit) });
                    break;

                case ShareMediaType.RecentCommentary:
                    this.mailBody = String.Format(CultureInfo.InvariantCulture, Resources.MailBodyCricketRecentCommentary,
                                    new object[] { Utility.TruncateString(ApplicationData.CurrentItem.Caption, titleLimit), 
                                        this.ShortLink,
                                        
                                        Utility.TruncateString(ApplicationData.CurrentItem.Description, descriptionLimit) });
                    this.mailHeader = String.Format(CultureInfo.InvariantCulture, Resources.MailHeaderCricketRecentCommentary,
                                    new object[] { Utility.TruncateString(ApplicationData.CurrentItem.Caption, subjectLineLimit) });
                    break;

                case ShareMediaType.RecentMatches:
                    this.mailBody = String.Format(CultureInfo.InvariantCulture, Resources.MailBodyCricketRecentMatchList,
                           new object[] { this.ShortLink });
                    this.mailHeader = Resources.MailHeaderCricketRecentMatchList;
                    break;
                
                case ShareMediaType.UpcomingMatches:
                    this.mailBody = String.Format(CultureInfo.InvariantCulture, Resources.MailBodyCricketUpcomingMatchList,
                           new object[] {this.ShortLink });
                    this.mailHeader = Resources.MailHeaderCricketUpcomingMatchList;
                    break;

                case ShareMediaType.Weather:
                    this.mailBody = String.Format(CultureInfo.InvariantCulture, Resources.MailBodyWeather,
                           new object[] { this.ShortLink });
                    this.mailHeader = Resources.MailHeaderWeather;
                    break;

                default:
                    break;
            }
        }

         /// <summary>
        /// Handles the send mail feature.
        /// </summary>        
        /// <param name="parameter">Parameter</param>
        private void HandleCancel(object parameter)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
             {
                 this.mailShare.Close();
             }));
        }

        /// <summary>
        /// Sends the mail containing data to share.
        /// </summary>
        /// <param name="parameter">Parameter</param>
        private void SendShareMail(object parameter)
        {
            if (this.ValidateEmailAddress(this.senderEmail, false) && this.ValidateEmailAddress(this.receiverEmail, true) 
                && false == String.IsNullOrEmpty(this.mailBody))
            {
                this.mailBody = "hey " + (false == string.IsNullOrEmpty(this.ReceiverName) ? this.ReceiverName : string.Empty) + "," +
                                this.mailBody +
                                (false == string.IsNullOrEmpty(this.SenderName) ? this.SenderName : string.Empty);
                string body = Helper.RemoveHtmlTags(this.mailBody);
                MailShareRequest mailShareRequest = new MailShareRequest(this.senderEmail, this.FormattedReceiverEmail, this.mailHeader, body);
                ProcessRequest(mailShareRequest, ProcessResponse, null, false);
            }
        }

        /// <summary>
        /// This method processes the report error response
        /// </summary>
        /// <param name="response">Response object</param>
        private void ProcessResponse(Response response)
        {
            if (response.ResponseException == null)
            {
                this.HandleCancel(string.Empty);
            }
        }

        /// <summary>
        /// Sets email address
        /// </summary>
        /// <param name="emailValue">Email id</param>
        /// <returns>Success/Failure</returns>
        private bool ValidateEmailAddress(string emailValue, bool allowMultiple)
        {
            if (!string.IsNullOrEmpty(emailValue))
            {
                if (emailValue.Contains(";"))
                {
                    bool isValid = true;
                    string[] email = emailValue.Split(new Char[] { ';',',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (email.Length > 1 && !allowMultiple)
                    {                        
                        return false;
                    }
                    
                    StringBuilder emailBuilder = new StringBuilder();
                    foreach (string emailAdd in email)
                    {                        
                        if (ValidateEmail(emailAdd.Trim()))
                        {
                            emailBuilder.Append(emailAdd.Trim());
                            emailBuilder.Append(",");
                        }
                        else
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid)
                    {
                        if (allowMultiple)
                        {
                            this.FormattedReceiverEmail = emailBuilder.ToString();
                            this.FormattedReceiverEmail = this.FormattedReceiverEmail.Remove(emailBuilder.Length - 1, 1);
                        }

                        //Remove the comma
                        emailBuilder.Remove(emailBuilder.Length - 1, 1);                        
                        return true;
                    }
                }
                else
                {
                    if (ValidateEmail(emailValue))
                    {
                        if (allowMultiple)
                        {
                            this.FormattedReceiverEmail = emailValue;
                        }

                        return true;
                    }
                }
            }
            
            return false;
        }

        /// <summary>
        /// Validates if the email id is valid
        /// </summary>
        /// <param name="emailValue">the email id.</param>
        /// <returns>True or false based on email id.</returns>
        private static bool ValidateEmail(string emailValue)
        {
            if (!string.IsNullOrEmpty(emailValue))
            {
                Regex rgx = new Regex(EmailValidationExp, RegexOptions.IgnoreCase);                
                MatchCollection matchCollection = rgx.Matches(emailValue.Trim());
                if (matchCollection.Count > 0)
                {
                    return true;
                }
            }            
            return false;
        }

        /// <summary>
        /// Determines if the send button should be enabled or not.
        /// </summary>
        private void CheckIsSendEnabled()
        {
            if (string.IsNullOrEmpty(this.mailBody.Trim()) || string.IsNullOrEmpty(this.senderEmail) || string.IsNullOrEmpty(this.receiverEmail)
                || this.isSenderMandatory || this.isReceiverMandatory)
            {
                this.IsSendEnabled = false;
            }
            else
            {
                this.IsSendEnabled = true;
            }
        }
    }
}
