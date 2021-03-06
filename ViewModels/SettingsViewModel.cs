﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Wox.Plugin.Call.Infrastructure;

namespace Wox.Plugin.Call.ViewModels
{
    class SettingsViewModel : ViewModelBase
    {
        CallPluginSettings Model { get; }

        private ObservableCollection<EntryViewModel> Entries { get; }

        private CollectionViewSource entriesViewSource;
        public ICollectionView EntriesView => entriesViewSource.View;

        public string CallCommandTemplate
        {
            get => Model.CallCommandTemplate;
            set
            {
                if (Model.CallCommandTemplate != value)
                {
                    Model.CallCommandTemplate = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddEntryCommand { get; }
        public ICommand RemoveEntryCommand { get; }

        public SettingsViewModel(CallPluginSettings settings)
        {
            Model = settings;
            Entries = new SyncCollection<EntryViewModel, Entry>(settings.Entries, (m) => new EntryViewModel(m), (vm) => vm.Model);

            entriesViewSource = new CollectionViewSource();            
            entriesViewSource.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            entriesViewSource.Source = Entries;

            AddEntryCommand = new RelayCommand(() => Entries.Add(new EntryViewModel(new Entry("New entry", "na"))));
            RemoveEntryCommand = new RelayCommand<EntryViewModel>((entry) => Entries.Remove(entry), (entry) => entry != null);
        }
    }

    class EntryViewModel : ViewModelBase
    {
        public Entry Model { get; }

        public string Name
        {
            get => Model.Name;
            set
            {
                if (Model.Name != value)
                {
                    Model.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Number
        {
            get => Model.Number;
            set
            {
                if (Model.Number != value)
                {
                    Model.Number = value;
                    OnPropertyChanged();
                }
            }
        }

        public EntryViewModel(Entry entry)
        {
            Model = entry ?? throw new ArgumentNullException(nameof(entry));
        }
    }

}
