using System;

namespace NDTV.Entities
{

    /// <summary>
    ///  Holds Ad banner related data.
    /// </summary>
    public class AdBannerItem
    {

        #region  CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AdBannerItem()
        {

            AdWidth = new Double();
            AdHeight = new Double();
            AdContent = String.Empty;
            AdOwner = Pages.None;
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Ad Owner indicates current page.
        /// </summary>
        public Pages AdOwner
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set Ad Width for the current page.
        /// </summary>
        public Double AdWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set Ad Height for the current page.
        /// </summary>
        public Double AdHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set Ad content for the current page.
        /// </summary>
        public String AdContent
        {
            get;
            set;
        }


        #endregion

    }
}
