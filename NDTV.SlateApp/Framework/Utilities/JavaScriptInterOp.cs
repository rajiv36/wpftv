using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using NDTV.Controller;
using Microsoft.CSharp.RuntimeBinder;
using System.Linq;
using mshtml;
using System.Reflection;

namespace NDTV.SlateApp.Framework.Utilities
{
    /// <summary>
    /// Utitlity class to interact with java script from wpf web brwoser control.
    /// </summary>
    [ComVisibleAttribute(true)]
    public class JavaScriptInterOp
    {      
        /// <summary>
        /// Event to response on notification received. 
        /// </summary>       
        public event Action<Object, String> NotificationReceived;

        #region PUBLIC METHODS

        /// <summary>
        /// This function will be called from javascript.
        /// </summary>
        /// <param name="message"></param>
        public void Notify(string message)
        {
            if (false == String.IsNullOrEmpty(message))
            {
                OnNotificationReceived(message);
            }
        }


        public static void HideScriptErrors(WebBrowser wb, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            object objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null) return;
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
        } 

        /// <summary>
        /// This function helps in injecting java script methods in html.
        /// Call this function in webbrowser's LoadCompleted event and as the last statement, 
        /// </summary>
        /// <param name="webBrower"> eb Brwoser parameter</param>
        /// <param name="javaScriptFunction"> Java script Function. example :@"function btn1_OnClick(str){     alert('you clicked' + str);" </param>
        /// <returns> Returns true if script is injected success fully otherwise returns false.</returns>
        [ComVisible(false)]
        public static Boolean InjectJavaScriptFunction(WebBrowser webBrower, String javaScriptFunction, string styleText = null)
        {
            Boolean isScriptInjected = false;
            if ((null != webBrower) && (false == String.IsNullOrEmpty(javaScriptFunction)))
            {
                try
                {
                    dynamic document = webBrower.Document;
                    document.documentElement.style.overflow = "hidden";
                    if (null != document)
                    {
                        dynamic head = document.GetElementsByTagName("head")[0];
                        dynamic scriptElement = document.CreateElement("script");
                        scriptElement.type = @"text/javascript";
                        scriptElement.text = javaScriptFunction;
                        if (null != head)
                        {
                            head.AppendChild(scriptElement);
                            if (null != styleText)
                            {
                                IHTMLStyleSheet styleSheet = document.createStyleSheet("", 0);
                                styleSheet.cssText = styleText;
                            }
                            isScriptInjected = true;
                        }
                    }
                }
                catch (RuntimeBinderException binderException)
                {
                    ApplicationData.ErrorLogger.Log(binderException);
                }
                catch (AccessViolationException exception)
                {
                    ApplicationData.ErrorLogger.Log(exception);

                }
                catch (InvalidOperationException exception)
                {
                    ApplicationData.ErrorLogger.Log(exception);
                }
                catch (NullReferenceException exception)
                {
                    ApplicationData.ErrorLogger.Log(exception);
                }
            }
            return isScriptInjected;
        }

        /// <summary>
        /// Disable java script error.
        /// </summary>
        /// <param name="webBrower"> Webbrowser control</param>
        [ComVisible(false)]
        public static void DisableJavaScriptError(WebBrowser webBrower)
        {
            const String disableScriptError = @"function noError() { return true; }noError(); window.onerror = noError; ";
            JavaScriptInterOp.InjectJavaScriptFunction(webBrower, disableScriptError);
        }

        #endregion

        #region PROTECTED METHDOS

        /// <summary>
        /// Raise event to respond on notification received. 
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnNotificationReceived(String message)
        {
            if (null != NotificationReceived)
            {
                NotificationReceived(this, message);
            }
        }

        #endregion
    }
}
