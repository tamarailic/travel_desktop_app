using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using travel_app.MVVM.Model;

namespace travel_app.MVVM.View
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            
        }

        public static List<Travel> Travels
        {
            get
            {
                using(var db = new TravelContext())
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
