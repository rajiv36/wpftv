
using System;
using System.Globalization;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.Properties;
using NDTV.SlateApp.View;

namespace NDTV.SlateApp.ViewModel
{
    public class CommentsViewModel : ViewModelBase
    {
        private bool getComments;

        /// <summary>
        /// Indicates when the comments are obtained.
        /// </summary>
        public event EventHandler CommentsObtained;

        /// <summary>
        /// The comments UI control.
        /// </summary>
        private CommentsControl commentsUI;

        /// <summary>
        /// Login options UI.
        /// </summary>
        private LoginOptions loginOptions;

        /// <summary>
        /// Creates an instance of the comments view model.
        /// </summary>
        public CommentsViewModel()
        {
            // Retrieve comments.
            this.RetrieveComments(ApplicationData.CurrentItem.Guid);

            // Set the title data.
            this.SetTitleData();
        }

        /// <summary>
        /// Gets or sets the title data.
        /// </summary>
        private string titleData;
        public string TitleData
        {
            get
            {
                return this.titleData;
            }
            set
            {
                this.titleData = value;
                OnPropertyChanged("TitleData");
            }
        }

        /// <summary>
        /// Gets or sets the sub title data.
        /// </summary>
        private string subTitleData;
        public string SubTitleData
        {
            get
            {
                return this.subTitleData;
            }
            set
            {
                this.subTitleData = value;
                OnPropertyChanged("SubTitleData");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the post
        /// button should be enabled.
        /// </summary>
        private bool isPostEnabled;
        public bool IsPostEnabled
        {
            get
            {
                return this.isPostEnabled;
            }
            set
            {
                this.isPostEnabled = value;
                OnPropertyChanged("IsPostEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the login link should be visible.
        /// </summary>
        private bool loginVisible;
        public bool LoginVisible
        {
            get
            {
                return this.loginVisible;
            }
            set
            {
                this.loginVisible = value;
                OnPropertyChanged("LoginVisible");
            }
        }

        /// <summary>
        /// Gets or sets the comment data value.
        /// </summary>
        private string commentData;
        public string CommentData
        {
            get
            {
                return this.commentData;
            }
            set
            {
                this.commentData = value;
                this.SetPostIsEnabled();
                OnPropertyChanged("CommentData");
            }
        }

        /// <summary>
        /// Gets or sets the post response message.
        /// </summary>
        private string postMessage;
        public string PostMessage
        {
            get
            {
                return this.postMessage;
            }
            set
            {
                this.postMessage = value;
                OnPropertyChanged("PostMessage");
            }
        }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        private int currentPage;
        public int CurrentPage
        {
            get
            {
                return this.currentPage;
            }
            set
            {
                this.currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        private int totalPages;
        public int TotalPages
        {
            get
            {
                return this.totalPages;
            }
            set
            {
                this.totalPages = value;
                OnPropertyChanged("TotalPages");
            }
        }

        /// <summary>
        /// Gets or sets the total number of comments.
        /// </summary>
        private int totalComments;
        public int TotalComments
        {
            get
            {
                return this.totalComments;
            }
            set
            {
                this.totalComments = value;
                OnPropertyChanged("TotalComments");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the right arrow
        /// should be enabled.
        /// </summary>
        private bool rightArrowEnabled;
        public bool RightArrowEnabled
        {
            get
            {
                return this.rightArrowEnabled;
            }
            set
            {
                this.rightArrowEnabled = value;
                OnPropertyChanged("RightArrowEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the left arrow
        /// should be enabled.
        /// </summary>
        private bool leftArrowEnabled;
        public bool LeftArrowEnabled
        {
            get
            {
                return this.leftArrowEnabled;
            }
            set
            {
                this.leftArrowEnabled = value;
                OnPropertyChanged("LeftArrowEnabled");
            }
        }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        private Comments comments;
        public Comments Comments
        {
            get
            {
                return this.comments;
            }
            set 
            {
                this.comments = value;
                OnPropertyChanged("Comments");
            }
        }

        /// <summary>
        /// Gets the login click handler.
        /// </summary>
        private RelayCommand loginClick;
        public RelayCommand LoginClick
        {
            get
            {
                if (null == this.loginClick)
                {
                    this.loginClick = new RelayCommand(HandleLoginClick);
                }
                return this.loginClick;
            }
        }

        /// <summary>
        /// Gets the post comment handler.
        /// </summary>
        private RelayCommand postComment;
        public RelayCommand PostComment
        {
            get
            {
                if (null == this.postComment)
                {
                    this.postComment = new RelayCommand(HandlePostComment);
                }
                return this.postComment;
            }
        }

        /// <summary>
        /// Gets the cancel click handler.
        /// </summary>
        private RelayCommand cancelClick;
        public RelayCommand CancelClick
        {
            get
            {
                if (null == this.cancelClick)
                {
                    this.cancelClick = new RelayCommand(HandleCancelClick);
                }
                return this.cancelClick;
            }
        }

        /// <summary>
        /// Gets the comments per page.
        /// </summary>
        private RelayCommand getCommentsPerPage;
        public RelayCommand GetCommentsPerPage
        {
            get
            {
                if (null == this.getCommentsPerPage)
                {
                    this.getCommentsPerPage = new RelayCommand(RetrieveCommentsPerPage);
                }
                return this.getCommentsPerPage;
            }
        }

        /// <summary>
        /// Displays the comments UI.
        /// </summary>
        public void DisplayCommentsUI()
        {
            App.Current.MainWindow.Dispatcher.BeginInvoke(new Action(() =>
            {
                commentsUI = new CommentsControl();
                commentsUI.Owner = App.Current.MainWindow;
                this.SetTitleData();
                this.PostMessage = string.Empty;
                this.CommentData = string.Empty;
                commentsUI.DataContext = this;
                commentsUI.Closed += CommentsUIClosed;
                commentsUI.ShowDialog();
            }));
        }

        /// <summary>
        /// Handler for the closed event.
        /// </summary>
        /// <param name="sender">Comments Dialog</param>
        /// <param name="e">Event arguments</param>
        private void CommentsUIClosed(object sender, EventArgs e)
        {
            ApplicationData.IsPopUpOpen = false;   
        }

        /// <summary>
        /// This method handles the functionality to be carried out 
        /// on login click.
        /// </summary>
        /// <param name="parameter">Parameter</param>
        private void HandleLoginClick(object parameter)
        {
            this.loginOptions = new LoginOptions();
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    LoginOptionsViewModel loginOptionViewModel = new LoginOptionsViewModel();
                    loginOptionViewModel.CancelLogin += LoginOptionViewModelCancelLogin;
                    loginOptionViewModel.UserInstanceObtained += UserInstanceObtained;
                    loginOptions.Owner = App.Current.MainWindow;
                    loginOptions.DataContext = loginOptionViewModel;
                    loginOptions.ShowDialog();
                }));            
        }

        /// <summary>
        /// This method is invoked on cancel button click.
        /// </summary>
        /// <param name="sender">Login options control</param>
        /// <param name="e">Event arguments</param>
        private void LoginOptionViewModelCancelLogin(object sender, EventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.loginOptions.Close();
                }));
        }

        /// <summary>
        /// This method is invoked on user initialization complete.
        /// </summary>
        /// <param name="sender">Login options control</param>
        /// <param name="e">Event arguments</param>
        private void UserInstanceObtained(object sender, EventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.loginOptions.Close();
                    this.SetPostIsEnabled();
                    this.SetTitleData();
                }));            
        }

        /// <summary>
        /// This method handles the post comment functionality.
        /// </summary>
        /// <param name="parameter">Parameter</param>
        private void HandlePostComment(object parameter)
        {
            string ctype = ApplicationData.CurrentItem.ShareType == ShareMediaType.Article ? "story" : "video";
            string identifier = String.Format(CultureInfo.InvariantCulture, Resources.CommentsIdentifier,
                new object[] { ctype, ApplicationData.CurrentItem.Guid});

            string postData = String.Format(CultureInfo.InvariantCulture, Constants.CommentConstants.PostCommentPostData,
                new object[] { ApplicationData.UserInstance.UserId, ApplicationData.CurrentItem.Caption, 
                   Uri.EscapeUriString(ApplicationData.CurrentItem.Link), ctype , identifier, this.commentData });

            PostCommentRequest request = new PostCommentRequest(postData);
            ApplicationData.Controller.ProcessRequest(request, this.HandlePostResponse, this.HandleError);
        }
    
        /// <summary>
        /// Handles the response received on comment post.
        /// </summary>
        /// <param name="response"></param>
        private void HandlePostResponse(Response response)
        {
            PostCommentResponse commentsReponse = response as PostCommentResponse;
            this.PostMessage = commentsReponse.PostMessage;
        }

        /// <summary>
        /// This method handles the close of the comments UI.
        /// </summary>
        /// <param name="parameter">Parameter</param>
        private void HandleCancelClick(object parameter)
        {
            this.commentsUI.Close();
        }

        /// <summary>
        /// This method handles the functionality to obtain comments
        /// per page
        /// </summary>
        /// <param name="parameter">Parameter</param>
        private void RetrieveCommentsPerPage(object parameter)
        {
            if (parameter != null)
            {
                string navigated = parameter.ToString();
                if (!string.IsNullOrEmpty(navigated))
                {
                    if ((navigated.Equals("Back", StringComparison.OrdinalIgnoreCase)))
                    {
                        this.CurrentPage = this.CurrentPage - 1;
                        this.RetrieveCommentsPerPage(this.CurrentPage, ApplicationData.CurrentItem.Guid);
                    }
                    else
                    {
                        this.CurrentPage = this.CurrentPage + 1;
                        this.RetrieveCommentsPerPage(this.CurrentPage, ApplicationData.CurrentItem.Guid);
                    }
                }
            }
        }      

        /// <summary>
        /// Retrieves comments given a page number and media ID.
        /// </summary>
        private void RetrieveComments(string mediaId)
        {
            getComments = true;
            string commentsUrl = string.Format(CultureInfo.InvariantCulture, Constants.CommentConstants.RetrieveCommentsUrl, 
                new object[] { "1", mediaId });

            GetCommentsRequest commentsRequest = new GetCommentsRequest(Uri.EscapeUriString(commentsUrl));
            ApplicationData.Controller.ProcessRequest(commentsRequest, ObtainCommentsData, this.HandleError);
        }

        /// <summary>
        /// Gets comments specific to a page.
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="mediaId">Media ID</param>
        private void RetrieveCommentsPerPage(int pageNumber, string mediaId)
        {
            this.getComments = true;
            string commentsUrl = string.Format(CultureInfo.InvariantCulture, Constants.CommentConstants.RetrieveCommentsUrl, 
                new object[] { pageNumber.ToString(CultureInfo.InvariantCulture), mediaId });

            GetCommentsRequest commentsRequest = new GetCommentsRequest(Uri.EscapeUriString(commentsUrl));
            ApplicationData.Controller.ProcessRequest(commentsRequest, OnGetCommentsPagewiseComplete, this.HandleError);
        }

        /// <summary>
        /// This method obtains the response of the comments request.        
        /// </summary>
        /// <param name="response">Response</param>
        private void OnGetCommentsPagewiseComplete(Response response)
        {
            GetCommentsResponse commentsResponse = response as GetCommentsResponse;
            this.Comments = commentsResponse.Comments;
            this.getComments = false;
            this.SubTitleData = null == this.Comments || this.Comments.PagingDetails.TotalCount == 0 ? Resources.FirstToComment
                : string.Format(CultureInfo.InvariantCulture, Resources.AllComments, this.Comments.PagingDetails.TotalCount);
            this.TotalPages = this.SetNumberOfPages(this.Comments.PagingDetails.TotalCount, this.Comments.PagingDetails.ResultsPerPage);
            this.TotalComments = this.Comments.PagingDetails.TotalCount;

            this.LeftArrowEnabled = this.CurrentPage == 1 ? false: true;
            this.RightArrowEnabled = this.CurrentPage == this.TotalPages ? false : true;            
        }

        /// <summary>
        /// This method obtains the response of the facebook 
        /// access token request.
        /// </summary>
        /// <param name="response">Response</param>
        private void ObtainCommentsData(Response response)
        {
            GetCommentsResponse commentsResponse = response as GetCommentsResponse;
            this.Comments = commentsResponse.Comments;
            this.getComments = false;
            if (null != this.Comments.PagingDetails)
            {
                this.SubTitleData = null == this.Comments || this.Comments.PagingDetails.TotalCount == 0 ? Resources.FirstToComment
                    : string.Format(CultureInfo.InvariantCulture, Resources.AllComments, this.Comments.PagingDetails.TotalCount);

                this.TotalPages = this.SetNumberOfPages(this.Comments.PagingDetails.TotalCount, this.Comments.PagingDetails.ResultsPerPage);
                this.TotalComments = this.Comments.PagingDetails.TotalCount;

                // Set initial display page.
                this.CurrentPage = 1;
                this.LeftArrowEnabled = false;
                this.RightArrowEnabled = this.TotalPages == 1 ? false : true;
            }
            else
            {
                this.SubTitleData = Resources.FirstToComment;
                this.LeftArrowEnabled = false;
                this.RightArrowEnabled = false;
            }
            
            if (null != this.CommentsObtained)
            {
                this.CommentsObtained(null, null);
            }            
        }

        /// <summary>
        /// This will show the error message
        /// </summary>  
        /// <param name="exception">Exception</param>
        private void HandleError(Exception exception)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (this.getComments)
                {
                    (App.Current as App).DisplayErrorMessage(Resources.CommentPostError, string.Empty, false, exception);
                }
                else
                {
                    (App.Current as App).DisplayErrorMessage(Resources.GetCommentsError, string.Empty, false, exception);
                }
            }));
        }

        /// <summary>
        /// Determines if the post button should be enabled or not.
        /// </summary>
        private void SetPostIsEnabled()
        {
            if (false == string.IsNullOrEmpty(this.commentData) && null != ApplicationData.UserInstance)
            {
                this.IsPostEnabled = true;
            }
            else
            {
                this.IsPostEnabled = false;
            }
        }

        /// <summary>
        /// Sets the title based on whether a user is logged in or not.
        /// </summary>
        private void SetTitleData()
        {
            if (ApplicationData.FirstLoggedOn == LoginMode.None)
            {
                this.TitleData = Resources.PostCommentHeader;
                this.LoginVisible = true;                
            }
            else
            {
                this.TitleData = String.Format(CultureInfo.InvariantCulture, Resources.CommentsSubTitle,
                    new object[] { ApplicationData.UserInstance.DisplayName, ApplicationData.UserInstance.LoginMode.ToString() });
                this.LoginVisible = false;
            }
        }

        /// <summary>
        /// Sets the number of pages available.
        /// </summary>
        /// <param name="totalCount">The total number of comments.</param>
        /// <param name="resultPerPage">The number of comments displayed in each page.</param>
        /// <returns>The number of pages.</returns>
        private int SetNumberOfPages(int totalCount, int resultPerPage)
        {          
            int noOfPages = totalCount / resultPerPage;
            if (totalCount % resultPerPage == 0)
            {
                return noOfPages;
            }
            else
            {
                return noOfPages + 1;
            }            
        }
    }
}
