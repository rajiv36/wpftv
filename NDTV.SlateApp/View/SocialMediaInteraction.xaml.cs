using System.Windows;
using System.Windows.Controls;
using NDTV.SlateApp.ViewModel;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for SocialMediaInteraction.xaml
    /// </summary>
    public partial class SocialMediaInteraction : UserControl
    {
        /// <summary>
        /// The view model for the data context.
        /// </summary>
        private SocialMediaInteractionViewModel viewModel;

        /// <summary>
        /// The comments visible property indicates if the comments button
        /// should be visible or not.
        /// </summary>
        public static readonly DependencyProperty CommentsVisibleProperty =
            DependencyProperty.Register("CommentsVisible", typeof(bool), typeof(SocialMediaInteraction), new PropertyMetadata(true, OnCommentsVisibleChanged));

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialMediaInteraction()
        {
            InitializeComponent();
            this.viewModel = new SocialMediaInteractionViewModel();
            this.DataContext = this.viewModel;
        }

        /// <summary>
        /// Gets or sets the comments visible property.
        /// </summary>
        public bool CommentsVisible
        {
            get
            {
                return (bool)GetValue(CommentsVisibleProperty);
            }
            set
            {
                SetValue(CommentsVisibleProperty, value);
            }
        }

        /// <summary>
        /// Comments visible property changed.
        /// </summary>
        /// <param name="dpObj">Dependency object</param>
        /// <param name="dpEvntArgs">Dependency event arguments</param>
        private static void OnCommentsVisibleChanged(DependencyObject dpObj, DependencyPropertyChangedEventArgs dpEvntArgs)
        {
            if (null != dpObj && dpObj is SocialMediaInteraction)
            {
                SocialMediaInteraction media = dpObj as SocialMediaInteraction;
                media.commentsBtn.Visibility = media.CommentsVisible == true ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
