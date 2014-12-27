
namespace NDTV.Entities
{
    /// <summary>
    ///  LinkedIn user data.
    /// </summary>
    public class LinkedInUser
    {
        /// <summary>
        /// First name.
        /// </summary>
        public string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Last name.
        /// </summary>
        public string LastName
        {
            get;
            set;
        }

        /// <summary>
        /// Profile headline.
        /// </summary>
        public string Headline
        {
            get;
            set;
        }

        /// <summary>
        /// Profile public link.
        /// </summary>
        public string ProfileLink
        {
            get;
            set;
        }

    }
}

// TODO: Remove following snippet of user profile received from linked In, 
//         Once the entities are finanlized. (Following is the repsonse for user profile request)
//  <person>
//  <first-name>Prashant</first-name> 
//  <last-name>Soni</last-name> 
//  <headline>Senior Development Engineer at Aditi Technologies</headline> 
//- <site-standard-profile-request>
//  <url>http://www.linkedin.com/profile?viewProfile=&key=53321717&authToken=ZI1_&authType=name&trk=api*a143975*s152316*</url> 
//  </site-standard-profile-request>
//  </person>

