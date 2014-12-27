using System;
using System.Collections.ObjectModel;
using NDTV.Entities;

namespace NDTV.SlateApp.Framework.CustomEventArgs
{
    /// <summary>
    /// Image gallery agreements class which is passes with the associated raised event.
    /// </summary>
    public class ImageGalleryArgs : EventArgs
    {
        /// <summary>
        /// Image list of the selected album.
        /// </summary>
        private ObservableCollection<ImageItem> imageList;

        /// <summary>
        /// Image list of the selected album.
        /// </summary>
        public ObservableCollection<ImageItem> ImageList
        {
            get { return imageList; }
            set { imageList = value; }
        }

        /// <summary>
        /// Selected Index of the Image.
        /// </summary>
        private int selectedIndex;

        /// <summary>
        /// Selected Index of the Image.
        /// </summary>
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; }
        }
    }
}
