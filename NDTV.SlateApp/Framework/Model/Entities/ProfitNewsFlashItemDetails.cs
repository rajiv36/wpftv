
namespace NDTV.Entities
{
    /// <summary>
    /// Details associated with a news flash item
    /// </summary>
    public class ProfitFlashNewsItemDetails
    {
        /// <summary>
        /// Concerned details.
        /// </summary>
        public string FlashItemDetails { get; set; }

        /// <summary>
        /// The associated Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Date associated with the News flash Item.
        /// </summary>
        public string Date { get; set; }
    }
}
