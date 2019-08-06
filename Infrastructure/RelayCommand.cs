using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wox.Plugin.Call.Infrastructure
{
    [Serializable]
    public class RelayCommand<T> : ICommand, INotifyPropertyChanged
       where T : class
    {
        Func<T, bool> canExecute;
        Action<T> executeAction;

        string _commandText = "";

        /// <summary>
        /// Beschreibung des Commands, die bspw. auf einem Button angezeigt werden soll
        /// </summary>
        public string CommandText
        {
            get
            {
                return _commandText;
            }
            set
            {
                if (_commandText != value)
                {
                    _commandText = value;
                    OnPropertyChanged("CommandText");
                }
            }
        }

        public RelayCommand(Action<T> executeAction)
            : this(executeAction, null)
        {
        }

        public RelayCommand(Action<T> executeAction, Func<T, bool> canExecute)
        {
            if (executeAction == null)
            {
                throw new ArgumentNullException("executeAction");
            }
            this.executeAction = executeAction;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            bool result = true;
            Func<T, bool> canExecuteHandler = this.canExecute;
            if (canExecuteHandler != null)
            {
                result = canExecuteHandler(parameter as T);
            }

            return result;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }



        public void Execute(object parameter)
        {
            this.executeAction(parameter as T);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    /// <summary>
    /// Relay Command without parameters
    /// </summary>
    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action executeAction)
            : base((o) => executeAction())
        {
        }

        public RelayCommand(Action executeAction, Func<bool> canExecute)
            : base((o) => executeAction(), (o) => canExecute())
        {

        }
    }
}
