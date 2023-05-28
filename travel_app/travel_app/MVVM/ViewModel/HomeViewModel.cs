using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using travel_app.Command;
using travel_app.Core;
using travel_app.MVVM.View;
using travel_app.Services;
using travel_app.Store;

namespace travel_app.MVVM.ViewModel
{
    public class HomeViewModel : ObservableObject
    {
        public ICommand NavigateDetailsCommand { get; }

        public HomeViewModel(NavigationStore navigationStore)
        {
            NavigateDetailsCommand = new NavigateCommand<SalesViewModel>(navigationStore, () => new SalesViewModel(navigationStore));
        }

        public List<Travel> Travels
        {
            get
            {
                // You can populate the list from any source you want
                return new List<Travel>()
                {
                new Travel() { Name = "Paris", Description = "The city of love", Price = 1000, ShortDescription = "The city of love", Image="" , Command = NavigateDetailsCommand
            },
                new Travel() { Name = "London", Description = "The city of history", Price = 1200, ShortDescription = "The city of history", Image="" , Command = NavigateDetailsCommand},
                new Travel() { Name = "New York", Description = "The city of dreams", Price = 1500, ShortDescription = "The city of dreams", Image="" , Command = NavigateDetailsCommand}
            };
            }
        }
    }
}
