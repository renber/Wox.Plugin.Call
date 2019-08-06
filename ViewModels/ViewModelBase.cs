using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Wox.Plugin.Call.ViewModels
{
    class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var pc = PropertyChanged;
            if (pc != null)
                pc(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the value of the backing field to newValue <br/>
        /// If the values are different a PropertyChangedEvent is raised for teh given propertyName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="backingField">A reference to the field which backs the property and should be updated</param>
        /// <param name="newValue">The new value to set</param>
        /// <param name="propertyName">The property name</param>
        /// <returns>True when the value was changed</returns>
        protected bool SetProperty<T>(ref T backingField, T newValue, [CallerMemberName] String propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(backingField, newValue))
            {
                backingField = newValue;
                OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }
    }
}
