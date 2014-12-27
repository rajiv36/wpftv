
namespace NDTV.Entities
{
    /// <summary>
    /// Request for the Channel Schedule
    /// </summary>
    public class ChannelScheduleRequest:Request
    {
      /// <summary>
      /// Constructor that raises the Get request of the schedule link
      /// </summary>
      /// <param name="link">Schedule Link</param>
        public ChannelScheduleRequest(string link): base(Constants.Constant.Schedule, link, HttpOperation.Get)
        {

        }
        /// <summary>
        /// Call back method of the Response 
        /// </summary>
        /// <param name="responseMessage">Response message</param>
        /// <returns> Channel Schedule Response</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            return new ChannelScheduleResponse(responseString);
        }
    }
}
