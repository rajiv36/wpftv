using System;
using System.Collections.Generic;
using NDTV.Entities;

namespace NDTV.SlateApp.Framework.Utilities
{
    /// <summary>
    ///  The class is a wrapper for ad banner dictionaries (Height, Widht and 
    ///  content). The class is based on singleton pattern. 
    /// </summary>
    public class AdBannerData
    {
        private static AdBannerData singletonAdBannerData;

        #region CONSTRUCTORS

        /// <summary>
        /// Default contructor.
        /// </summary>
        private AdBannerData()
        {

            HeightDictionary = new Dictionary<Pages, double>();
            WidthDictionary = new Dictionary<Pages, double>();
            AdContentDictionary = new Dictionary<Pages, string>();

            InitializeDictionaries();
        }

        #endregion

        #region PROPERTIES
        /// <summary>
        /// Returns singlton instance of Ad Banner class. 
        /// </summary>
        public static AdBannerData Instance
        {
            get
            {
                if (null == singletonAdBannerData)
                {
                    singletonAdBannerData = new AdBannerData();
                }
                return singletonAdBannerData;
            }
        }
        #endregion

        #region PUBLIC METHODS



        /// <summary>
        /// Get or set height dictionary.
        /// </summary>
        public Dictionary<Pages, Double> HeightDictionary
        {
            get;
            private set;
        }

        /// <summary>
        /// Get or Set widht dictionary.
        /// </summary>
        public Dictionary<Pages, Double> WidthDictionary
        {
            get;
            private set;
        }

        /// <summary>
        /// Get or set ad content dictionary.
        /// </summary>
        public Dictionary<Pages, string> AdContentDictionary
        {
            get;
            private set;
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Initilization Module.
        /// </summary>
        private void InitializeDictionaries()
        {
            InitializeWidthAndHeightDictionary();
            InitializeAdContentDictionary();
        }

        /// <summary>
        /// Initialize Width and Height dictionary.
        /// </summary>
        private void InitializeWidthAndHeightDictionary()
        {

            WidthDictionary.Add(Pages.None, 0);
            WidthDictionary.Add(Pages.Home, 300);
            WidthDictionary.Add(Pages.CricketLiveCommentary, 300);
            WidthDictionary.Add(Pages.CricketScorecard, 728);
            WidthDictionary.Add(Pages.CricketFullCommentary, 300);
            WidthDictionary.Add(Pages.LiveTV, 300);
            WidthDictionary.Add(Pages.NewsItems, 300);
            WidthDictionary.Add(Pages.PhotoGallerySmall, 290);
            WidthDictionary.Add(Pages.PhotoGalleryBig, 300);
            WidthDictionary.Add(Pages.VideoGallery, 728);
            WidthDictionary.Add(Pages.Weather, 300);
            WidthDictionary.Add(Pages.SensexScorecardSmall, 290);
            WidthDictionary.Add(Pages.SensexScorecardBig, 300);

            HeightDictionary.Add(Pages.None, 0);
            HeightDictionary.Add(Pages.Home, 250);
            HeightDictionary.Add(Pages.CricketLiveCommentary, 100);
            HeightDictionary.Add(Pages.CricketScorecard, 90);
            HeightDictionary.Add(Pages.CricketFullCommentary, 100);
            HeightDictionary.Add(Pages.LiveTV, 100);
            HeightDictionary.Add(Pages.NewsItems, 100);
            HeightDictionary.Add(Pages.PhotoGallerySmall, 50);
            HeightDictionary.Add(Pages.PhotoGalleryBig, 100);
            HeightDictionary.Add(Pages.VideoGallery, 90);
            HeightDictionary.Add(Pages.Weather, 250);
            HeightDictionary.Add(Pages.SensexScorecardSmall, 50);
            HeightDictionary.Add(Pages.SensexScorecardBig, 250);
        }

        /// <summary>
        /// Initialize ad content dictionary
        /// </summary>
        private void InitializeAdContentDictionary()
        {
            const string htmlStart = "<HTML> <HEAD> <meta http-equiv='Content-Type' content='text/html;charset=UTF-8'></HEAD><BODY scroll=NO><Style>body{margin-left: 0px; margin-top: 0px;}</Style>";
            const string htmlEnd = "</BODY> </HTML>";

            AdContentDictionary.Add(Pages.None, htmlStart + string.Empty + htmlEnd);

            AdContentDictionary.Add(Pages.Home, htmlStart
                                                 + "<!--Iframe Tag  --><!-- begin ZEDO for channel:  Slate HP , publisher: default , Ad Dimension: Medium Rectangle - 300 x 250 --><iframe src=\"http://d2.zedo.com/jsc/d2/ff2.html?n=767;c=186/185;d=9;w=300;h=250\" frameborder=0 marginheight=0 marginwidth=0 scrolling=\"no\" allowTransparency=\"true\" width=300 height=250></iframe><!-- end ZEDO for channel:  Slate HP , publisher: default , Ad Dimension: Medium Rectangle - 300 x 250 -->"
                                                 + htmlEnd);

            AdContentDictionary.Add(Pages.CricketLiveCommentary, htmlStart
                                                 + string.Empty
                                                 + htmlEnd);

            AdContentDictionary.Add(Pages.CricketScorecard, htmlStart +
                                                "<!--Iframe Tag  --><!-- begin ZEDO for channel:  Slate Cricket , publisher: default , Ad Dimension: Super Banner - 728 x 90 --><iframe src=\"http://d2.zedo.com/jsc/d2/ff2.html?n=767;c=187/185;d=14;w=728;h=90\" frameborder=0 marginheight=0 marginwidth=0 scrolling=\"no\" allowTransparency=\"true\" width=728 height=90></iframe><!-- end ZEDO for channel:  Slate Cricket , publisher: default , Ad Dimension: Super Banner - 728 x 90 -->"
                                                 + htmlEnd);

            AdContentDictionary.Add(Pages.CricketFullCommentary, htmlStart +
                                                "<!--Iframe Tag  --><!-- begin ZEDO for channel:  Slate Cricket , publisher: default , Ad Dimension: 300x100 - 300 x 100 --><iframe src=\"http://d2.zedo.com/jsc/d2/ff2.html?n=767;c=187/185;d=41;w=300;h=100\" frameborder=0 marginheight=0 marginwidth=0 scrolling=\"no\" allowTransparency=\"true\" width=300 height=100></iframe><!-- end ZEDO for channel:  Slate Cricket , publisher: default , Ad Dimension: 300x100 - 300 x 100 -->"
                                                 + htmlEnd);

            AdContentDictionary.Add(Pages.LiveTV, htmlStart +
                                                "<!--Iframe Tag  --><!-- begin ZEDO for channel:  Slate Video , publisher: default , Ad Dimension: 300x100 - 300 x 100 --><iframe src=\"http://d2.zedo.com/jsc/d2/ff2.html?n=767;c=188/185;d=41;w=300;h=100\" frameborder=0 marginheight=0 marginwidth=0 scrolling=\"no\" allowTransparency=\"true\" width=300 height=100></iframe><!-- end ZEDO for channel:  Slate Video , publisher: default , Ad Dimension: 300x100 - 300 x 100 -->"
                                                 + htmlEnd);

            AdContentDictionary.Add(Pages.NewsItems, htmlStart +
                                                "<!--Iframe Tag  --><!-- begin ZEDO for channel:  Slate News , publisher: default , Ad Dimension: 300x100 - 300 x 100 --><iframe src=\"http://d2.zedo.com/jsc/d2/ff2.html?n=767;c=189/185;d=41;w=300;h=100\" frameborder=0 marginheight=0 marginwidth=0 scrolling=\"no\" allowTransparency=\"true\" width=300 height=100></iframe><!-- end ZEDO for channel:  Slate News , publisher: default , Ad Dimension: 300x100 - 300 x 100 -->"
                                                + htmlEnd);

            AdContentDictionary.Add(Pages.PhotoGallerySmall, htmlStart +
                                                "<!--Iframe Tag  --><!-- begin ZEDO for channel:  Slate Photogallery , publisher: default , Ad Dimension: 290x50 - 290 x 50 --><iframe src=\"http://d2.zedo.com/jsc/d2/ff2.html?n=767;c=190/185;d=49;w=290;h=50\" frameborder=0 marginheight=0 marginwidth=0 scrolling=\"no\" allowTransparency=\"true\" width=290 height=50></iframe><!-- end ZEDO for channel:  Slate Photogallery , publisher: default , Ad Dimension: 290x50 - 290 x 50 -->"
                                                 + htmlEnd);

            AdContentDictionary.Add(Pages.PhotoGalleryBig, htmlStart +
                                                "<!--Iframe Tag  --><!-- begin ZEDO for channel:  Slate Photogallery , publisher: default , Ad Dimension: 300x100 - 300 x 100 --><iframe src=\"http://d2.zedo.com/jsc/d2/ff2.html?n=767;c=190/185;d=41;w=300;h=100\" frameborder=0 marginheight=0 marginwidth=0 scrolling=\"no\" allowTransparency=\"true\" width=300 height=100></iframe><!-- end ZEDO for channel:  Slate Photogallery , publisher: default , Ad Dimension: 300x100 - 300 x 100 -->"
                                                 + htmlEnd);

            AdContentDictionary.Add(Pages.VideoGallery, htmlStart +
                                                "<!--Iframe Tag  --><!-- begin ZEDO for channel:  Slate Cricket , publisher: default , Ad Dimension: Super Banner - 728 x 90 --><iframe src=\"http://d2.zedo.com/jsc/d2/ff2.html?n=767;c=187/185;d=14;w=728;h=90\" frameborder=0 marginheight=0 marginwidth=0 scrolling=\"no\" allowTransparency=\"true\" width=728 height=90></iframe><!-- end ZEDO for channel:  Slate Cricket , publisher: default , Ad Dimension: Super Banner - 728 x 90 -->"
                                                 + htmlEnd);

            AdContentDictionary.Add(Pages.Weather, htmlStart +
                                                "<!--Iframe Tag  --><!-- begin ZEDO for channel:  Slate Weather , publisher: default , Ad Dimension: Medium Rectangle - 300 x 250 --><iframe src=\"http://d2.zedo.com/jsc/d2/ff2.html?n=767;c=191/185;d=9;w=300;h=250\" frameborder=0 marginheight=0 marginwidth=0 scrolling=\"no\" allowTransparency=\"true\" width=300 height=250></iframe><!-- end ZEDO for channel:  Slate Weather , publisher: default , Ad Dimension: Medium Rectangle - 300 x 250 -->"
                                                 + htmlEnd);

            AdContentDictionary.Add(Pages.SensexScorecardSmall, htmlStart +
                                                "<!--Iframe Tag  --><!-- begin ZEDO for channel:  Slate Stock Indices , publisher: default , Ad Dimension: 290x50 - 290 x 50 --><iframe src=\"http://d2.zedo.com/jsc/d2/ff2.html?n=767;c=192/185;d=49;w=290;h=50\" frameborder=0 marginheight=0 marginwidth=0 scrolling=\"no\" allowTransparency=\"true\" width=290 height=50></iframe><!-- end ZEDO for channel:  Slate Stock Indices , publisher: default , Ad Dimension: 290x50 - 290 x 50 -->"
                                                 + htmlEnd);

            AdContentDictionary.Add(Pages.SensexScorecardBig, htmlStart +
                                                "<!--Iframe Tag  --><!-- begin ZEDO for channel:  Slate Stock Indices , publisher: default , Ad Dimension: Medium Rectangle - 300 x 250 --><iframe src=\"http://d2.zedo.com/jsc/d2/ff2.html?n=767;c=192/185;d=9;w=300;h=250\" frameborder=0 marginheight=0 marginwidth=0 scrolling=\"no\" allowTransparency=\"true\" width=300 height=250></iframe><!-- end ZEDO for channel:  Slate Stock Indices , publisher: default , Ad Dimension: Medium Rectangle - 300 x 250 -->"
                                                 + htmlEnd);
        }

        #endregion
    }
}
