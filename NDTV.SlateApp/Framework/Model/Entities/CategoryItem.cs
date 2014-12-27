using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NDTV.Entities
{
    /// <summary>
    /// every property required for the category class would be mentioned here..
    /// all the properties bound in the ui should be present in this Category class hierarchically..
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Constructor which allocates the resources for its properties
        /// </summary>
        public Category()
        {
            categoryItemList = new SortedList<int, CategoryItem>();
        }

        internal SortedList<int, CategoryItem> categoryItemList;
        /// <summary>
        /// Gets the category item list
        /// </summary>
        public List<CategoryItem> CategoryItemList
        {
            get
            {
                return categoryItemList.Values.ToList();
            }
        }

        /// <summary>
        /// The method which is exposed to add a new category from the ViewModel(Ex: Photo Gallery & Image Gallery)..
        /// </summary>
        /// <param name="newCategoryName"></param>
        internal void AddCategory(string newCategoryName)
        {
            CategoryItem itemToBeAdded = new CategoryItem()
            {
                Title = newCategoryName,
                IsNDTVSourceCategory = false,
                Link = string.Empty,
                Priority = categoryItemList.Count
            };
            if (null != categoryItemList && categoryItemList.Count > 0)
            {
                categoryItemList.Add(categoryItemList.Max(item => item.Key) + 1, itemToBeAdded);
            }
        }
    }

    /// <summary>
    /// Category Item Collection which represents the item array which can be directly bound to any collection accepting control
    /// </summary>
    public class CategoryItemCollection : System.Collections.ObjectModel.Collection<CategoryItem>
    {

    }

    /// <summary>
    /// This class holds a category item
    /// </summary>
    public class CategoryItem
    {
        /// <summary>
        /// Category title
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Flag which says whether it is an NDTV Source or not..
        /// </summary>
        public bool IsNDTVSourceCategory
        {
            get;
            set;
        }

        /// <summary>
        /// Category URL
        /// </summary>
        public string Link
        {
            get;
            set;
        }

        /// <summary>
        /// Category priority
        /// </summary>
        public int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// The Local Image which gets bound to the Category Context menu
        /// </summary>
        public string ThumbNailImageLink
        {
            get
            {
                return
                    System.IO.File.Exists(Utilities.Utility.FetchImagePath() + Constants.Constant.FilePathSeparator + Title + Constants.Constant.PngExtension) ?
                    Utilities.Utility.FetchImagePath() + Constants.Constant.FilePathSeparator + Title + Constants.Constant.PngExtension :
                    Utilities.Utility.FetchImagePath() + Constants.Constant.FilePathSeparator + Constants.TopStories.Categories.India + Constants.Constant.PngExtension;
            }
        }
    }
}
