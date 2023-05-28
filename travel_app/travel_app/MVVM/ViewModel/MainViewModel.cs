using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using travel_app.Core;

namespace travel_app.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand SalesViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand AttractionsViewCommand { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public SalesViewModel SalesVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        public AttractionsViewModel AttractionsVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel() { 
            HomeVM = new HomeViewModel();
            SalesVM = new SalesViewModel();
            SettingsVM = new SettingsViewModel();
            AttractionsVM = new AttractionsViewModel();
            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o=>
            {
                CurrentView = HomeVM;
            });
            SalesViewCommand = new RelayCommand(o => { 
                CurrentView = SalesVM;
            });
            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });
            AttractionsViewCommand = new RelayCommand(o =>
            {
                CurrentView = AttractionsVM;
            });
        }

    }
}
