using System;
using System.Collections.ObjectModel;
using System.Linq;
using NDTV.Entities;
using NDTV.SlateApp.Framework.CustomEventArgs;
using NDTV.Utilities;
using System.Net;
using NDTV.SlateApp.Properties;
using NDTV.Controller;
using System.Globalization;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// Sensex Scorecard ViewModel.
    /// </summary>
    public class StockViewModel : ViewModelBase
    {
        #region Events
        /// <summary>
        /// Stock Loaded Event arguments.
        /// </summary>
        public event EventHandler<StocksLoadedEventArgs> StocksLoaded;

        #endregion

        #region Private Fields

        /// <summary>
        /// Variable to indicate if more data can be fetched
        /// </summary>
        private bool canFetchMoreData;

        /// <summary>
        /// Variable to indicate if the scorecard is busy or not
        /// </summary>
        private bool isScorecardBusy;

        /// <summary>
        /// Variable to indicate if the details pane is busy
        /// </summary>
        private bool isScorecardDetailsPaneBusy;

        /// <summary>
        /// Selected Company Details
        /// </summary>
        private CompanySpecificStockData selectedCompanyDetails;

        /// <summary>
        /// Index of the chosen company.
        /// </summary>
        private int chosenCompanyIndex;

        /// <summary>
        /// Page number of the current category;
        /// </summary>
        private int currentCategoryPageNumber;

        /// <summary>
        /// Maximum data associated with the link
        /// </summary>
        private int maxNumber;

        /// <summary>
        /// List of tab headers.
        /// </summary>
        public ObservableCollection<string> ActiveStockCategories { get; private set; }

        /// <summary>
        /// List of News Flash Items.
        /// </summary>
        public ObservableCollection<string> NewsflashItems { get; private set; }

        /// <summary>
        /// Details associated with the active Stock Market.
        /// </summary>
        public ObservableCollection<StockItemDetails> ActiveStockMarketDataDetails { get; private set; }

        /// <summary>
        /// Flash news object.
        /// </summary>
        private ProfitFlashNewsItem profitNewsFlashItem;

        /// <summary>
        /// Command to fetch data once the scroll viewer reaches the end of the list.
        /// </summary>
        private RelayCommand fetchDataCommand;

        /// <summary>
        /// Relay command to handle change in active categories.
        /// </summary>
        private RelayCommand onActiveCategoriesChangedCommand;

        /// <summary>
        /// Relay command to handle change of item.
        /// </summary>
        private RelayCommand onActiveItemChangedCommand;

        /// <summary>
        /// Current Link Name
        /// </summary>
        private string currentLinkName;

        #endregion

        #region Public Properties

        /// <summary>
        /// Variable to indicate if the scorecard is busy or not.
        /// </summary>
        public bool IsScorecardBusy
        {
            get { return isScorecardBusy; }
            set
            {
                isScorecardBusy = value;
                OnPropertyChanged("IsScorecardBusy");
            }
        }

        /// <summary>
        /// Variable to indicate of the Details busy or not.
        /// </summary>
        public bool IsScorecardDetailsPaneBusy
        {
            get { return isScorecardDetailsPaneBusy; }
            set
            {
                isScorecardDetailsPaneBusy = value;
                OnPropertyChanged("IsScorecardDetailsPaneBusy");
            }
        }

        /// <summary>
        /// Details associated with the selected Company.
        /// </summary>
        public CompanySpecificStockData SelectedCompanyDetails 
        {
            get { return selectedCompanyDetails; } 
            set 
            {
                selectedCompanyDetails = value;
                OnPropertyChanged("SelectedCompanyDetails");
            }
        }

        /// <summary>
        /// Index of the chosen company.
        /// </summary>
        public int ChosenCompanyIndex
        {
            get { return chosenCompanyIndex; }
            set
            {
                chosenCompanyIndex = value;
                OnPropertyChanged("ChosenCompanyIndex");
            }
        }

        /// <summary>
        /// Stock Ticker Details.
        /// </summary>
        public ObservableCollection<StockIndexItem> StockTickerDetails { get; set; }

        /// <summary>
        /// Related Business News collection
        /// </summary>
        public ObservableCollection<TopStoryItem> RelatedBusinessNews { get; private set; }

        /// <summary>
        /// Relay command to handle the fetching of more data when requested by the end user.
        /// </summary>
        public RelayCommand FetchDataCommand
        {
            get 
            {
                if (null == fetchDataCommand)
                {
                    fetchDataCommand = new RelayCommand((e)=>FetchNextData(e));
                }
                return fetchDataCommand; 
            }
        }

        /// <summary>
        /// Relay command to handle change in active categories.
        /// </summary>
        public RelayCommand OnActiveCategoriesChangedCommand
        {
            get 
            {
                if (null == onActiveCategoriesChangedCommand)
                {
                    onActiveCategoriesChangedCommand = new RelayCommand((e) => OnActiveCategoriesChanged(e));
                }
                return onActiveCategoriesChangedCommand; 
            }
        }

        /// <summary>
        /// Relay Command to handle change of item.
        /// </summary>
        public RelayCommand OnActiveItemChangedCommand
        {
            get 
            {
                if (null == onActiveItemChangedCommand)
                {
                    onActiveItemChangedCommand = new RelayCommand((e) => OnActiveItemChanged(e));
                }
                return onActiveItemChangedCommand; 
            }
        }

        /// <summary>
        /// Top Story Item
        /// </summary>
        public TopStory BusinessTopStoryItem { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public StockViewModel(ObservableCollection<StockIndexItem> stockTickerItems)
        {
            //TODO:Add following categories:"52-Week High","52-Week Low","Sensex Stocks","Nifty Stocks","Full Dashboard" to the below list.
            this.IsScorecardBusy = true;
            this.ActiveStockCategories = new ObservableCollection<string>() { Constants.Constant.MostActiveStocks, Constants.Constant.Gainers, Constants.Constant.Losers, Constants.Constant.YearHigh, Constants.Constant.YearLow, Constants.Constant.Sensex, Constants.Constant.Nifty,Constants.Constant.FullDashboard };
            this.ActiveStockMarketDataDetails = new ObservableCollection<StockItemDetails>();
            this.NewsflashItems = new ObservableCollection<string>();
            this.SelectedCompanyDetails = new CompanySpecificStockData();
            this.profitNewsFlashItem = new ProfitFlashNewsItem();
            this.StockTickerDetails = new ObservableCollection<StockIndexItem>();
            this.RelatedBusinessNews = new ObservableCollection<TopStoryItem>();
            this.StockTickerDetails = stockTickerItems;
            this.currentCategoryPageNumber = 1;
            this.canFetchMoreData = true;

            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                this.IsScorecardBusy = true;
                this.IsScorecardDetailsPaneBusy = true;
            }));
            ProcessRequest(new StockDetailRequest(Utilities.Utility.GetLink(Constants.LinkNames.MostActiveStocksLink)), LoadStockDataDetails,HandleCategoryProcessRequestError, true);
            ProcessRequest(new ProfitFlashNewsItemRequest(Utilities.Utility.GetLink(Constants.LinkNames.ProfitNewsflashLink)), LoadNewsFlashData);
            if (null != ApplicationData.BusinessCategoryItem)
            {
                ProcessRequest(new TopStoryFeedRequest(ApplicationData.BusinessCategoryItem), LoadBusinessRelatedTopNews);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Call back method.
        /// </summary>
        /// <param name="response">Response</param>
        private void LoadStockDataDetails(Response response)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                if (null != this.ActiveStockMarketDataDetails)
                {
                    if (this.ActiveStockMarketDataDetails.Count != 0)
                    {
                        this.ActiveStockMarketDataDetails.Clear();
                    }
                }
                else
                {
                    this.ActiveStockMarketDataDetails = new ObservableCollection<StockItemDetails>(); 
                }

                if (null != response)
                {
                    var parseCheck = (response as StockDetailResponse).SelectedItemStockDetails;
                    if (null != parseCheck && parseCheck.Count>0)
                    {
                        var tempStockData = (response as StockDetailResponse).SelectedItemStockDetails.ToList();
                        if (null != tempStockData && tempStockData.Count>0)
                        {
                            foreach (var stock in tempStockData)
                            {
                                if (null != this.ActiveStockMarketDataDetails)
                                {
                                    this.ActiveStockMarketDataDetails.Add(stock);
                                }
                            }
                            this.IsScorecardBusy = false;
                            this.ChosenCompanyIndex = 0;
                            ProcessRequest(new CompanySpecificStockDataRequest(this.ActiveStockMarketDataDetails[0].Id), LoadCompanySpecificData, HandleCompanySpecificProcessRequestError, true);
                        }
                    }
                    else
                    {
                        HandleError(new WebException(Resources.StockCategoriesError));
                        this.IsScorecardBusy = false;
                        this.IsScorecardDetailsPaneBusy = false;
                    }
                }

                this.StocksLoaded(this, new StocksLoadedEventArgs(this.ActiveStockMarketDataDetails.ToList<StockItemDetails>()));
            }));

        }

        /// <summary>
        /// Function invoked when the Active Category Changes.
        /// </summary>
        /// <param name="SelectedCategory">Current Selected Category</param>
        private void OnActiveCategoriesChanged(object SelectedCategory)
        {
            if (null == SelectedCategory)
            {
                return;
            }

            this.currentCategoryPageNumber = 1;
            this.canFetchMoreData = true;

            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                this.IsScorecardBusy = true;
                this.IsScorecardDetailsPaneBusy = true;
            }));


            switch (SelectedCategory.ToString())
            {
                case Constants.Constant.MostActiveStocks:
                    currentLinkName = Utilities.Utility.GetLink(Constants.LinkNames.MostActiveStocksLink);
                    ProcessRequest(new StockDetailRequest(currentLinkName,currentCategoryPageNumber), LoadStockDataDetails,HandleCategoryProcessRequestError,true);
                    break;
                case Constants.Constant.Gainers:
                    currentLinkName = Utilities.Utility.GetLink(Constants.LinkNames.MarketTopGainersLink);
                    ProcessRequest(new StockDetailRequest(currentLinkName, currentCategoryPageNumber), LoadStockDataDetails, HandleCategoryProcessRequestError, true);
                    break;
                case Constants.Constant.Losers:
                    currentLinkName = Utilities.Utility.GetLink(Constants.LinkNames.MarketTopLosersLink);
                    ProcessRequest(new StockDetailRequest(currentLinkName, currentCategoryPageNumber), LoadStockDataDetails, HandleCategoryProcessRequestError, true);
                    break;
                case Constants.Constant.YearHigh:
                    currentLinkName = Utilities.Utility.GetLink(Constants.LinkNames.YearHighLink);
                    maxNumber = 10;
                    ProcessRequest(new StockDetailRequest(currentLinkName, currentCategoryPageNumber, maxNumber), LoadStockDataDetails, HandleCategoryProcessRequestError, true);
                    break;
                case Constants.Constant.YearLow:
                    currentLinkName = Utilities.Utility.GetLink(Constants.LinkNames.YearLowLink);
                    maxNumber = 10;
                    ProcessRequest(new StockDetailRequest(currentLinkName, currentCategoryPageNumber, maxNumber), LoadStockDataDetails, HandleCategoryProcessRequestError, true);
                    break;
                case Constants.Constant.Sensex:
                    currentLinkName = Utilities.Utility.GetLink(Constants.LinkNames.SensexStocksLink);
                    maxNumber = 10;
                    ProcessRequest(new StockDetailRequest(currentLinkName, currentCategoryPageNumber, maxNumber), LoadStockDataDetails, HandleCategoryProcessRequestError, true);
                    break;
                case Constants.Constant.Nifty:
                    currentLinkName = Utilities.Utility.GetLink(Constants.LinkNames.NiftyStocksLink);
                    maxNumber = 10;
                    ProcessRequest(new StockDetailRequest(currentLinkName, currentCategoryPageNumber, maxNumber), LoadStockDataDetails, HandleCategoryProcessRequestError, true);
                    break;
                case Constants.Constant.FullDashboard:
                    currentLinkName = Utilities.Utility.GetLink(Constants.LinkNames.MarketFullDashboardLink);
                    maxNumber = 5;
                    ProcessRequest(new StockDetailRequest(currentLinkName,currentCategoryPageNumber,maxNumber), LoadStockDataDetails, HandleCategoryProcessRequestError, true);
                    this.FetchDataCommand.Execute(Constants.Constant.FullDashboard);
                    break;
            }

            if (this.SelectedCompanyDetails != null)
            {
                this.SelectedCompanyDetails = null; 
            }
        }

        /// <summary>
        /// Function called when the Active Item Changes.
        /// </summary>
        /// <param name="SelectedItem">Current Selected Item</param>
        private void OnActiveItemChanged(object SelectedItem)
        {
            if (null != SelectedItem)
            {
                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    this.IsScorecardDetailsPaneBusy = true;
                }));
                ProcessRequest(new CompanySpecificStockDataRequest((SelectedItem as StockItemDetails).Id), LoadCompanySpecificData,HandleCompanySpecificProcessRequestError,true);
            }
        }

        /// <summary>
        /// Function to load the company specific data.
        /// </summary>
        private void LoadCompanySpecificData(Response response)
        {
            if (null != response)
            {
                this.SelectedCompanyDetails = (response as CompanySpecificStockDataResponse).CompanySpecificData;
                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    this.IsScorecardDetailsPaneBusy = false;
                    this.IsScorecardBusy = false;
                }));
            }
        }

        /// <summary>
        /// Loads the Flash News Response.
        /// </summary>
        /// <param name="response">Response</param>
        private void LoadNewsFlashData(Response response)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                this.profitNewsFlashItem = (response as ProfitFlashNewsItemResponse).NewsflashItem;
                if (null != response)
                {
                    int count = 0;
                    while (count++ < Utility.GetNumberOfItemsToRetrieve("FlashNewsItemsCount"))
                    {
                        if (null != this.NewsflashItems)
                        {
                            this.NewsflashItems.Add(this.profitNewsFlashItem.ItemDetails[count].FlashItemDetails);
                        }
                    }
                }

            }));
        }

        /// <summary>
        /// Function to handle any errors in the process request associated with the Category change.
        /// </summary>
        /// <param name="request">The appropriate request</param>
        private void HandleCategoryProcessRequestError(Request request)
        {
            this.IsScorecardBusy = false;
            HandleError(new WebException(Resources.StockCategoriesError));
        }

        /// <summary>
        /// Function to handle any errors in the process request associated with the company change.
        /// </summary>
        /// <param name="request">The appropriate request</param>
        private void HandleCompanySpecificProcessRequestError(Request request)
        {
            this.IsScorecardDetailsPaneBusy = false;
            HandleError(new WebException(Resources.StockItemError));
        }

        /// <summary>
        /// Call back method called when the Response is received.
        /// </summary>
        /// <param name="response">Response</param>
        private void LoadBusinessRelatedTopNews(Response response)
        {
            if (null != response)
            {
                this.BusinessTopStoryItem = (response as TopStoryFeedResponse).TopStory;

                if (null != this.BusinessTopStoryItem)
                {
                    App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                     {
                         foreach (var item in this.BusinessTopStoryItem.TopStoryCollection)
                         {
                             if (null != this.RelatedBusinessNews)
                             {
                                 this.RelatedBusinessNews.Add(item);
                             }
                         }
                     }));
                }
            }
        }

        /// <summary>
        /// Method that makes a service request to fetch the next bunch of details.
        /// </summary>
        /// <param name="selectedCategory">Selected Category</param>
        private void FetchNextData(object selectedCategory)
        {
            if (canFetchMoreData)
            {
                if (null != selectedCategory)
                {
                    currentCategoryPageNumber += 1;
                    switch (selectedCategory.ToString())
                    {
                        case Constants.Constant.MostActiveStocks:
                        case Constants.Constant.Gainers:
                        case Constants.Constant.Losers:
                            ProcessRequest(new StockDetailRequest(currentLinkName, currentCategoryPageNumber), LoadAdditionalData);
                            return;
                        default:
                            ProcessRequest(new StockDetailRequest(currentLinkName, currentCategoryPageNumber, maxNumber), LoadAdditionalData);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Appropriate response which loads the additional data.
        /// </summary>
        /// <param name="response">The appropriate response</param>
        private void LoadAdditionalData(Response response)
        {
            if (null != response)
            {
                var additionalData = (response as StockDetailResponse).SelectedItemStockDetails;
                if (null != additionalData && additionalData.Count>0)
                {
                    App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                    {
                        var listData = additionalData.ToList();
                        if (null != listData && listData.Count > 0)
                        {
                            foreach (var item in listData)
                            {
                                if (null != this.ActiveStockMarketDataDetails)
                                {
                                    this.ActiveStockMarketDataDetails.Add(item);
                                }
                            }
                        }
                    }));
                }
                else
                {
                    this.canFetchMoreData = false;
                }
            }
        }

        /// <summary>
        /// Dispose method.
        /// </summary>
        protected override void DisposeResources()
        {
            this.SelectedCompanyDetails = null;
            this.StockTickerDetails = null;
            this.ActiveStockMarketDataDetails = null;
            this.onActiveCategoriesChangedCommand = null;
            this.onActiveItemChangedCommand = null;
            this.fetchDataCommand = null;
            this.RelatedBusinessNews = null;
        }

        #endregion
    }
}
