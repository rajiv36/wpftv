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
    public partial class LandingPageSearchPopup : UserControl
    {
        /// <summary>
        /// Search Started Event
        /// </summary>
        public event EventHandler<SearchStartedEventArgs> SearchStarted;

        /// <summary>
        /// Constructor
        /// </summary>
        public LandingPageSearchPopup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler to indicate that the search has started
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Search Started Event arguments</param>
        private void OnSearchStarted(object sender, SearchStartedEventArgs e)
        {
            if (null != SearchStarted)
            {
                SearchStarted(this, new SearchStartedEventArgs(e.SearchString));
            }
        }
    }
}
