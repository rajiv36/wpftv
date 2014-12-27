using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;


namespace NDTV.SlateApp.Controls
{
    public class CustomButton :Button
    {
        protected override void OnMouseDoubleClick(System.Windows.Input.MouseButtonEventArgs e)
        {
            //e.Handled = true;
        }
    }
}
