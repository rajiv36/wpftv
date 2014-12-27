using System;
using System.Diagnostics;
using System.Threading;
using NDTV.Controller;

namespace NDTV.SlateApp
{
    public class ApplicationMain
    {
        private const string applicationId = "a3b90e55-7793-40cb-bead-df329ea02e5a";

        /// <summary>
        /// The main method which launches the application
        /// </summary>
        /// <param name="args">start arguments</param>
        [STAThread]
        public static void Main(string[] args) 
        {
            try
            {
                Mutex mutex = new Mutex(true, applicationId);                
                if (mutex.WaitOne(TimeSpan.Zero, false))
                {
                    App app = new App();
                    app.InitializeComponent();                    
                    app.Run();
                    mutex.ReleaseMutex();
                }
                else
                {
                    Process[] processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
                    if (processes.Length > 1)
                    {
                        NativeMethods.ShowWindow(processes[0].MainWindowHandle, 9);
                        NativeMethods.SetForegroundWindow(processes[0].MainWindowHandle);
                    }
                }
            }
            catch (Exception ex)
            {
                //Catching the exception here in case the application crashes
                ApplicationData.ErrorLogger.Log(ex);                
            }
        }        
    }
}
