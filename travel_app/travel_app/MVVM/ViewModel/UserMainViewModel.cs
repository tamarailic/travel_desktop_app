using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using travel_app.Command;
using travel_app.Core;
using travel_app.Store;

namespace travel_app.MVVM.ViewModel
{
    internal class UserMainViewModel:ObservableObject
    {
        private readonly NavigationStore _navigationStore;
        public ObservableObject CurrentViewModel => _navigationStore.CurrentViewModel;
        public ICommand HomeViewCommand { get; }
        public ICommand MyTravelViewCommand { get; }

        public UserMainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            HomeViewCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore));
            MyTravelViewCommand = new NavigateCommand<UserTravelViewModel>(navigationStore, () => new UserTravelViewModel(navigationStore));
        }
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
