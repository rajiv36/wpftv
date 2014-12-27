using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDTV.Interfaces;
using NDTV.Entities;
using NDTV.Connector;
using System.IO;

namespace NDTV.Processor
{
    /// <summary>
    /// This class processes the request and get the response
    /// </summary>
    public class NDTVProcessor : IProcessor
    {
        /// <summary>
        /// This method process the request and returns the response
        /// </summary>
        /// <param name="request">Request entity</param>
        /// <param name="processResponse">Method which processes the response</param>
        /// <param name="onError">Method which is invoked on error</param>
        public void Process(Request request, ProcessResponse processResponse, Action<Exception> onError)
        {            
            switch (request.RequestOperation)
            {
                case HttpOperation.Get:
                    HttpConnector.Get(request.RequestLink,
                        new Action<Stream>
                            ((responseStream) =>
                                {
                                    request.BuildResponse(responseStream);
                                    if (null != processResponse)
                                    {
                                        processResponse(request.ResponseObject);
                                    }
                                }),onError, request.RequestHeader, request.RequestUserName, request.RequestPassword,request.RequestAuthentication,request.EnableCache,request.RequestContentType);
                    break;                 
                case HttpOperation.Post :
                    HttpConnector.Post(request.RequestLink,
                       new Action<Stream>
                           ((responseStream) =>
                           {
                               request.BuildResponse(responseStream);
                               if (null != processResponse)
                               {
                                   processResponse(request.ResponseObject);
                               }
                           }), onError, request.RawRequest, request.RequestHeader, request.RequestUserName, request.RequestPassword, request.RequestAuthentication, request.EnableCache, request.RequestContentType);
                    break;              
            }            
        }        
    }
}
