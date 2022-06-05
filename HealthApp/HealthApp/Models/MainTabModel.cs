﻿using HealthApp.Common.Model;
using MvvmHelpers;

namespace HealthApp.Models
{
    public class MainTabModel : BaseViewModel
    {
        private Record _hotRecord;
        public Record HotRecord
        {
            get => _hotRecord;
            set 
            { 
                _hotRecord = value;
                OnPropertyChanged();
            }
        }

        private ObservableRangeCollection<TabModel> _subTabModel;
        public ObservableRangeCollection<TabModel> SubTabModel
        {
            get => _subTabModel;
            set
            {
                _subTabModel = value;
                OnPropertyChanged();
            }
        }

        private ObservableRangeCollection<Record> _records;
        public ObservableRangeCollection<Record> Records
        {
            get { return _records; }
            set
            {
                _records = value;
                OnPropertyChanged();
            }
        }

        private bool _hasError;
        public bool HasError
        {
            get { return _hasError; }
            set
            {
                _hasError = value;
                OnPropertyChanged();
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public MainTabModel()
        {
            SubTabModel = new ObservableRangeCollection<TabModel>();
            Records = new ObservableRangeCollection<Record>();
            HotRecord = new Record();

            IsBusy = true;
        }
    }
}