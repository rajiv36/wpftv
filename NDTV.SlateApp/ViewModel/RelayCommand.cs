using System;
using System.Windows.Input;

namespace NDTV.SlateApp.ViewModel
{   

    /// <summary>
    /// This class is the base class that implements the ICommand interface.
    /// 2 methods of the ICommand are implemented as Abstract so that they are manadated to be overriden in derived class.
    /// </summary>
    public class RelayCommand : ICommand
    {

        private Action<object> executeAction;

        private Predicate<object> canExecutePredicate;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executeAction">Method which is to be called while executing the command</param>
        public RelayCommand(Action<object> executeAction)
        {
            this.executeAction = executeAction;
            this.canExecutePredicate = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executeAction">Method which is to be called while executing the command</param>
        /// <param name="canExecutePredicate">Method which is to be caleed whether the execute can be called</param>
        public RelayCommand(Action<object> executeAction, Predicate<object> canExecutePredicate)
        {
            this.executeAction = executeAction;
            this.canExecutePredicate = canExecutePredicate;
        }

        /// <summary>
        /// This checks whether the command can be executed or nor
        /// </summary>
        /// <param name="parameter">any command parameter</param>
        /// <returns>True / false if the command can be executed</returns>
        public bool CanExecute(object parameter)
        {
            return (null != canExecutePredicate) ? canExecutePredicate(parameter) : true;            
        }


        /// <summary>
        /// This is the handler for the binding system to refresh the CanExecute value.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add 
            { 
                CommandManager.RequerySuggested += value; 
            }
            remove 
            { 
                CommandManager.RequerySuggested -= value;
            }
        }
        

        /// <summary>
        /// This method executes the command
        /// </summary>
        /// <param name="parameter">any command parameter</param>
        public void Execute(object parameter)
        {
            if (null != executeAction)
            {
                executeAction(parameter);
            }
        }        
    }
}
