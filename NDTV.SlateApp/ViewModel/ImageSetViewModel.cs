using System.Collections.ObjectModel;
using NDTV.Controller;
using NDTV.Entities;
using System.Globalization;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// Class ViewModel for Image Album Carousel. 
    /// </summary>
    public class ImageSetViewModel : ViewModelBase
    {
        /// <summary>
        /// Categories available.
        /// </summary>
        private ObservableCollection<ImageCategoryItem> categories;

        /// <summary>
        /// Categories available.
        /// </summary>
        public ObservableCollection<ImageCategoryItem> Categories
        {
            get 
            {
                return categories; 
            }
            private set 
            { 
                categories = value;
                OnPropertyChanged("Categories");
            }
        }

        /// <summary>
        /// Collection of Albums.
        /// </summary>
        private ObservableCollection<ImageAlbum> imageAlbums;

        /// <summary>
        /// Collection of Albums.
        /// </summary>
        public ObservableCollection<ImageAlbum> ImageAlbums
        {
            get 
            { 
                return imageAlbums;
            }
            private set
            { 
                imageAlbums = value;
                OnPropertyChanged("ImageAlbums");
            }
        }
     
        /// <summary>
        /// Flag which is TRUE when the request is processed completely.And the imageAlbums are loaded.
        /// </summary>
        private bool isLoadingComplete;

        /// <summary>
        /// Flag which is TRUE when the request is processed completely.And the imageAlbums are loaded.
        /// </summary>
        public bool IsLoadingComplete
        {
            get 
            { 
                return isLoadingComplete;
            }
            set
            { 
                isLoadingComplete = value;
                OnPropertyChanged("IsLoadingComplete");
            }
        }

        /// <summary>
        /// Initializes a new instance of the ImageSetViewModel class. Constructor.
        /// </summary>
        public ImageSetViewModel(ImageCategoryItem item)
        {
            if (null != item)
            {
                ImageSetViewModelReload(item);
                this.Categories = new ObservableCollection<ImageCategoryItem>();

                if (null == ApplicationData.Settings.ImageAlbumCarouselCategory)
                {
                    bool checkFirstCategory = false;
                    foreach (ImageCategoryItem category in ApplicationData.ImagesCategoryList)
                    {
                        if (false == checkFirstCategory)
                        {
                            category.IsCheckedProperty = true;
                            this.Categories.Add(category);
                            checkFirstCategory = true;
                        }
                        else
                        {
                            this.Categories.Add(category);
                        }
                    }
                }
                else
                {
                    foreach (ImageCategoryItem category in ApplicationData.ImagesCategoryList)
                    {

                        if (ApplicationData.Settings.ImageAlbumCarouselCategory == category.ImageAlbumTitle)
                        {
                            category.IsCheckedProperty = true;
                            this.Categories.Add(category);
                        }
                        else
                        {
                            this.Categories.Add(category);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Request for getting the albums of the selected category.
        /// </summary>
        private ImageAlbumRequest request = null;

        public void ImageSetViewModelReload(ImageCategoryItem item)
        {
            if (null != item)
            {
                request = null;
                this.IsLoadingComplete = false;
                request = new ImageAlbumRequest(item.ImageAlbumLink,true);
                ProcessRequest(request, LoadResponse);
            }
        }

        /// <summary>
        /// Loads the content after it has been retrieved
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="eventArgs">Controller event args</param>
        private void LoadResponse(Response imageAlbumRequest)
        {
            if (imageAlbumRequest.GetType() == typeof(ImageAlbumResponse))
            {
                this.ImageAlbums = new ObservableCollection<ImageAlbum>(((ImageAlbumResponse)imageAlbumRequest).ImageAlbumCollection);               
            }
            this.IsLoadingComplete = true;
        }

        /// <summary>
        /// Disposes all the objects in the view model
        /// </summary>
        protected override void DisposeResources()
        {
            this.Categories = null;
            this.imageAlbums = null;
        }

    }
}
