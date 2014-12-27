using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using NDTV.Entities;
using NDTV.Utilities;

namespace NDTV.Connector
{
    /// <summary>
    /// This class is used to connect to a web service via Http
    /// </summary>
    public class HttpConnector
    {
        private Action<Stream> onLoad;
        private Action<Exception> onError;     

        /// <summary>
        /// This method does a Http get on the given URL and returns the result
        /// </summary>
        /// <param name="link">Input URL</param>
        /// <param name="onLoad">Method to be invoked on load</param>
        /// <param name="onError">Method to be invoked on error</param>
        /// <param name="header">Header</param>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <param name="authenticationType">Authentication type</param>
        /// <param name="enableCache">Flag to denote whether cache is enabled or not</param>
        /// <param name="requestContentType">Request content type</param>
        /// <returns>Response string</returns>
        public static void Get(string link, Action<Stream> onLoad, Action<Exception> onError, string header, string userName, string password, HttpAuthentication authenticationType, bool enableCache,string requestContentType)
        {
            HttpConnector connector = new HttpConnector(onLoad, onError);
            connector.Submit(link, Constants.HttpMethodNames.Get, header, string.Empty, userName, password, authenticationType,enableCache,requestContentType);
        }

        /// <summary>
        /// This method posts the given request message to the url and returns the result
        /// </summary>
        /// <param name="link">URL</param>
        /// <param name="onLoad">On load event</param>
        /// <param name="onError">On error event</param>
        /// <param name="requestMessage">Request message</param>
        /// <param name="header">Header</param>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <param name="authenticationType">Authentication type</param>
        /// <param name="enableCache">Flag to denote whether cache is enabled or not</param>
        /// <param name="requestContentType">Request content type</param>
        /// <returns>Response string</returns>
        public static void Post(string link, Action<Stream> onLoad, Action<Exception> onError, string requestMessage, string header, string userName, string password, HttpAuthentication authenticationType, bool enableCache, string requestContentType)
        {
            HttpConnector connector = new HttpConnector(onLoad, onError);
            connector.Submit(link, Constants.HttpMethodNames.Post, header, requestMessage, userName, password, authenticationType, enableCache,requestContentType);
        }
        
        /// <summary>
        /// Protected constructor to create the Http connector
        /// </summary>
        /// <param name="onLoad">Method to be fired on load</param>
        /// <param name="onError">Method to be fired on error</param>
        protected HttpConnector(Action<Stream> onLoad, Action<Exception> onError)
        {
            this.onLoad = onLoad;
            this.onError = onError;
        }

        /// <summary>
        /// This method process the Http request
        /// </summary>
        /// <param name="link">URL</param>
        /// <param name="method">Http operation method</param>
        /// <param name="header">Http header</param>
        /// <param name="requestMessage">Post message</param>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <param name="authenticationType">Authentication type</param>
        /// <param name="enableCache">Flag to denote whether cache is enabled or not</param>
        /// <param name="requestContentType">Request content type</param>
        protected void Submit(string link, string method, string header, string requestMessage, string userName, string password, HttpAuthentication authenticationType,bool enableCache, string requestContentType)
        {
            if (false == string.IsNullOrEmpty(link))
            {
                try
                {                   
                    HttpWebRequest webRequest = System.Net.WebRequest.Create(link) as HttpWebRequest;
                    webRequest.Method = method;
                    HttpRequestState requestState = new HttpRequestState();
                    requestState.WebRequest = webRequest;
                    HttpRequestCacheLevel cacheLevel = (enableCache) ? HttpRequestCacheLevel.CacheIfAvailable : HttpRequestCacheLevel.NoCacheNoStore;
                    webRequest.CachePolicy = new HttpRequestCachePolicy(cacheLevel);                   

                    if (authenticationType != HttpAuthentication.None)
                    {
                        if (false == string.IsNullOrEmpty(header))
                        {
                            // Regualar header inclusions (ex: Oauth headers).
                            webRequest.Headers[HttpRequestHeader.Authorization] = header;
                        }
                        else if (string.IsNullOrEmpty(header) && false == string.IsNullOrEmpty(userName) && false == string.IsNullOrEmpty(password))
                        {
                            if(authenticationType == HttpAuthentication.Basic)
                            {
                                // Authentication header.
                                string authHeader = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", userName, password);
                                byte[] authBytes = Encoding.UTF8.GetBytes(authHeader.ToCharArray());
                                webRequest.Headers[HttpRequestHeader.Authorization] = "Basic " + Convert.ToBase64String(authBytes);
                            }
                            else if (authenticationType == HttpAuthentication.Digest)
                            {
                                CredentialCache credentialCache = new CredentialCache();
                                credentialCache.Add(new Uri(link), "Digest", new NetworkCredential(userName, password));
                                webRequest.Credentials = credentialCache;
                            }
                        }                                               
                    }
                    if (method == Constants.HttpMethodNames.Post)
                    {
                        if (false == string.IsNullOrEmpty(requestMessage))
                        {                            
                            webRequest.ContentType = (string.IsNullOrWhiteSpace(requestContentType)) ? "application/x-www-form-urlencoded" : requestContentType;
                        }

                        if (false == string.IsNullOrEmpty(header))
                        {
                            // Regular header inclusions (ex: Oauth headers).
                            webRequest.Headers[HttpRequestHeader.Authorization] = header;
                        }

                        requestState.RequestMessage = requestMessage;
                        webRequest.BeginGetRequestStream(new AsyncCallback(ServiceRequestCallback), requestState);
                    }                    
                    else
                    {
                        if (false == string.IsNullOrEmpty(header))
                        {
                            // Regualar header inclusions (ex: Oauth headers).
                            webRequest.Headers[HttpRequestHeader.Authorization] = header;
                        }

                        webRequest.BeginGetResponse(new AsyncCallback(ServiceResponseCallback), requestState);
                    }
                }
                catch (ProtocolViolationException exception)
                {
                    this.RaiseException(exception);
                }
                catch (WebException exception)
                {
                    // to suppress the error 303 or 404
                    if (Utility.SuppressError(exception.Message.ToString()))
                    {
                        return;
                    }
                    this.RaiseException(exception);
                }
                catch (InvalidOperationException exception)
                {
                    this.RaiseException(exception);
                }
                catch (Exception exception)
                {
                    // to suppress the error 303 or 404, 404 was not caught in WebException
                    if (Utility.SuppressError(exception.Message.ToString()))
                    {
                        return;
                    }
                    this.RaiseException(exception);
                }
            }
            else
            {
                //ApplicationData.ErrorLogger.Log("URL sent is empty");
                this.RaiseException(null);
            }
        }

        /// <summary>
        /// The service request call back to write post data.
        /// </summary>
        /// <param name="asynchronousResult">Holds the request and postdata object.</param>
        private void ServiceRequestCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpRequestState requestState = (HttpRequestState)asynchronousResult.AsyncState;
                HttpWebRequest webRequest = requestState.WebRequest;
                Stream reqStream = webRequest.EndGetRequestStream(asynchronousResult);

                using (StreamWriter writer = new StreamWriter(reqStream))
                {
                    if (false == string.IsNullOrEmpty(requestState.RequestMessage))
                    {
                        writer.Write(requestState.RequestMessage);
                        writer.Flush();
                    }
                }

                webRequest.BeginGetResponse(new AsyncCallback(ServiceResponseCallback), requestState);
            }
            catch (IOException exception)
            {
                this.RaiseException(exception);
            }
            catch (WebException exception)
            {
                if (Utility.SuppressError(exception.Message.ToString()))
                {
                    return;
                }
                this.RaiseException(exception);
            }
            catch (ProtocolViolationException exception)
            {
                this.RaiseException(exception);
            }
            catch (InvalidOperationException exception)
            {
                this.RaiseException(exception);
            }
            catch (Exception exception)
            {
                if (Utility.SuppressError(exception.Message.ToString()))
                {
                    return;
                }
                this.RaiseException(exception);
            }
        }

        /// <summary>
        /// This is the response callback.
        /// </summary>
        /// <param name="asynchronousResult">Holds the request and state object.</param>
        private void ServiceResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                // State of request is asynchronous.                                                   
                HttpRequestState requestState = (HttpRequestState)asynchronousResult.AsyncState;
                HttpWebRequest webRequest = requestState.WebRequest;
                HttpWebResponse response = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);

                // Read the response into a Stream object.
                Stream responseStream = response.GetResponseStream();
                // Clear the exception.
                //this.errorHandler = null;
                onLoad(responseStream);
            }
            catch (WebException exception)
            {
                if (Utility.SuppressError(exception.Message.ToString()))
                {
                    return;
                }
                this.RaiseException(exception);
            }
            catch (InvalidOperationException exception)
            {
                this.RaiseException(exception);
            }
            catch (Exception exception)
            {
                if (Utility.SuppressError(exception.Message.ToString()))
                {
                    return;
                }
                this.RaiseException(exception);
            }
        }

        /// <summary>
        /// Raises the exception thrown during http async calls.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private void RaiseException(Exception exception)
        {
            //if (this.errorHandler != null && !string.IsNullOrEmpty(errorHandler.Message))
            //{
            //    // Log the call where the error was raised.
            //    ApplicationData.ErrorLogger.Log(this.errorHandler.Message);
            //}

            //// Log exception and raise the error.
            //ApplicationData.ErrorLogger.Log(exception);
            if (null != this.onError)
            {
                if (null != exception)
                {
                    this.onError(exception);
                }
                else
                {
                    this.onError(new Exception("Empty URL"));
                }
            }
        }
    }
}
