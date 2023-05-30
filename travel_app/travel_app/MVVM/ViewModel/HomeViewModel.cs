using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using travel_app.Command;
using travel_app.Core;
using travel_app.MVVM.Model;
using travel_app.MVVM.View;
using travel_app.Services;
using travel_app.Store;
using System.Diagnostics;

namespace travel_app.MVVM.ViewModel
{
    public class HomeViewModel : ObservableObject
    {
        public ICommand NavigateDetailsCommand { get; }
        public ICommand CreateNewCommand { get; }

        public HomeViewModel(NavigationStore navigationStore)
        {
            NavigateDetailsCommand = new NavigateCommand<SalesViewModel>(navigationStore, () => new SalesViewModel(navigationStore));
            CreateNewCommand = new NavigateCommand<CreateNewTravelViewModel>(navigationStore, () => new CreateNewTravelViewModel(navigationStore));
        }

        public static List<Travel> Travels
        {
            get
            {
                using (var db = new TravelContext())
                {
                    return db.Travels.ToList();
                }
            }
        }
    }
}
