using System;
using System.Collections.ObjectModel;
using NDTV.Entities;

namespace NDTV.SlateApp.Framework.CustomEventArgs
{
    /// <summary>
    ///Arguments class for any album click in the carousel.
    /// </summary>
    public class CarouselAlbumArgs : EventArgs
    {
        /// <summary>
        /// Selected album.
        /// </summary>
        private ImageAlbum selectedAlbum;

        /// <summary>
        /// Selected album.
        /// </summary>
        public ImageAlbum SelectedAlbum
        {
            get { return selectedAlbum; }
            set { selectedAlbum = value; }
        }

        /// <summary>
        /// List of related albums.
        /// </summary>
        private ObservableCollection<ImageAlbum> relatedAlbumLists;

        /// <summary>
        /// List of related albums.
        /// </summary>
        public ObservableCollection<ImageAlbum> RelatedAlbumLists
        {
            get { return relatedAlbumLists; }
            set { relatedAlbumLists = value; }
        }
    }
}
