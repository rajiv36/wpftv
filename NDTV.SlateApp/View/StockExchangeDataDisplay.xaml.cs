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
using NDTV.Entities;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for StockExchangeDataDisplay.xaml
    /// </summary>
    public partial class StockExchangeDataDisplay : UserControl
    {
        public CompanySpecificStockData Stock
        {
            get { return (CompanySpecificStockData)GetValue(StockProperty); }
            set { SetValue(StockProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stock.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StockProperty =
            DependencyProperty.Register("Stock", typeof(CompanySpecificStockData), typeof(StockExchangeDataDisplay), new PropertyMetadata(OnStockPropertySet));

        private static void OnStockPropertySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as StockExchangeDataDisplay).AssignData();
        }

        private void AssignData()
        {
            this.LayoutRoot.DataContext = null;
            this.LayoutRoot.DataContext = this.Stock;
        }

        public StockExchangeDataDisplay()
        {
            InitializeComponent();
        }
    }
}
