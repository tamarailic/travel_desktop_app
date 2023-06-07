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
    internal class CreateNewTravelViewModel : ObservableObject
    {
        public ICommand NavigateDetailsCommand { get; }

        
        public CreateNewTravelViewModel(NavigationStore navigationStore)
        {
            NavigateDetailsCommand = new NavigateCommand<SalesViewModel>(navigationStore, () => new SalesViewModel(navigationStore));
        }

    }
}
