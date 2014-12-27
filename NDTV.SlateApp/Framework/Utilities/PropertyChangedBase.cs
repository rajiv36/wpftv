namespace NewsDesk.Framework
{
    using System.Collections.Specialized;
    using System.ComponentModel;

    /// <summary>
    /// This is the base class to be used by all viewModels that want the 
    /// property or collection change notifications to be raised for the binding system.
    /// </summary>
    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// This even is handled by the binding system to refresh the data when the value is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This is the method that should be called by derived classes to raise the proerpty change event notification 
        /// so that the binding systems refreshes the data on the UI.
        /// </summary>
        /// <param name="propertyName">propertyName that is changed / updated</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs arguments = new PropertyChangedEventArgs(propertyName);
                handler(this, arguments);
            }
        }


        #endregion      
    }
}