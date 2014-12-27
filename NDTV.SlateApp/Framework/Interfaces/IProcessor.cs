using System;
using NDTV.Entities;

namespace NDTV.Interfaces
{
    /// <summary>
    /// Interface to process the request
    /// </summary>
    public interface IProcessor
    {
        /// <summary>
        /// This method process the request and returns the response
        /// </summary>
        /// <param name="request">Request entity</param>
        /// <param name="processResponse">Process Response delegate</param>     
        void Process(Request request, ProcessResponse processResponse, Action<Exception> onError);
    }   

    /// <summary>
    /// Delegate which will process the response on the caller
    /// </summary>
    /// <param name="response">Response object</param>
    public delegate void ProcessResponse(Response response);

    /// <summary>
    /// Delegate for Close Button of Main Window.
    /// </summary>
    public delegate void CloseButtonMainGalleryEventHandler();

    /// <summary>
    /// Delegate for close button of carousel view.
    /// </summary>
    public delegate void ImageAlbumCarouselCloseEventHandler();

}
