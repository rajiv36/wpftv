using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDTV.Entities
{
    /// <summary>
    /// Class to hold values for Video Category list
    /// </summary>
    public class VideoListCategories
    {
        /// <summary>
        /// Link for the video album
        /// </summary>
        public string Link
        {
            get;
            set;
        }

        /// <summary>
        /// Title of the category
        /// </summary>
        public string CategoryTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Priority of the category
        /// </summary>
        public int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// Link of the image
        /// </summary>
        public string ImageLink
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set if the Current category is selected.
        /// </summary>
        public Boolean IsSelected
        {
            get;
            set;
        }
    }
}
