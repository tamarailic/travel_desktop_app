using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    public partial class AttractionsView : UserControl
    {
        public AttractionsView()
        {
            InitializeComponent();

            InitializeOptions();
        }

        private void InitializeOptions()
        {
            using (var context = new TravelContext())
            {
                var attractionTypes = context.AttractionTypes.Select(el => el.Name).ToList();
                attractionTypesComboBox.ItemsSource = attractionTypes;

                var restaurantTypes = context.RestaurantTypes.Select(el => el.Name).ToList();
                restaurantTypesComboBox.ItemsSource = restaurantTypes;

                hotelStarsComboBox.ItemsSource = new ArrayList() { 1, 2, 3, 4, 5 };
            }
        }

        private void CreateNewHotel(object sender, RoutedEventArgs e)
        {

        }

        private void CreateNewRestaurant(object sender, RoutedEventArgs e)
        {

        }

        private void CreateNewAttraction(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            if(MainWindow.LogedInUser.Pro == true)
            {
                var result = MessageBox.Show("Potvrdite unos preko CSV-a. Kliknite Yes ukoliko želite da učitate atrakcije iz izabranog fajla.", "Potvrda unosa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No) return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                txtFilePath.Text = openFileDialog.FileName;
                ReadCsvFile();
            }
           
        }
        private void ReadCsvFile()
        {
            List<Attractions> attractions = new List<Attractions>();

            try
            {
                using (StreamReader sr = new StreamReader(txtFilePath.Text))
                {
                    if (chkHasHeader.IsChecked == true)
                    {
                        sr.ReadLine();
                    }
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] tokens = line.Split(',');
                        Attractions attraction = new Attractions(tokens[0], tokens[1], int.Parse(tokens[2]));
                        attractions.Add(attraction);
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Greška prilikom učitavanja podataka iz fajla. Fajl nije u odgovarajućem obliku.", "Greška prilikom unosa", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

            saveToDB(attractions);    
        }

        private void saveToDB(List<Attractions> attractions)
        {
            using (var db = new TravelContext())
            {
                db.Attractions.AddRange(attractions);
                db.SaveChanges();

                MessageBox.Show($"Uspešno je dodat novih {attractions.Count} atrakcija");

            }
        }

    }
}
