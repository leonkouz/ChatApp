using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatApp
{
    public class DelegateParametrisedCommand : ICommand
    {
        /// <summary>
        /// The action to run
        /// </summary>
        private readonly Action<object> _action;

        public DelegateParametrisedCommand(Action<object> action)
        {
            _action = action;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _action(parameter);
        }

        /// <summary>
        /// A comman can always execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// The event thats fire when the <see cref="CanExecute(object)"/> value has changed
        /// </summary>
#pragma warning disable 67
        public event EventHandler CanExecuteChanged; //always returns true
#pragma warning restore 67
    }

}
