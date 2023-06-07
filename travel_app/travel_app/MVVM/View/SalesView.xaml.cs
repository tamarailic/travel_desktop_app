using System;
using System.Collections;
using System.Collections.Generic;
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
    public partial class SalesView : UserControl
    {
        public SalesView()
        {
            InitializeComponent();
            InitializeOptions();
        }

        private void InitializeOptions()
        {
            monthComboBox.ItemsSource = new ArrayList() { "Januar", "Februar", "Mart", "April", "Maj", "Jun", "Jul", "Avgust", "Septembar", "Oktobar", "Novembar", "Decembar" };
            ByMonthTitle.Text = $"Analiza prodaja za {GetMonthName(DateTime.Now.Month)}";
            ByTravelTitle.Text = $"Analiza prodaja po putovanju";

            monthComboBox.SelectedIndex = DateTime.Now.Month - 1;

            List<Sale> salesMonth = new List<Sale>();
            List<Travel> salesTravel = new List<Travel>();
            using (var db = new TravelContext())
            {
                salesMonth = db.Sales.Include("Travel").Include("User").Where(item => item.DateTime.Month == DateTime.Now.Month).ToList();
                salesTravel = db.Travels.ToList();
            }
            salesByMonth.ItemsSource = salesMonth;
            salesByTravel.ItemsSource = salesTravel;
        }

        private string GetMonthName(int monthIndex)
        {
            Dictionary<int, string> months = new Dictionary<int, string> { { 1, "Januar" }, { 2, "Februar" }, { 3, "Mart" }, { 4, "April" }, { 5, "Maj" }, { 6, "Jun" }, { 7, "Jul" }, { 8, "Avgust" }, { 9, "Septembar" }, { 10, "Oktobar" }, { 11, "Novembar" }, { 12, "Decembar" } };
            return months[monthIndex];
        }

        private void monthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var db = new TravelContext())
            {
                
                salesByMonth.ItemsSource = db.Sales.Include("Travel").Include("User").Where(item => item.DateTime.Month == monthComboBox.SelectedIndex + 1).ToList();
                ByMonthTitle.Text = $"Analiza prodaja za {monthComboBox.SelectedItem}";
            }
        }
    }
}
