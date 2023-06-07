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
    internal class SalesViewModel:ObservableObject
    {
        public ICommand NavigateCommand { get; }

        public SalesViewModel(NavigationStore navigationStore)
        {
            NavigateCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore));
        }
    }
}
