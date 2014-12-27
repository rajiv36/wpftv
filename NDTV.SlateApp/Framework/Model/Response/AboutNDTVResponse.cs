using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class AboutNDTVResponse :Response
    {
         /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response message to be parsed</param>
        public AboutNDTVResponse(string responseMessage)
            : base(responseMessage)
        {
            Parse();
        }

        /// <summary>
        /// About Ndtv text.
        /// </summary>
        public string AboutNdtvText
        {
            get;
            private set;
        }

        /// <summary>
        /// This method parses the response and creates the entities
        /// </summary>
        private void Parse()
        {
            XElement element = XElement.Parse(responseMessage);

            if (null != element.Element("channel") && null != element.Element("channel").Element("content"))
            {
                AboutNdtvText = element.Element("channel").Element("content").Value;
            }
            else
            {
                AboutNdtvText = Constants.StaticText.AboutNdtvDefaultText;
            }
        
        }

    }
}
