using System;
using System.ComponentModel;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.Interfaces;
using NDTV.SlateApp.Properties;
using System.Collections.Specialized;

namespace NDTV.SlateApp.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyCollectionChanged, IDisposable
    {
        protected bool showOfflineErrorMessage;        
        
        /// <summary>
        /// Default constructor
        /// </summary>
        protected ViewModelBase()
        {
            showOfflineErrorMessage = true;
        }

        /// <summary>
        /// This starts the execution of the requests
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="processResponseMethod">Process response delegate</param>
        protected void ProcessRequest(Request request, ProcessResponse processResponseMethod)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                ApplicationData.Controller.ProcessRequest(request, processResponseMethod, HandleError);
            }
            else
            {
                if (showOfflineErrorMessage)
                {
                    (App.Current as App).DisplayErrorMessage(Resources.ContentLoadFailureMessage, string.Empty, false, null);
                }
                showOfflineErrorMessage = false;
            }
        }

        /// <summary>
        /// This starts the execution of the requests
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="processResponseMethod">Process response delegate</param>
        /// <param name="handleError">Handle error delegate</param>
        /// <param name="showErrorMessage">Flag denoting whether error message is to be shown or not</param>
        protected void ProcessRequest(Request request, ProcessResponse processResponseMethod, Action<Request> handleError, bool showErrorMessage)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                ApplicationData.Controller.ProcessRequest(
                    request,
                    processResponseMethod,
                    new Action<Exception>
                        ((exception) =>
                            {
                                if (null != handleError)
                                {
                                    handleError(request);
                                }
                                if (showErrorMessage)
                                {
                                    HandleError(exception);
                                }
                            }));
            }
            else
            {
                if (null != handleError)
                {
                    handleError(request);
                }
                if (showOfflineErrorMessage)
                {
                    (App.Current as App).DisplayErrorMessage(Resources.ContentLoadFailureMessage, string.Empty, false, null);
                }
                showOfflineErrorMessage = false;
            }
        }

        /// <summary>
        /// This will show the error message
        /// </summary>  
        /// <param name="exception">Exception</param>
        protected void HandleError(Exception exception)
        {
            (App.Current as App).DisplayErrorMessage(Resources.ContentLoadFailureMessage, string.Empty, true, exception);
        }

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This is the method that should be called by derived classes to raise the property change event notification 
        /// so that the binding systems refreshes the data on the UI.
        /// </summary>
        /// <param name="propertyName">propertyName that is changed / updated</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                PropertyChangedEventArgs arguments = new PropertyChangedEventArgs(propertyName);
                handler(this, arguments);
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        /// <summary>
        /// This is the method that should be called by derived classes to raise the property change event notification 
        /// so that the binding systems refreshes the data on the UI. Normally the dervied class will listen to the collection change and then 
        /// call this method with the parameters
        /// </summary>
        ///<param name="sender">sender</param>        
        ///<param name="arguments">NotifyCollectionChangedEventArgs</param> 
        protected void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs arguments)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null)
            {
                handler(sender, arguments);
            }
        }

        /// <summary>
        /// Dispose method to dispose all the properties used by the view model. This is to be implemented in all view models
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Individual dispose method for each of the view models
        /// </summary>
        /// <param name="disposing">flag denoting whether the disposing of the managed resources is to be done or not</param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeResources();
            }
        }

        /// <summary>
        /// Abstract method which is to be implemented in every view model
        /// </summary>
        protected virtual void DisposeResources()
        {
            //TODO: to be made abstract after implementing this in all the view models.
        }

    }
}
