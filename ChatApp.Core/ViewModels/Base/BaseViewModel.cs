using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core
{
    /// <summary>
    /// A base view model that fire the Property Changed event
    /// </summary>

    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The event that is fire when any property when a property changes value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires the Property Changed event to force the UI to update
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Command Helpers

        /// <summary>
        /// Runs a command if the updating flag is not set
        /// If the flag is true (indicating the function is already running) then the action is not run
        /// If the flag is false (indicating the function is NOT already running) then the action is run
        /// Once the action is finished, if it was run, the flag is reset to false
        /// </summary>
        /// <param name="updatingFlag">The boolean property flag definining if the command is already running</param>
        /// <param name="action">The action to run if the action is not already running</param>
        /// <returns></returns>
        protected async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            // Check if the flag property is true (indicating the function is already running)
            if (updatingFlag.GetPropertyValue())
                return;

            // Set the property flag to true to indicate we are running
            updatingFlag.SetPropertyValue(true);

            try
            {
                // Run the passed in action
                await action();
            }
            finally
            {
                // Set the property flag back to flase now that it has finished
                updatingFlag.SetPropertyValue(false);
            }

        }

        #endregion

    }
}
