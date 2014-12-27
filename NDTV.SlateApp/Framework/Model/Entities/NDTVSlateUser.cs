
namespace NDTV.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class NDTVSlateUser
    {
        /// <summary>
        /// The NDTV user id.
        /// </summary>
        [DataMember(Name = "uid")]
        public string Id
        {
            get;
            set;
        }
    }
}
