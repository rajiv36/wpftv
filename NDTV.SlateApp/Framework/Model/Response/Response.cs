using System;

namespace NDTV.Entities
{
    /// <summary>
    /// Base class for response
    /// </summary>
    public abstract class Response
    {
        protected string responseMessage;
        protected Exception responseException;

        /// <summary>
        /// Gets Response exception
        /// </summary>
        public Exception ResponseException
        {
            get
            {
                return responseException;
            }
        }

        /// <summary>
        /// Default constructor for the base class
        /// </summary>
        /// <param name="responseMessage">Actual response message</param>
        protected Response(string responseMessage)
        {
            this.responseMessage = responseMessage;
        }

        /// <summary>
        /// Constructor for the base class
        /// </summary>
        /// <param name="responseException">Response exception</param>
        protected Response(Exception responseException)
        {
            this.responseException = responseException;
        }
    }
}
