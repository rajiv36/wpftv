using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Timers;
using System.Windows;
using System.Windows.Media.Animation;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.ViewModel;
using NDTV.Utilities;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for SensexScorecard.xaml
    /// </summary>
    public partial class SensexScorecard : SlateWindow
    {

        /// <summary>
        /// Private variable.
        /// </summary>
        private int count;

        /// <summary>
        /// Index of the previously selected category.
        /// </summary>
        private int previousCategorySelectedIndex;

        /// <summary>
        /// Index of the previously selected company.
        /// </summary>
        private int previousCompanySelectedIndex;

        /// <summary>
        /// View Model Instance
        /// </summary>
        private StockViewModel sensexViewer;

        /// <summary>
        /// The scroll timer
        /// </summary>
        private Timer scrollTimer;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stockTickerDetails">Data associated with the Stock Ticker.</param>
        public SensexScorecard(ObservableCollection<StockIndexItem> stockTickerDetails)
        {
            InitializeComponent();
            sensexViewer = new StockViewModel(stockTickerDetails);

            this.LayoutRoot.Loaded += (s, args) =>
            {
                this.DataContext = sensexViewer;
                this.LandscapeLocalAdvertisement.CurrentPage = Pages.SensexScorecardSmall;
            };
            this.LandscapeSensexScoreCard.CloseButtonClicked += (s, args) =>
            {
                this.Dispose();
                sensexViewer.Dispose();
                this.Close();
            };

            this.PotraitSensexScoreCard.CloseButtonClicked += (s, args) =>
            {
                this.Dispose();
                sensexViewer.Dispose();
                this.Close();
            };

            sensexViewer.StocksLoaded += (s, e) =>
            {
                if (null != e.Stocks && e.Stocks.Count > 0)
                {
                    this.LandscapeStockDisplayListBox.ScrollIntoView(e.Stocks[0]);
                }
            };
            this.LandscapeStockDisplayListBox.SelectionChanged += (s, args) =>
            {
                if (ApplicationData.IsApplicationOnline)
                {
                    this.LandscapeStockDisplayListBox.ScrollIntoView(this.LandscapeStockDisplayListBox.SelectedItem);
                    sensexViewer.OnActiveItemChangedCommand.Execute(this.LandscapeStockDisplayListBox.SelectedItem);
                    previousCompanySelectedIndex = sensexViewer.ChosenCompanyIndex;
                }
                else
                {
                    this.LandscapeStockDisplayListBox.SelectedIndex = previousCompanySelectedIndex;
                }
            };

            this.PotraitStockDisplayListBox.SelectionChanged += (s, args) =>
            {
                if (ApplicationData.IsApplicationOnline)
                {
                    this.PotraitStockDisplayListBox.ScrollIntoView(this.PotraitStockDisplayListBox.SelectedItem);
                }
                else
                {
                    this.PotraitStockDisplayListBox.SelectedIndex = previousCompanySelectedIndex;
                }
            };

            this.LandscapeCategoryChoser.SelectionChanged += (s, args) =>
            {
                if (ApplicationData.IsApplicationOnline)
                {
                    this.LandscapeLocalAdvertisement.RefreshAdBanner();
                    sensexViewer.OnActiveCategoriesChangedCommand.Execute(this.LandscapeCategoryChoser.SelectedItem);
                }
                else
                {
                    this.LandscapeCategoryChoser.SelectedIndex = previousCategorySelectedIndex;
                }
                previousCategorySelectedIndex = this.LandscapeCategoryChoser.SelectedIndex;
            };

            this.PotraitCategoryChoser.SelectionChanged += (s, args) =>
            {
                if (ApplicationData.IsApplicationOnline)
                {
                    this.PortraitLocalAdvertisement.RefreshAdBanner();
                    this.PotraitCategoryChoser.ScrollIntoView(this.PotraitCategoryChoser.SelectedItem);
                }
                else
                {
                    this.PotraitCategoryChoser.SelectedIndex = previousCategorySelectedIndex;
                }
                previousCategorySelectedIndex = this.PotraitCategoryChoser.SelectedIndex;
            };

            int duration = int.Parse(Utilities.Utility.GetTimerInterval("SwapTimerInterval"), CultureInfo.InvariantCulture);
            scrollTimer = new Timer(duration);
            scrollTimer.Elapsed += new ElapsedEventHandler(OnScrollTimerElapsed);
            scrollTimer.Start();
        }

        /// <summary>
        /// Overriding the method where in the grids are Visibility controlled here..
        /// Potriat Container is hidden and Landscape Container is brought to focus
        /// </summary>
        protected override void SwitchToLandscapeView()
        {
            LandscapeView();
            this.SetWindowPosition();
        }

        /// <summary>
        ///  Overriding the method where in the grids are Visibility controlled here..
        ///  Landscape Container is hidden and Potrait Container is brought to focus
        /// </summary>
        protected override void SwitchToPortraitView()
        {
            PortraitView();
            this.SetWindowPosition();
        }

        /// <summary>
        /// The overriden method which sets the size of the window based on whether it is in landscape or potriat version
        /// </summary>
        protected override void SetSize()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                this.SetWindowPosition();
                LandscapeView();
            }
            else
            {
                this.SetWindowPosition();
                PortraitView();
            }
        }

        /// <summary>
        /// A Method which sets the window to Landscape orientation with all the required properties
        /// </summary>
        private void LandscapeView()
        {
            this.Height = 550;
            this.Width = 950;
            if (null != PotraitSensexScoreCard)
            {
                PotraitSensexScoreCard.Visibility = Visibility.Hidden;
            }
            if (null != LandscapeSensexScoreCard)
            {
                if (null != this.PotraitCategoryChoser.SelectedItem && null != this.PotraitStockDisplayListBox.SelectedItem)
                {
                    this.LandscapeCategoryChoser.ScrollIntoView(this.PotraitCategoryChoser.SelectedItem);
                    this.LandscapeStockDisplayListBox.ScrollIntoView(this.PotraitStockDisplayListBox.SelectedItem);
                }
                LandscapeSensexScoreCard.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// A Method which sets the window to Potrait orientation with all the required properties
        /// </summary>
        private void PortraitView()
        {
            this.Height = 950;
            this.Width = 550;
            if (null != LandscapeSensexScoreCard)
            {
                LandscapeSensexScoreCard.Visibility = Visibility.Hidden;
            }
            if (null != PotraitSensexScoreCard)
            {
                if (null != this.LandscapeCategoryChoser.SelectedItem && null != this.LandscapeStockDisplayListBox.SelectedItem)
                {
                    this.PotraitCategoryChoser.ScrollIntoView(this.LandscapeCategoryChoser.SelectedItem);
                    this.PotraitStockDisplayListBox.ScrollIntoView(this.LandscapeStockDisplayListBox.SelectedItem);
                }
                PotraitSensexScoreCard.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Event handler which performs manipulations once the Scroll Timer elapses.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void OnScrollTimerElapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                if (ApplicationData.IsLandscapeOrientation)
                {
                    Storyboard fadeOutStoryBoard = (Storyboard)TryFindResource("LandscapeAnimateNewsControl");
                    fadeOutStoryBoard.Begin();
                    if (count++ < Utility.GetNumberOfItemsToRetrieve("FlashNewsItemsCount"))
                    {
                        this.LandscapeNewsFlashItemsScrollViewer.ScrollToVerticalOffset(this.LandscapeNewsFlashItemsScrollViewer.VerticalOffset + 30);
                    }
                    else
                    {
                        this.LandscapeFlashNewsItemsControl.Visibility = Visibility.Collapsed;
                        this.LandscapeNewsFlashItemsScrollViewer.ScrollToVerticalOffset(0);
                        this.LandscapeFlashNewsItemsControl.Visibility = Visibility.Visible;
                        count = 0;
                    }
                }
                else
                {
                    Storyboard fadeOutStoryBoard = (Storyboard)TryFindResource("PotraitAnimateNewsControl");
                    fadeOutStoryBoard.Begin();
                    if (count++ < Utility.GetNumberOfItemsToRetrieve("FlashNewsItemsCount"))
                    {
                        this.PotraitNewsFlashItemsScrollViewer.ScrollToVerticalOffset(this.PotraitNewsFlashItemsScrollViewer.VerticalOffset + 30);
                    }
                    else
                    {
                        this.PotraitFlashNewsItemsControl.Visibility = System.Windows.Visibility.Collapsed;
                        this.PotraitNewsFlashItemsScrollViewer.ScrollToVerticalOffset(0);
                        this.PotraitFlashNewsItemsControl.Visibility = System.Windows.Visibility.Visible;
                        count = 0;
                    }
                }
            }));
        }

        /// <summary>
        /// Sets the window position calculating the resolution of the window.
        /// </summary>
        private void SetWindowPosition()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            Left = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
            Top = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;
        }

        /// <summary>
        /// Event handler to display the business related articles on the article page.
        /// </summary>
        private void OnBusinessArticleClick(object sender, RoutedEventArgs e)
        {
            if (null != sensexViewer.BusinessTopStoryItem && null != sender)
            {
                OpenPopupWindow(new Article((Entities.TopStoryItem)(((FrameworkElement)sender).DataContext), sensexViewer.BusinessTopStoryItem), this, ModalBG,null);
            }
        }

        /// <summary>
        /// Event handler which fires a command to obtain more data once the scroll viewer reaches the end of the Landscape ListBox.
        /// </summary>
        /// <param name="sender">Landscape Surface List Box</param>
        /// <param name="e">Event args</param>
        private void OnLandscapeStockDisplayListBoxScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var activeListBoxScrollViewer = SensexScorecard.FindScroll((sender as SurfaceListBox));
            if (null != activeListBoxScrollViewer)
            {
                if (e.VerticalOffset == activeListBoxScrollViewer.ScrollableHeight && e.VerticalOffset != 0)
                {
                    if (ApplicationData.IsApplicationOnline)
                    {
                        this.sensexViewer.FetchDataCommand.Execute(this.LandscapeCategoryChoser.SelectedItem);
                    }
                }
            }
        }

        /// <summary>
        /// Event handler which fires a command to obtain more data once the scroll viewer reaches the eSnd of the Portrait ListBox.
        /// </summary>
        /// <param name="sender">Portrait Surface List Box</param>
        /// <param name="e">Event args</param>
        private void OnPotraitStockDisplayListBoxScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var activeListBoxScrollViewer = SensexScorecard.FindScroll((sender as SurfaceListBox));
            if (null != activeListBoxScrollViewer)
            {
                if (e.VerticalOffset == activeListBoxScrollViewer.ScrollableHeight && e.VerticalOffset != 0)
                {
                    if (ApplicationData.IsApplicationOnline)
                    {
                        this.sensexViewer.FetchDataCommand.Execute(this.PotraitCategoryChoser.SelectedItem);
                    }
                }
            }
        }

        /// <summary>
        /// Method to extract the Scroll Viewer from within the ListBox.
        /// </summary>
        /// <param name="activeListBox">The listbox from which the ScrollViewer has to be extracted</param>
        /// <returns>The ScrollViewer (if found)</returns>
        private static SurfaceScrollViewer FindScroll(SurfaceListBox activeListBox)
        {
            Grid scrollGrid = VisualTreeHelper.GetChild(activeListBox, 0) as Grid;
            if (scrollGrid is Grid)
            {
                SurfaceScrollViewer activeScrollViewer = scrollGrid.Children[0] as SurfaceScrollViewer;
                if (activeScrollViewer is SurfaceScrollViewer)
                {
                    return activeScrollViewer;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        protected override void DisposeResources()
        {
            base.DisposeResources();
            scrollTimer.Elapsed -= new ElapsedEventHandler(OnScrollTimerElapsed);
            if (null != scrollTimer)
            {
                scrollTimer.Stop();
                scrollTimer = null;
            }
        }

    }
}
