using NDTV.Controller;
using NDTV.Entities;
using System;

namespace NDTV.SlateApp.ViewModel
{
    public class AppViewModel : ViewModelBase
    {
        private Action dataLoadComplete;
        private Action dataLoadError;

        /// <summary>
        /// Gets and sets the flag which indicates whether load is complete
        /// </summary>
        public bool IsLoadComplete
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AppViewModel()
        {
            LoadApplicationInformation();    
        }

        /// <summary>
        /// Overloaded constructor for the unit test cases
        /// </summary>
        public void LoadData()
        {
            IsLoadComplete = false;
            this.dataLoadComplete = new Action(() => { IsLoadComplete = true; });
            this.dataLoadError = null;
            VideoCategoriesRequest request = new VideoCategoriesRequest();
            ProcessRequest(request, LoadVideoCategoriesResponse, HandleLoadError, false);                      
        }

        /// <summary>
        /// This method loads the initial data which is required by the application
        /// </summary>
        /// <param name="dataLoadComplete">Action to be performed when data load is complete</param>
        /// <param name="dataLoadError">Action to be performed where data load fails</param>
        public void LoadData(Action dataLoadComplete, Action dataLoadError)
        {
            //TODO : Move all the synchronous calls into their respective view models
            this.dataLoadComplete = dataLoadComplete;
            this.dataLoadError = dataLoadError;
            VideoCategoriesRequest request = new VideoCategoriesRequest();            
            ProcessRequest(request, LoadVideoCategoriesResponse,HandleLoadError,false);                        
        }

        /// <summary>
        /// This method handles the load error
        /// </summary>
        /// <param name="request">Request for which load failed</param>
        private void HandleLoadError(Request request)
        {
            if (null != this.dataLoadError)
            {
                dataLoadError();
            }
        }
        

        /// <summary>
        /// This method loads the initial data
        /// </summary>
        private void LoadApplicationInformation()
        {
            //TODO :            
            //1. Cache the categories -> Image, video and news categories
            //2. Invoke the categories fetch and update only the delta
        }

        

        /// <summary>
        /// Loads the content after it has been retrieved
        /// </summary>
        /// <param name="response">Response</param>
        private void LoadVideoCategoriesResponse(Response response)
        {
            if (response.GetType() == typeof(VideoCategoriesResponse))
            {
                ApplicationData.RelatedVideoList = ((VideoCategoriesResponse)response).VideoCategoryList;      
                ImageCategoriesRequest imageCategoryRequest = new ImageCategoriesRequest();
                ProcessRequest(imageCategoryRequest, LoadImageCategoriesResponse, HandleLoadError,false);
            }            
        }

        /// <summary>
        /// Loads the Image categories content after it has been retrieved
        /// </summary>
        /// <param name="response">Response</param>
        private void LoadImageCategoriesResponse(Response response)
        {
            if (response.GetType() == typeof(ImageCategoriesResponse))
            {
                ApplicationData.ImagesCategoryList = ((ImageCategoriesResponse)response).ImageCategoryCollection;
            }
            WeatherCitiesRequest citiesRequest = new WeatherCitiesRequest();
            ProcessRequest(citiesRequest, LoadcitiesResponse, HandleLoadError, false);
        } 
      
        /// <summary>
        /// Loads the Cities response
        /// </summary>
        /// <param name="response">Response</param>
        private void LoadcitiesResponse(Response response)
        {
            if (response.GetType() == typeof(WeatherCitiesResponse))
            {
                ApplicationData.IndianCities = ((WeatherCitiesResponse)response).cities.IndianCities;
                ApplicationData.ForeignCities = ((WeatherCitiesResponse)response).cities.ForeignCities;
            }            
            ProcessRequest(new AboutNDTVRequest(), LoadAboutNDTVText, HandleLoadError, false);
        }

        /// <summary>
        /// Loads the about NDTV text
        /// </summary>
        /// <param name="response"></param>
        private void LoadAboutNDTVText(Response response)
        {
            if (response.GetType() == typeof(AboutNDTVResponse))
            {
                ApplicationData.AboutNdtvText = (response as AboutNDTVResponse).AboutNdtvText;
            }
            if (null != this.dataLoadComplete)
            {
                dataLoadComplete();
            }
        }


        /// <summary>
        /// Disposes all the resources in ApplicationData before closing the application
        /// </summary>
        protected override void DisposeResources()
        {
            ApplicationData.MatchFixtureResponse = null;
            ApplicationData.RelatedVideoList = null;
            ApplicationData.StockMarketList = null;
            ApplicationData.TotalImagesOfSelectedAlbum = null;
            
            base.Dispose();
        }
    }
}
