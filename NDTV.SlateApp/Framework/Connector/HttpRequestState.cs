using System.Net;

namespace NDTV.Connector
{
    /// <summary>
    /// This class is used to store the http request state
    /// </summary>
    public class HttpRequestState
    {
        /// <summary>
        /// Holds the HTTP web request object.
        /// </summary>
        public HttpWebRequest WebRequest { get; set; }

        /// <summary>
        /// Holds the body of the request during a post operation.
        /// </summary>
        public string RequestMessage { get; set; }
    }
}
