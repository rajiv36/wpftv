using System.Collections.Generic;

namespace NDTV.Entities
{
    /// <summary>
    /// News Flash Item Entity
    /// </summary>
    public class ProfitFlashNewsItem
    {
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Detailed description
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Language(English,Spanish etc)
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Concerned details associated with each item
        /// </summary>
        public List<ProfitFlashNewsItemDetails> ItemDetails { get; set; }
    }

    
}
