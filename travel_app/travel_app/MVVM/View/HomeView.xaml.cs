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

namespace travel_app.MVVM.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
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
                // You can populate the list from any source you want
                return new List<Travel>()
                {
                new Travel() { Name = "Paris", Description = "The city of love", Price = 1000, ShortDescription = "The city of love", Image="" },
                new Travel() { Name = "London", Description = "The city of history", Price = 1200, ShortDescription = "The city of history", Image="" },
                new Travel() { Name = "New York", Description = "The city of dreams", Price = 1500, ShortDescription = "The city of dreams", Image="" }
            };
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

    public class Travel
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public ICommand Command { get; set; }
    }
}
