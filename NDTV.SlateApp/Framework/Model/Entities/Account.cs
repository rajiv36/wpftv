using System;

namespace NDTV.Entities
{
    public abstract class Account
    {               
        /// <summary>
        /// Event for completion of authentication.
        /// </summary>
        public abstract event EventHandler<EventArgs> AuthenticationCompleted;        

        #region Account Properties
       
        /// <summary>
        /// Gets the consumer Key.
        /// </summary>
        public abstract string ConsumerKey
        {
            get;
        }

        /// <summary>
        /// Gets the consumer secret.
        /// </summary>
        public abstract string ConsumerSecret
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether logged or not.
        /// </summary>
        public abstract bool IsLogged
        {
            get;
        }

        /// <summary>
        /// Gets the access token retrieved from successfull authentication.
        /// </summary>
        public abstract string AccessToken
        {
            get;
        }
        #endregion

        #region Account Members

        /// <summary>
        /// To Authenticate the User.
        /// </summary>
        public abstract void Authenticate();
        #endregion
    }
}
