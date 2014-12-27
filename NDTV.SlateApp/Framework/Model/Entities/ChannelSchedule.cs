
namespace NDTV.Entities
{
   /// <summary>
    /// This class holds the current and upcoming program from the API.
    /// </summary>
   public class ChannelSchedule
    {
       
       /// <summary>
       /// Current program Time
       /// </summary>
       public string ScheduleTime
      {
          get ;
          set ;
      }
       
       /// <summary>
       /// Current program Description
       /// </summary>
       public string Description
       {
           get;
           set;

       }
      
       /// <summary>
       /// Current program description
       /// </summary>
       public string CurrentShow
       {
           get;
           set;

       }
       
       /// <summary>
       /// Upcoming program
       /// </summary>
       public string UpcomingShow
       {
           get;
           set;
           

       }
    }
 
   
}
