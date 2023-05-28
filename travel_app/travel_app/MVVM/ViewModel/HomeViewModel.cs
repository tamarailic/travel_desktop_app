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

namespace travel_app.MVVM.ViewModel
{
    public class HomeViewModel : ObservableObject
    {
        public ICommand NavigateDetailsCommand { get; }

        public HomeViewModel(NavigationStore navigationStore)
        {
            NavigateDetailsCommand = new NavigateCommand<SalesViewModel>(navigationStore, () => new SalesViewModel(navigationStore));
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

        private void CreateNewTravel(object sender, RoutedEventArgs e)
        {
            var newTravelView = new CreateNewTravelView();
            newTravelView.Show();
        }

        private void SeeDetailes(object sender, MouseButtonEventArgs e)
        {
            Border? border = sender as Border;
            if (border != null)
            {
                Travel? travel = border.DataContext as Travel;
                if (travel != null)
                {
                    // You can use any method you want to navigate, such as using a Frame or a NavigationWindow
                    var detailsView = new DetailsView();
                    detailsView.DataContext = travel;
                    detailsView.Show();
                }
            }
        }
    }
}
