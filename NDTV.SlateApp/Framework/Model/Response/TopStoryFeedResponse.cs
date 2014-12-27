namespace NDTV.Entities
{
    public class TopStoryFeedResponse : Response
    {
        public  TopStory TopStory { get; set; }

        public TopStoryFeedResponse(string responseMessage):base(responseMessage)
        {
            TopStory = new TopStory();
            TopStory.BuildGraphObject(base.responseMessage);
        }
    }
}
