using System.IO;
using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Base class for request
    /// </summary>
    public abstract class Request
    {
        private string requestName;
        protected string requestLink;
        private HttpOperation operation;
        protected string rawRequest;
        protected string requestHeader;
        protected string requestUserName;
        protected string requestPassword;
        protected Response response;
        protected HttpAuthentication authentication;
        protected bool enableCache;
        protected string requestContentType;

        /// <summary>
        /// Gets request URL
        /// </summary>
        public string RequestLink
        {
            get
            {
                return requestLink;
            }
        }

        /// <summary>
        /// Gets request operation
        /// </summary>
        public HttpOperation RequestOperation
        {
            get
            {
                return operation;
            }
        }

        /// <summary>
        /// Gets request authentication
        /// </summary>
        public HttpAuthentication RequestAuthentication
        {
            get
            {
                return authentication;
            }
        }

        /// <summary>
        /// Get Raw request
        /// </summary>
        public string RawRequest
        {
            get
            {
                return rawRequest;
            }
        }

        /// <summary>
        /// Gets request header
        /// </summary>
        public string RequestHeader
        {
            get
            {
                return requestHeader;
            }
        }

        /// <summary>
        /// Gets request user name
        /// </summary>
        public string RequestUserName
        {
            get
            {
                return requestUserName;
            }
        }

        /// <summary>
        /// Gets request password
        /// </summary>
        public string RequestPassword
        {
            get
            {
                return requestPassword;
            }
        }

        /// <summary>
        /// Gets response object
        /// </summary>
        public Response ResponseObject
        {
            get
            {
                return response;
            }
        }

        /// <summary>
        /// Gets the flag which denotes whether the cache is enabled or not
        /// </summary>
        public bool EnableCache
        {
            get
            {
                return enableCache;
            }
        }

        /// <summary>
        /// Gets the web request content type
        /// </summary>
        public string RequestContentType
        {
            get
            {
                return requestContentType;
            }
        }

        /// <summary>
        /// Constructor for base class
        /// </summary>
        /// <param name="requestName">Request name</param>
        /// <param name="requestLink">Request URL</param>
        /// <param name="operation">Http operation to be performed</param>
        protected Request(string requestName, string requestLink, HttpOperation operation)
        {
            this.requestName = requestName;
            this.requestLink = requestLink;
            this.operation = operation;
            this.requestHeader = string.Empty;
            this.requestUserName = string.Empty;
            this.requestPassword = string.Empty;
            this.authentication = HttpAuthentication.None;
            rawRequest = string.Empty;
            enableCache = false;
            requestContentType = Constants.HttpRequestContentType.ApplicationFormLinkEncoded;
        }

        /// <summary>
        /// Overloaded constructor for base class
        /// </summary>
        /// <param name="requestName">Request name</param>
        /// <param name="requestLink">Request URL</param>
        /// <param name="operation">Http operation to be performed</param>
        /// <param name="requestHeader">Request Header</param>
        /// <param name="requestUserName">Request user name</param>
        /// <param name="requestPassword">Request password</param>
        /// <param name="requestAuthentication">Request authentication</param>
        protected Request(string requestName, string requestLink, HttpOperation operation, string requestHeader, string requestUserName, string requestPassword, HttpAuthentication requestAuthentication)
        {
            this.requestName = requestName;
            this.requestLink = requestLink;
            this.operation = operation;
            this.requestHeader = requestHeader;
            this.requestUserName = requestUserName;
            this.requestPassword = requestPassword;
            this.authentication = requestAuthentication;
            rawRequest = string.Empty;
            enableCache = false;
            requestContentType = Constants.HttpRequestContentType.ApplicationFormLinkEncoded;
        }

        /// <summary>
        /// This method sets the response object
        /// </summary>
        /// <param name="stream">Response stream</param>
        /// <returns>Response object</returns>
        public void BuildResponse(Stream responseStream)
        {
            string responseString = string.Empty;
            using (StreamReader reader = new StreamReader(responseStream))
            {
                responseString = reader.ReadToEnd();
            }
            response = BuildResponseObject(responseString);
        }

        /// <summary>
        /// Abstract method which is used to build response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns></returns>
        protected abstract Response BuildResponseObject(string responseString);

        /// <summary>
        /// This is the virtual method which is to be overridden by the derived class to build URL in case of get scenarios
        /// </summary>
        /// <returns>Resultant URL</returns>
        protected virtual string BuildRequestLink()
        {
            return requestLink;
        }

        /// <summary>
        /// This is the virtual method which is to be overridden by the derived class to build the full request in case of post scenarios
        /// </summary>
        /// <returns>Full request</returns>
        protected virtual string BuildFullRequest()
        {
            return rawRequest;
        }

        /// <summary>
        /// Gets the request URL from the 
        /// </summary>
        /// <param name="linkName"></param>
        /// <returns></returns>
        protected string GetRequestLink(string linkName)
        {
            return Utility.GetLink(linkName);
        }
    }
}
