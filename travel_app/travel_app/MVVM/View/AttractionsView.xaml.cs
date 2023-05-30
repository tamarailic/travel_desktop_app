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

namespace travel_app.MVVM.View
{
    public partial class AttractionsView : UserControl
    {
        public string check { get; set; }
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

                hotelStarsComboBox.ItemsSource = new ArrayList() { 1, 2, 3, 4, 5 };
            }
        }

        private void CreateNewHotel(object sender, RoutedEventArgs e)
        {
            if (ValidateHotelData()) {
                var result = MessageBoxResult.Yes;
                if (!MainWindow.LogedInUser.Pro)
                {
                    result = MessageBox.Show($"Da li ste sigurni da želite da dodate smeštaj {hotelName.Text.Trim()}?", "Potvrdite dodavanje smeštaja", MessageBoxButton.YesNo, MessageBoxImage.Question);
                }
                if (result == MessageBoxResult.Yes)
                {
                    using (var db = new TravelContext())
                    {
                        try
                        {
                            var name = hotelName.Text.Trim();
                            var address = hotelLocation.Text.Trim();
                            var stars = int.Parse(hotelStarsComboBox.SelectedItem.ToString());

                            db.Hotels.Add(new Hotels(name, address, stars));
                            db.SaveChanges();
                            hotelName.Text = string.Empty;
                            hotelLocation.Text = string.Empty;
                            hotelStarsComboBox.SelectedIndex = -1;

                            MessageBox.Show($"Novi smeštaj pod nazivom {name} je uspešno dodat");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Greška prilikom unosa. Smeštaj pod tim imenom već postoji ili podaci forme nisu u odgovarajućem obliku. Ispravite unos i pokušajte ponovo.", "Neuspeo unos", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionhotelLocation = hotelLocation.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionhotelLocation.UpdateSource();
            if (Validation.GetHasError(hotelLocation))
            {
                var errors = Validation.GetErrors(hotelLocation);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionhotelStarsComboBox = hotelStarsComboBox.GetBindingExpression(ComboBox.SelectedValueProperty);
            bindingExpressionhotelStarsComboBox.UpdateSource();
            if (Validation.GetHasError(hotelStarsComboBox))
            {
                var errors = Validation.GetErrors(hotelStarsComboBox);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    result = MessageBox.Show($"Da li ste sigurni da želite da dodate restoran {restaurantName.Text.Trim()}?", "Potvrdite dodavanje restorana", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                            MessageBox.Show($"Novi restoran pod nazivom {name} je uspešno dodat");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Greška prilikom unosa. Restoran pod tim imenom već postoji ili podaci forme nisu u odgovarajućem obliku. Ispravite unos i pokušajte ponovo.", "Unos neuspesan", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionrestaurantLocation = restaurantLocation.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionrestaurantLocation.UpdateSource();
            if (Validation.GetHasError(restaurantLocation))
            {
                var errors = Validation.GetErrors(restaurantLocation);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionrestaurantTypesComboBox = restaurantTypesComboBox.GetBindingExpression(ComboBox.SelectedValueProperty);
            bindingExpressionrestaurantTypesComboBox.UpdateSource();
            if (Validation.GetHasError(restaurantTypesComboBox))
            {
                var errors = Validation.GetErrors(restaurantTypesComboBox);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    result = MessageBox.Show($"Da li ste sigurni da želite da kreirate atrakciju {attractionName.Text.Trim()}?", "Potvrdite kreiranje atrakcije", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                            MessageBox.Show("Nova atrakcija je uspešno kreirana");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Greška prilikom unosa. Atrakcija pod tim imenom već postoji.", "Neuspeo unos", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionattractionAddress = attractionAddress.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionattractionAddress.UpdateSource();
            if (Validation.GetHasError(attractionAddress))
            {
                var errors = Validation.GetErrors(attractionAddress);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionattractionTypesComboBox = attractionTypesComboBox.GetBindingExpression(ComboBox.SelectedValueProperty);
            bindingExpressionattractionTypesComboBox.UpdateSource();
            if (Validation.GetHasError(attractionTypesComboBox))
            {
                var errors = Validation.GetErrors(attractionTypesComboBox);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
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
            if (openFileDialog.ShowDialog() == true && ValidCSV())
            {
                txtFilePath.Text = openFileDialog.FileName;
                ReadCsvFile();
            }
        }

        private bool ValidCSV() {
            var bindingExpressiontxtFilePath = txtFilePath.GetBindingExpression(TextBox.TextProperty);
            bindingExpressiontxtFilePath.UpdateSource();
            if (Validation.GetHasError(txtFilePath))
            {
                var errors = Validation.GetErrors(txtFilePath);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
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
