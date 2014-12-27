using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NDTV.SlateApp.Framework.CustomEventArgs;

namespace NDTV.SlateApp.View
{
    
    /// <summary>
    /// Interaction logic for SearchBoxUserControl.xaml
    /// </summary>
    public partial class SearchBoxUserControl : UserControl
    {
        public event EventHandler<SearchStartedEventArgs> SearchInitiated;

        /// <summary>
        /// The Search String
        /// </summary>
        private string searchString;

        public SearchBoxUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the Search Button Click Event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void OnSearchButtonClick(object sender, RoutedEventArgs e)
        {
            if (false == string.IsNullOrWhiteSpace(this.SearchTextBox.Text))
            {
                this.searchString = this.SearchTextBox.Text;
                this.SearchTextBox.Text = string.Empty;
                if (null != SearchInitiated)
                {
                    SearchInitiated(this, new SearchStartedEventArgs(this.searchString));
                }
            }
        }

        private void OnSearchTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (false == string.IsNullOrWhiteSpace(this.SearchTextBox.Text))
                    {
                        this.searchString = this.SearchTextBox.Text;
                        this.SearchTextBox.Text = string.Empty;
                        if (null != SearchInitiated)
                        {
                            SearchInitiated(this, new SearchStartedEventArgs(this.searchString));
                        }
                    }
                    break;
            }
        }
    }
}
