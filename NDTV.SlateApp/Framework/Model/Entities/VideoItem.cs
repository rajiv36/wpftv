namespace NDTV.Entities
{
    /// <summary>
    /// Properties to store video item
    /// </summary>
    public class VideoItem : Item
    {
        #region Properties
        /// <summary>
        /// Title of the video
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Link of the video related to NDTV Video Player
        /// </summary>
        public string VideoLink
        {
            get;
            set;
        }

        /// <summary>
        /// Description of the video
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Filepath of the video
        /// </summary>
        public string VideoFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the video
        /// </summary>
        public int VideoId
        {
            get;
            set;
        }

        /// <summary>
        /// Link for thumbnail of the video
        /// </summary>
        public string ThumbnailLink
        {
            get;
            set;
        }

        /// <summary>
        /// Link for thumbnail of the video(Large Size)
        /// </summary>
        public string ThumbnailLinkLarge
        {
            get;
            set;
        }

        /// <summary>
        /// Publised Date of the Video
        /// </summary>
        public string PublishDate
        {
            get;
            set;
        }

        /// <summary>
        /// Duration of the Video
        /// </summary>
        public string Duration
        {
            get;
            set;
        }

        #endregion Properties
    }
}
