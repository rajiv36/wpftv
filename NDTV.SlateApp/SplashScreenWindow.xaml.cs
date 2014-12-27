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
using System.Windows.Shapes;
using NDTV.Controller;

namespace NDTV.SlateApp
{
    /// <summary>
    /// Interaction logic for SplashScreenWindow.xaml
    /// </summary>
    public partial class SplashScreenWindow : SlateWindow
    {
        public SplashScreenWindow()
        {
            InitializeComponent();            
            if (ApplicationData.IsLandscapeOrientation)
            {
                LandscapeView.Visibility = Visibility.Visible;
                this.Width = 1024;
                this.Height = 600;
            }
            else
            {
                PortraitView.Visibility = Visibility.Visible;
                this.Height = 1024;
                this.Width = 600;
            }
        }

        private void OnWindowsLoad(object sender, RoutedEventArgs e)
        {
            PresentationSource source = PresentationSource.FromVisual(this);

            double dpiX, dpiY;
            if (source != null)
            {
                dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
                ApplicationData.DpiX = dpiX;
                //MessageBox.Show("DPI of your system is - X::  " + dpiX.ToString() + "  Y::  " + dpiY.ToString());
            }
        }        
    }
}
