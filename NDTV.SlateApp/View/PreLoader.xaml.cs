using System.Windows.Controls;
using System.Windows;
using System;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for PreLoader.xaml
    /// </summary>
    public partial class PreLoader : UserControl
    {
        public event EventHandler plainEvent;

        private enum PropertyNames 
        {
           BusyText,
           IsBusy
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public PreLoader()
        {
            InitializeComponent();
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// Busy Text to be displayed.
        /// </summary>
        public string BusyText
        {
            get { return (string)GetValue(BusyTextProperty); }
            set { SetValue(BusyTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BusyText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BusyTextProperty =
            DependencyProperty.Register("BusyText", typeof(string), typeof(PreLoader), new PropertyMetadata(OnBusyTextPropertyChanged));

        /// <summary>
        /// Value to indicate if the Busy Indicator has to be displayed or not.
        /// </summary>
        public bool? IsBusy
        {
            get { return (bool?)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool?), typeof(PreLoader), new PropertyMetadata(default(bool?),OnIsBusyPropertyChanged,  new CoerceValueCallback(OnIsBusySet)));

        private static void OnIsBusyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PreLoader).ChangeValues(PropertyNames.IsBusy);
        }
        
        private static void OnBusyTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PreLoader).ChangeValues(PropertyNames.BusyText);
           
        }

        private static object OnIsBusySet(DependencyObject obj, object o)
        {
            return o;
        }
        
        private void ChangeValues(PropertyNames propertyValue)
        {
            switch (propertyValue)
            {
                case PropertyNames.BusyText:
                    this.BusyMessageTextBlock.Text = BusyText;
                    break;
                case PropertyNames.IsBusy:
                    if (true == IsBusy)
                    {
                        this.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        this.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    break;
            }
        }

        
    }
}
