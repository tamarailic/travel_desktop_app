using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using WPFCustomMessageBox;

namespace travel_app.MVVM.View
{
    public partial class AttractionsView : UserControl
    {
        public string AttractionName { get; set; }
        public string AttractionAddress { get; set; }
        public string AttractionType { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }
        public string RestaurantType { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string HotelStars { get; set; }
 
        public AttractionsView()
        {
            InitializeComponent();
            DataContext = this;
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

                hotelStarsComboBox.ItemsSource = new List<string>() { "1","2", "3", "4", "5"};
            }
        }

        private void CreateNewHotel(object sender, RoutedEventArgs e)
        {
            if (ValidateHotelData()) {
                var result = MessageBoxResult.Yes;
                if (!MainWindow.LogedInUser.Pro)
                {
                    result = CustomMessageBox.ShowYesNo($"Da li ste sigurni da želite da dodate smeštaj {hotelName.Text.Trim()}?", "Potvrdite dodavanje smeštaja", "Da", "Ne");
                }
                if (result == MessageBoxResult.Yes)
                {
                    using (var db = new TravelContext())
                    {
                        try
                        {
                            var name = hotelName.Text.Trim();
                            var address = hotelLocation.Text.Trim();
                            var stars = int.Parse(hotelStarsComboBox.SelectedValue.ToString());

                            db.Hotels.Add(new Hotels(name, address, stars));
                            db.SaveChanges();
                            

                            hotelName.Clear();
                            
                            hotelLocation.Text = string.Empty;
                            hotelStarsComboBox.SelectedIndex = -1;

                            CustomMessageBox.ShowOK($"Novi smeštaj pod nazivom {name} je uspešno dodat", "Novi smeštaj", "U redu");
                        }
                        catch (Exception ex)
                        {
                            CustomMessageBox.ShowOK("Greška prilikom unosa. Smeštaj pod tim imenom već postoji ili podaci forme nisu u odgovarajućem obliku. Ispravite unos i pokušajte ponovo.", "Neuspeo unos", "U redu");
                        }
                    }
                }
            }
             
        }

        private bool ValidateHotelData()
        {
            var bindingExpressionhotelName = hotelName.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionhotelName.UpdateSource();
            if (Validation.GetHasError(hotelName))
            {
                var errors = Validation.GetErrors(hotelName);
                CustomMessageBox.ShowOK(errors[0].ErrorContent.ToString(), "Greška", "U red");
                return false;
            }

            var bindingExpressionhotelLocation = hotelLocation.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionhotelLocation.UpdateSource();
            if (Validation.GetHasError(hotelLocation))
            {
                var errors = Validation.GetErrors(hotelLocation);
                CustomMessageBox.ShowOK(errors[0].ErrorContent.ToString(), "Greška", "U redu");
                return false;
            }

            var bindingExpressionhotelStarsComboBox = hotelStarsComboBox.GetBindingExpression(ComboBox.SelectedValueProperty);
            bindingExpressionhotelStarsComboBox.UpdateSource();
            if (Validation.GetHasError(hotelStarsComboBox))
            {
                var errors = Validation.GetErrors(hotelStarsComboBox);
                CustomMessageBox.ShowOK(errors[0].ErrorContent.ToString(), "Greška", "U redu");
                return false;
            }
            return true;
        }

        private void CreateNewRestaurant(object sender, RoutedEventArgs e)
        {
            if (ValidateRestaurantData())
            {
                var result = MessageBoxResult.Yes;
                if (!MainWindow.LogedInUser.Pro)
                {
                    result = CustomMessageBox.ShowYesNo($"Da li ste sigurni da želite da dodate restoran {restaurantName.Text.Trim()}?", "Potvrdite dodavanje restorana", "Da", "Ne");
                }
                if (result == MessageBoxResult.Yes)
                {
                    using (var db = new TravelContext())
                    {
                        var name = restaurantName.Text.Trim();
                        var address = restaurantLocation.Text.Trim();
                        var typeName = restaurantTypesComboBox.SelectedItem.ToString();
                        var typeId = db.RestaurantTypes.Where(restaurent => restaurent.Name == typeName).Select(el => el.Id).FirstOrDefault();

                        try
                        {
                            db.Restaurants.Add(new Restaurants(name, address, typeId));
                            db.SaveChanges();
                            restaurantName.Text = string.Empty;
                            restaurantLocation.Text = string.Empty;
                            restaurantTypesComboBox.SelectedIndex = -1;
                            CustomMessageBox.ShowOK($"Novi restoran pod nazivom {name} je uspešno dodat", "Novi restoran", "U redu");
                        }
                        catch (Exception ex)
                        {
                            CustomMessageBox.ShowYesNo("Greška prilikom unosa. Restoran pod tim imenom već postoji ili podaci forme nisu u odgovarajućem obliku. Ispravite unos i pokušajte ponovo.", "Unos neuspesan", "Da", "Ne");
                        }
                    }
                }
            }
        }

        private bool ValidateRestaurantData()
        {
            var bindingExpressionrestaurantName = restaurantName.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionrestaurantName.UpdateSource();
            if (Validation.GetHasError(restaurantName))
            {
                var errors = Validation.GetErrors(restaurantName);
                CustomMessageBox.ShowYesNo(errors[0].ErrorContent.ToString(), "Greška", "Da", "Ne");
                return false;
            }

            var bindingExpressionrestaurantLocation = restaurantLocation.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionrestaurantLocation.UpdateSource();
            if (Validation.GetHasError(restaurantLocation))
            {
                var errors = Validation.GetErrors(restaurantLocation);
                CustomMessageBox.ShowYesNo(errors[0].ErrorContent.ToString(), "Greška", "Da", "Ne");
                return false;
            }

            var bindingExpressionrestaurantTypesComboBox = restaurantTypesComboBox.GetBindingExpression(ComboBox.SelectedValueProperty);
            bindingExpressionrestaurantTypesComboBox.UpdateSource();
            if (Validation.GetHasError(restaurantTypesComboBox))
            {
                var errors = Validation.GetErrors(restaurantTypesComboBox);
                CustomMessageBox.ShowYesNo(errors[0].ErrorContent.ToString(), "Greška", "Da", "Ne");
                return false;
            }
            return true;
        }

        private void CreateNewAttraction(object sender, RoutedEventArgs e)
        {
            if (ValidateAttractionData())
            {
                var result = MessageBoxResult.Yes;
                if (!MainWindow.LogedInUser.Pro)
                {
                    result = CustomMessageBox.ShowYesNo($"Da li ste sigurni da želite da kreirate atrakciju {attractionName.Text.Trim()}?", "Potvrdite kreiranje atrakcije", "Yes", "No");
                }
                if (result == MessageBoxResult.Yes)
                {
                    using (var db = new TravelContext())
                    {
                        try
                        {
                            var name = attractionName.Text.Trim();
                            var address = attractionAddress.Text.Trim();
                            var typeName = attractionTypesComboBox.SelectedItem.ToString();
                            var typeId = db.AttractionTypes.Where(attraction => attraction.Name == typeName).Select(el => el.Id).FirstOrDefault();

                            db.Attractions.Add(new Attractions(name, address, typeId));
                            db.SaveChanges();
                            attractionName.Text = string.Empty;
                            attractionAddress.Text = string.Empty;
                            attractionTypesComboBox.SelectedIndex = -1;
                            CustomMessageBox.ShowOK("Nova atrakcija je uspešno kreirana", "Nova atrakcija", "U redu");
                        }
                        catch (Exception ex)
                        {
                            CustomMessageBox.ShowOK("Greška prilikom unosa. Atrakcija pod tim imenom već postoji.", "Neuspeo unos", "U redu");
                        }
                    }
                }
            }
        }

        private bool ValidateAttractionData()
        {
            var bindingExpressionattractionName = attractionName.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionattractionName.UpdateSource();
            if (Validation.GetHasError(attractionName))
            {
                var errors = Validation.GetErrors(attractionName);
                CustomMessageBox.ShowYesNo(errors[0].ErrorContent.ToString(), "Greška", "Da", "Ne");
                return false;
            }

            var bindingExpressionattractionAddress = attractionAddress.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionattractionAddress.UpdateSource();
            if (Validation.GetHasError(attractionAddress))
            {
                var errors = Validation.GetErrors(attractionAddress);
                CustomMessageBox.ShowYesNo(errors[0].ErrorContent.ToString(), "Greška", "Da", "Ne");
                return false;
            }

            var bindingExpressionattractionTypesComboBox = attractionTypesComboBox.GetBindingExpression(ComboBox.SelectedValueProperty);
            bindingExpressionattractionTypesComboBox.UpdateSource();
            if (Validation.GetHasError(attractionTypesComboBox))
            {
                var errors = Validation.GetErrors(attractionTypesComboBox);
                CustomMessageBox.ShowYesNo(errors[0].ErrorContent.ToString(), "Greška", "Da", "Ne");
                return false;
            }
            return true;
        }

        private void BtnBrowse_ClickAttraction(object sender, RoutedEventArgs e)
        {
            if(MainWindow.LogedInUser.Pro == true)
            {
                var result = CustomMessageBox.ShowYesNo("Potvrdite unos preko CSV-a. Kliknite 'Da' ukoliko želite da učitate atrakcije iz izabranog fajla.", "Potvrda unosa", "Da", "Ne");
                if (result == MessageBoxResult.No) return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                txtFilePath.Text = openFileDialog.FileName;
                ReadCsvFileAttraction();
            }
        }

        private void BtnBrowse_ClickRestaurant(object sender, RoutedEventArgs e)
        {
            if (MainWindow.LogedInUser.Pro == true)
            {
                var result = CustomMessageBox.ShowYesNo("Potvrdite unos preko CSV-a. Kliknite 'Da' ukoliko želite da učitate restorane iz izabranog fajla.", "Potvrda unosa", "Da", "Ne");
                if (result == MessageBoxResult.No) return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                txtFilePathRestaurant.Text = openFileDialog.FileName;
                ReadCsvFileRestaurant();
            }
        }


        private void BtnBrowse_ClickHotel(object sender, RoutedEventArgs e)
        {
            if (MainWindow.LogedInUser.Pro == true)
            {
                var result = CustomMessageBox.ShowYesNo("Potvrdite unos preko CSV-a. Kliknite 'Da' ukoliko želite da učitate hotele iz izabranog fajla.", "Potvrda unosa", "Da", "Ne");
                if (result == MessageBoxResult.No) return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                txtFilePathHotel.Text = openFileDialog.FileName;
                ReadCsvFileHotel();
            }
        }



        private void ReadCsvFileAttraction()
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
                CustomMessageBox.ShowOK("Greška prilikom učitavanja podataka iz fajla. Fajl nije u odgovarajućem obliku.", "Greška prilikom unosa", "U redu");
            }
            saveToDBAttraction(attractions);    
        }

        private void saveToDBAttraction(List<Attractions> attractions)
        {
            using (var db = new TravelContext())
            {
                db.Attractions.AddRange(attractions);
                db.SaveChanges();

                CustomMessageBox.ShowOK($"Uspešno je dodat novih {attractions.Count} atrakcija", "Uspešno", "U redu");
            }
        }

        private void ReadCsvFileRestaurant()
        {
            List<Restaurants> restaurants = new List<Restaurants>();
            try
            {
                using (StreamReader sr = new StreamReader(txtFilePathRestaurant.Text))
                {
                    if (chkHasHeaderRestaurant.IsChecked == true)
                    {
                        sr.ReadLine();
                    }
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] tokens = line.Split(',');
                        Restaurants restaurant = new Restaurants(tokens[0], tokens[1], int.Parse(tokens[2]));
                        restaurants.Add(restaurant);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowOK("Greška prilikom učitavanja podataka iz fajla. Fajl nije u odgovarajućem obliku.", "Greška prilikom unosa", "U redu");
            }
            saveToDBRestaurant(restaurants);
        }

        private void saveToDBRestaurant(List<Restaurants> restaurants)
        {
            using (var db = new TravelContext())
            {
                db.Restaurants.AddRange(restaurants);
                db.SaveChanges();

                CustomMessageBox.ShowOK($"Uspešno je dodat novih {restaurants.Count} restorana", "Uspešno", "U redu");
            }
        }

        private void ReadCsvFileHotel()
        {
            List<Hotels> hotels = new List<Hotels>();
            try
            {
                using (StreamReader sr = new StreamReader(txtFilePathHotel.Text))
                {
                    if (chkHasHeaderHotel.IsChecked == true)
                    {
                        sr.ReadLine();
                    }
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] tokens = line.Split(',');
                        Hotels hotel = new Hotels(tokens[0], tokens[1], int.Parse(tokens[2]));
                        hotels.Add(hotel);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowOK("Greška prilikom učitavanja podataka iz fajla. Fajl nije u odgovarajućem obliku.", "Greška prilikom unosa", "U redu");
            }
            saveToDBHotels(hotels);
        }

        private void saveToDBHotels(List<Hotels> hotels)
        {
            using (var db = new TravelContext())
            {
                db.Hotels.AddRange(hotels);
                db.SaveChanges();

                CustomMessageBox.ShowOK($"Uspešno je dodat novih {hotels.Count} hotela", "Uspešno", "U redu");
            }
        }

    }
}
