using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class LinkedInUserDataResponse : Response
    {
        /// <summary>
        /// Gets the linkedIn user object.
        /// </summary>
        private LinkedInUser linkedInUser;
        public LinkedInUser LinkedInUser
        {
            get
            {
                return this.linkedInUser;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseString">Response string</param>
        public LinkedInUserDataResponse(string responseString)
            : base(responseString)
        {
            this.Parse();
        }

        /// <summary>
        /// Parses the response to obtain data.
        /// </summary>
        private void Parse()
        {
            XElement responseElement;

            if (Utility.XElementTryParse(responseMessage, out responseElement))
            {
                linkedInUser = new LinkedInUser()
                {
                    FirstName = (null != responseElement.Element("first-name")) ? responseElement.Element("first-name").Value : string.Empty,
                    LastName = (null != responseElement.Element("last-name")) ? responseElement.Element("last-name").Value : string.Empty,
                    Headline = (null != responseElement.Element("headline")) ? responseElement.Element("headline").Value : string.Empty,
                    ProfileLink = (null != responseElement.Element("site-standard-profile-request"))
                                    ? (null != responseElement.Element("site-standard-profile-request").Element("url")
                                        ? responseElement.Element("site-standard-profile-request").Element("url").Value
                                        : string.Empty
                                    ) : string.Empty
                };
            }
            else
            {
                linkedInUser = null;
            }
        }

        /// <summary>
        /// This deserializes the xml into the object
        /// </summary>
        /// <typeparam name="T">Type of object to which the xml is to be deserialized to</typeparam>
        /// <param name="xmlString">Xml string</param>
        /// <returns>The instance of object</returns>
        private static T DeserializeFromXml<T>(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream memoryReader = new MemoryStream(Encoding.Unicode.GetBytes(xmlString)))
            {
                var resultObject = (T)serializer.Deserialize(memoryReader);
                memoryReader.Close();
                return resultObject;
            }
        }
    }
}
