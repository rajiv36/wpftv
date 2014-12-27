using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDTV.Entities;
using NDTV.SlateApp.Framework.Utilities;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// Ad Banner view model. 
    /// </summary>
    public class AdBannerViewModel : ViewModelBase
    {
        private AdBannerData adBannerData;

        #region  CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// 
        public AdBannerViewModel()
        {
            adBannerData = AdBannerData.Instance;
            AdBanner = new AdBannerItem();
            CurrentPage = Pages.None;
        }


        #endregion

        #region PROPERTIES
        /// <summary>
        /// Get or set ad banner Item.
        /// </summary>
        public AdBannerItem AdBanner
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set current page, also on set page, updates the Height, 
        /// Width and Content for the current page.
        /// </summary>
        public Pages CurrentPage
        {
            set
            {
                AdBanner.AdOwner = value;
                AdBanner.AdWidth = adBannerData.WidthDictionary[value];
                AdBanner.AdHeight = adBannerData.HeightDictionary[value];
                AdBanner.AdContent = adBannerData.AdContentDictionary[value];
            }
            get
            {
                return AdBanner.AdOwner;
            }
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Disposes the objects created in view model
        /// </summary>
        protected override void DisposeResources()
        {
            this.adBannerData = null;
            this.AdBanner = null;
        }

        #endregion
    }
}
