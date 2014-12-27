using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Channel Schedule Response class
    /// </summary>
   public class ChannelScheduleResponse:Response
    {
        #region Properties
        /// <summary>
      /// Schedule Details 
      /// </summary>
        public ChannelSchedule ChannelScheduleDetails
        { get; set; }
        #endregion Properties

        /// <summary>
        /// Constructor gets the response message
        /// </summary>
        /// <param name="responseMessage">Schedule Response Message</param>
       public ChannelScheduleResponse(string responseMessage)  : base(responseMessage)
       {
         Parse();
       }

       #region Private methods
       /// <summary>
        /// Parses the response message
        /// </summary>
       private void Parse()
       {
           DateTime currentProgramTime = DateTime.Today.AddMinutes(0);
           DateTime upcomingProgramTime = DateTime.Today;
           bool setCurrentProgram=false;
           bool setTime=false;
           Double parsedTime=00;
           string programTime = string.Empty;
           string currentProgram = string.Empty;
           string comingUp=string.Empty;
           XElement xmlNode = new XElement(XElement.Parse(base.responseMessage).Element("Schedule"));
           IEnumerable<ChannelSchedule> items = (from eachItem in xmlNode.Elements("program")
                                              select new ChannelSchedule()
                                              {
                                                   Description = (null!= eachItem.Element("desc"))? Helper.RemoveHtmlTags(eachItem.Element("desc").Value): string.Empty,
                                                   CurrentShow= (null!= eachItem.Element("name"))?eachItem.Element("name").Value:string.Empty,
                                                   ScheduleTime = (null != eachItem.Element("timestamp")) ? eachItem.Element("timestamp").Value : string.Empty
                                              });

           foreach (ChannelSchedule item in items)
           {
               programTime = item.ScheduleTime;
               setTime = Double.TryParse(string.IsNullOrEmpty(programTime) ? "00": programTime.Substring(0,2) ,out parsedTime);
               upcomingProgramTime = DateTime.Today.AddHours(setTime ?parsedTime: 0);
               setTime = Double.TryParse(string.IsNullOrEmpty(programTime) ? "00" : programTime.Substring(3), out parsedTime);
               upcomingProgramTime = upcomingProgramTime.AddMinutes(setTime ? parsedTime:0);
               
               if (DateTime.Now >= currentProgramTime && DateTime.Now < upcomingProgramTime)
               {
                   if (item.CurrentShow == currentProgram && upcomingProgramTime != currentProgramTime)
                   {
                       continue;
                   }
                   comingUp = item.CurrentShow;
                   setCurrentProgram = true;
                   item.UpcomingShow = comingUp;
                   item.CurrentShow = currentProgram;
                   item.Description = item.Description;
                   ChannelScheduleDetails = item;
                   break;
               }


                currentProgramTime = upcomingProgramTime;
                currentProgram = item.CurrentShow;
                            
           }

           if (false==setCurrentProgram)
           {
               ChannelScheduleDetails = items.LastOrDefault();
           }

       }
       #endregion Private methods
    }
}
