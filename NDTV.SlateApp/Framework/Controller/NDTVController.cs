using System;
using NDTV.Entities;
using NDTV.Interfaces;
using NDTV.Processor;
using NDTV.Utilities;

namespace NDTV.Controller
{
    /// <summary>
    /// Controller class
    /// </summary>
    public class NDTVController
    {
        /// <summary>
        /// Processes the given request
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="processResponseDelegate">Method which is to be executed after getting the response</param>
        public void ProcessRequest(Request request, ProcessResponse processResponseDelegate, Action<Exception> handleError)
        {
            IProcessor processor = new NDTVProcessor();
            processor.Process(request,
                                processResponseDelegate,
                                    new Action<Exception>(
                                        (exception) =>
                                        {
                                            ApplicationData.ErrorLogger.Log(exception);
                                            //TODO need to suppress 304 not modified error. need to remove and suppress it in elegant manner
                                            if (Utility.SuppressError(exception.Message.ToString()))
                                            {
                                                return;
                                            }
                                            if (null != handleError)
                                            {
                                                handleError(exception);
                                            }

                                        }));
        }
    }
}
