using System;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Controls;
using NDTV.Utilities;
using System.IO;
using System.Windows.Documents;



namespace NDTV.SlateApp.Controls
{
    /// <summary>
    /// A Textbox used for formatting html content into its equivalent XAMl representation
    /// </summary>
    public class RichTextBox : System.Windows.Controls.RichTextBox
    {
        /// <summary>
        /// Text property which acts as the bridge between the Vi ew and the code behind(dependency property)
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(RichTextBox),
            new FrameworkPropertyMetadata(String.Empty,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnTextPropertyChanged)));

        /// <summary>
        /// Text which takes in the html string directly..
        /// </summary>
        public string Text
        {
            get
            { 
                return (string)GetValue(TextProperty); 
            }
            set 
            { 
                SetValue(TextProperty, value); 
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public RichTextBox()
        {
            base.Loaded += textBoxLoaded;           
        }

        /// <summary>
        /// Textbox set to readonly and a trigger raised once the data gets binded to the UI..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxLoaded(object sender, RoutedEventArgs e)
        {
            base.IsReadOnly = true;
            base.Focusable = false;
            if (BindingOperations.GetBinding(this, TextProperty) != null && 
                (BindingOperations.GetBinding(this, TextProperty).UpdateSourceTrigger == UpdateSourceTrigger.PropertyChanged))
                    TextChanged += (o, ea) => InvokeTextChanged(); 
        }

        /// <summary>
        /// method thats called on text changed where the entire logic of converting HTML to XAML gets fired..
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlowDocument document = ((RichTextBox)d).Document;
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(Utility.ConvertHtmlToXaml(e.NewValue.ToString()))))
            {
                TextRange textRange = new TextRange(document.ContentStart, document.ContentEnd);
                textRange.Load(memoryStream, DataFormats.Xaml);
            }
        }

        /// <summary>
        /// Incase if the text changes we can probably fire the OnTextPropertyChanged event or the ChangeTheText
        /// method shown below..
        /// </summary>
        private bool alreadyInvoked;
        private void InvokeTextChanged()
        {
            if (!alreadyInvoked)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(ChangeTheText));
                alreadyInvoked = true;
            }
        }

        /// <summary>
        /// A method which is called where in if there is some data change in the text box..
        /// </summary>
        void ChangeTheText()
        {

        }
    }
}
