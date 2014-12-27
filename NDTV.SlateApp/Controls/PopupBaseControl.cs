using System.Windows;
using System.Windows.Controls;

namespace NDTV.SlateApp.Controls
{
    /// <summary>
    /// This serves as a base control for all the popups
    /// </summary>
    public class PopupBaseControl : HeaderedContentControl
    {
        /// <summary>
        /// Using a DependencyProperty as the backing store for HeaderText.  
        /// </summary>
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(PopupBaseControl), new UIPropertyMetadata(""));

        // Using a DependencyProperty as the backing store for FooterVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FooterVisibleProperty =
            DependencyProperty.Register("FooterVisible", typeof(Visibility), typeof(PopupBaseControl), new UIPropertyMetadata(Visibility.Visible));

        // Using a DependencyProperty as the backing store for FooterDisabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FooterEnabledProperty =
            DependencyProperty.Register("FooterEnabled", typeof(bool), typeof(PopupBaseControl), new UIPropertyMetadata(true));

        // Using a DependencyProperty as the backing store for CommentsVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommentsVisibleProperty =
            DependencyProperty.Register("CommentsVisible", typeof(bool), typeof(PopupBaseControl), new UIPropertyMetadata(false));

        /// <summary>
        /// Static constructor
        /// </summary>
        static PopupBaseControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupBaseControl), new FrameworkPropertyMetadata(typeof(PopupBaseControl)));
            CloseButtonClickedEvent = EventManager.RegisterRoutedEvent("CloseButtonClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), 
                typeof(PopupBaseControl));
        }

        /// <summary>
        /// Overridden method on applying the template. We pick the close button and assign the event on button click
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var closeButton = this.Template.FindName("CloseButton", this);
            if (null != closeButton)
            {
                (closeButton as Button).Click += PopupBaseCloseButtonClick;
            }
        }

        /// <summary>
        /// Event on click of the close button in the popup base control
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event arguments</param>
        private void PopupBaseCloseButtonClick(object sender, RoutedEventArgs e)
        {
            OnCloseButtonClicked();
        }

        private static RoutedEvent CloseButtonClickedEvent;

        /// <summary>
        /// Adds and removes the subscription to close button clicked event
        /// </summary>
        public event RoutedEventHandler CloseButtonClicked
        {
            add { AddHandler(CloseButtonClickedEvent, value); }
            remove { RemoveHandler(CloseButtonClickedEvent, value); }
        }

        /// <summary>
        /// Virtual method which invokes the events subscribed to the click of the pop up close button click
        /// </summary>
        protected virtual void OnCloseButtonClicked()
        {
            RoutedEventArgs arguments = new RoutedEventArgs();
            if (null != CloseButtonClickedEvent)
            {
                arguments.RoutedEvent = CloseButtonClickedEvent;
                RaiseEvent(arguments);
            }
        }

        /// <summary>
        /// Gets and sets the header text in the control
        /// </summary>
        public string HeaderText
        {
            get
            {
                return (string)GetValue(HeaderTextProperty);
            }
            set
            {
                SetValue(HeaderTextProperty, value);
            }
        }
      
        /// <summary>
        /// Gets and sets the footer visibility property
        /// </summary>
        public Visibility FooterVisible
        {
            get 
            { 
                return (Visibility)GetValue(FooterVisibleProperty); 
            }
            set 
            { 
                SetValue(FooterVisibleProperty, value); 
            }
        }


        /// <summary>
        /// Get or set footer disable property.
        /// </summary>
        public bool FooterEnabled
        {
            get
            {
                return (bool)GetValue(FooterEnabledProperty);
            }
            set
            {
                SetValue(FooterEnabledProperty, value);
            }
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

    }
}
