using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace NDTV.SlateApp
{
    /// <summary>
    /// This class contains the native methods
    /// </summary>
    internal class NativeMethods
    {       
        /// <summary>
        /// Sets foreground window
        /// </summary>
        /// <param name="mainWindowHandle">Main window handle</param>
        /// <returns>Integer</returns>
        [DllImport("User32")]
        public static extern int SetForegroundWindow(IntPtr mainWindowHandle);

        /// <summary>
        /// Shows the window
        /// </summary>
        /// <param name="mainWindowHandle">Main window handle</param>
        /// <param name="commandShowIndex">Command show integer</param>
        /// <returns>Boolean</returns>
        [DllImportAttribute("User32.DLL")]
        public static extern bool ShowWindow(IntPtr mainWindowHandle, int commandShowIndex);
    }
}
