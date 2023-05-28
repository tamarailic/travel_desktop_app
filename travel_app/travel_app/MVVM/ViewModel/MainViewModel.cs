using System.Windows.Input;
using travel_app.Command;
using travel_app.Core;
using travel_app.Store;

namespace travel_app.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private readonly NavigationStore _navigationStore;
        public ObservableObject CurrentViewModel => _navigationStore.CurrentViewModel;
        public ICommand HomeViewCommand { get; }
        public ICommand SalesViewCommand { get; }

        public MainViewModel(NavigationStore navigationStore) {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            HomeViewCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore));
            SalesViewCommand = new NavigateCommand<SalesViewModel>(navigationStore, () => new SalesViewModel(navigationStore));
        }
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }
}
