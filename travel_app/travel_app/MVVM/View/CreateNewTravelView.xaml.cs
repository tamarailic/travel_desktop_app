using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Maps.MapControl.WPF;
using System.Resources;
using System.Drawing;
using Newtonsoft.Json;
using System.Collections;

namespace travel_app.MVVM.View
{
    public partial class CreateNewTravelView : UserControl
    {

        private string _fromAddress = string.Empty;
        private List<double> _fromLocation = new List<double>();
        private string _toAddress = string.Empty;
        private List<double> _toLocation = new List<double>();

        public List<string> ChoosenAttractions { get; set; } = new List<string>() {};
        public List<string> ChoosenRestaurants { get; set; } = new List<string>() {};
        public List<string> ChoosenHotels { get; set; } = new List<string>() {};

        public CreateNewTravelView()
        {
            InitializeComponent();
            DataContext = this;

            InitializeOptions();
        }

        private void InitializeOptions()
        {
            using(var db = new TravelContext())
            {
                var attrations = db.Attractions.ToList();
                attractionComboBox.ItemsSource = attrations;

                var restaurants = db.Restaurants.ToList();
                restaurantsComboBox.ItemsSource = restaurants;

                var hotels = db.Hotels.ToList();
                hotelsComboBox.ItemsSource = hotels;
            }
            
        }

        private byte[] imageData; // A variable to store the image data
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                photo.Source = new BitmapImage(new Uri(op.FileName));
                imageData = File.ReadAllBytes(op.FileName);
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var newTravelName = TravelName.Text.Trim();
            var newTravelShortDesctiption = TravelShortDescription.Text.Trim();
            var newTravelLongDescription = TravelLongDescription.Text.Trim();
            var newTravelPriceString = TravelPrice.Text.Trim();
            int newTravelPrice;
            if (imageData.Length > 0 && newTravelName != "" && newTravelShortDesctiption != "" && newTravelLongDescription != "" && int.TryParse(newTravelPriceString, out newTravelPrice) && _fromAddress != "" && _toAddress != "")
            {
                using (var db = new TravelContext())
                {
                    Travel newTravel = new Travel()
                    {
                        Name = newTravelName,
                        ShortDescription = newTravelShortDesctiption,
                        Description = newTravelLongDescription,
                        Price = newTravelPrice,
                        Image = imageData,
                        Start = _fromAddress,
                        End = _toAddress
                    };
                    if(ChoosenAttractions.Count > 0)
                    {
                        newTravel.Attractions.AddRange(db.Attractions.Where(el => ChoosenAttractions.Contains(el.Name)));
                    }
                    if(ChoosenHotels.Count > 0)
                    {
                        newTravel.Hotels.AddRange(db.Hotels.Where(el => ChoosenHotels.Contains(el.Name)));
                    }
                    if(ChoosenRestaurants.Count > 0)
                    {
                        newTravel.Restaurants.AddRange(db.Restaurants.Where(el => ChoosenHotels.Contains(el.Name)));
                    }

                    try
                    {
                        db.Travels.Add(newTravel);
                        db.SaveChanges();
                        MessageBox.Show("Novo putovanje je uspešno kreirano");

                        TravelName.Text = string.Empty;
                        TravelShortDescription.Text = string.Empty;
                        TravelLongDescription.Text = string.Empty;
                        TravelPrice.Text = string.Empty;
                        StartLocation.Text = string.Empty;
                        EndLocation.Text = string.Empty;
                        ChoosenAttractions.Clear();
                        ChoosenHotels.Clear();
                        ChoosenRestaurants.Clear();
                        InitializeOptions();
                    }catch(Exception ex) { 
                        MessageBox.Show("Podaci putovanja nisu u ispravnom obliku. Proverite sva polja i pokušajte ponovo.", "Neuspelo kreiranje", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Dodavanje nije uspelo. Proverite da li su svi unosi ispravno popunjeni i pokušajte ponovo", "Neuspelo dodavanje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UpdateMap(object sender, RoutedEventArgs e)
        {
            if (StartLocation.Text.Trim() != "" && StartLocation.Text.Trim() != _fromAddress)
            {
                if (_fromAddress != "" && _toAddress != "")
                {
                    mainMap.Children.Clear();
                    AddMainPin(_toLocation[0], _toLocation[1]);
                }
                _fromAddress = StartLocation.Text.Trim();
                var latlng = await GetLatLngFromAddress(_fromAddress);
                _fromLocation = latlng;

                AddMainPin(_fromLocation[0], _fromLocation[1]);

                mainMap.SetView(new Location(_fromLocation[0], _fromLocation[1]), 7);

                if (_fromAddress != "" && _toAddress!= "")
                {
                    DrawRoute(new List<List<double>> {_fromLocation,_toLocation});
                }
            }
            if (EndLocation.Text.Trim() != "" && EndLocation.Text.Trim() != _toAddress)
            {
                if (_toAddress != "" && _fromAddress != "")
                {
                    mainMap.Children.Clear();
                    AddMainPin(_fromLocation[0], _fromLocation[1]);
                }
                _toAddress = EndLocation.Text.Trim();
                var latlng = await GetLatLngFromAddress(_toAddress);
                _toLocation = latlng;

                AddMainPin(_toLocation[0], _toLocation[1]);

                mainMap.SetView(new Location(_toLocation[0], _toLocation[1]), 7);

                if (_fromAddress != "" && _toAddress != "")
                {
                    DrawRoute(new List<List<double>> { _fromLocation, _toLocation });
                }
            }
        }

        private async Task<List<double>> GetLatLngFromAddress(string address)
        {
            // Create an HttpClient instance
            HttpClient client = new HttpClient();

            // Set the request parameters
            var countryRegion = "RS";
            var addressLine = address;
            var maxResults = "1";
            var BingMapsKey = "AoDHQrsDjWxAd2BBTIt1TbvTMLqIHZ_Gki6iY1HMq-dcSVwoyF2K4QWog7Fi5QK8";

            // Build the request URL
            var requestUrl = $"http://dev.virtualearth.net/REST/v1/Locations?countryRegion={countryRegion}&addressLine={addressLine}&maxResults={maxResults}&strictMatch=1&locality=Serbia&key={BingMapsKey}";

            // Send the request and get the response as a JSON object
            var response = await client.GetFromJsonAsync<BingApiResponse>(requestUrl);
            var coordinates = response?.ResourceSets[0].Resources[0].Point.Coordinates;
            return coordinates;
        }

        private void DrawRoute(List<List<double>> locations)
        {
            MapPolyline polyline = new MapPolyline();
            polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            polyline.StrokeThickness = 5;
            polyline.Opacity = 0.7;
            polyline.Locations = new LocationCollection();
            foreach(var location in locations)
            {
                polyline.Locations.Add(new Location(location[0], location[1]));
            }
            
            mainMap.Children.Add(polyline);
        }

        private void AddAttraction(object sender, RoutedEventArgs e)
        {
            var result = MessageBoxResult.Yes;
            if (!MainWindow.LogedInUser.Pro)
            {
                result = MessageBox.Show($"Da li ste sigurni da želite da uključite atrakciju {attractionComboBox.SelectedItem}?", "Potvrdite dodavanje atrakcije", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new TravelContext())
                {
                    string attractionName = attractionComboBox.SelectedItem.ToString();
                    ChoosenAttractions.Add(attractionName);
                    CollectionViewSource.GetDefaultView(ChoosenAttractionsListBox.ItemsSource).Refresh();

                    AddAttractionToMap();

                    var remainingAttractions = new List<Attractions>();
                    foreach(var attraction in db.Attractions.ToList())
                    {
                        if (attraction.Name != attractionName) { remainingAttractions.Add(attraction); }
                    }
                    attractionComboBox.ItemsSource = remainingAttractions;
                }
            }
        }

        private void AddRestaurant(object sender, RoutedEventArgs e)
        {
            var result = MessageBoxResult.Yes;
            if (!MainWindow.LogedInUser.Pro)
            {
                result = MessageBox.Show($"Da li ste sigurni da želite da uključite restoran {attractionComboBox.SelectedItem}?", "Potvrdite dodavanje restorana", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new TravelContext())
                {
                    string restaurantName = restaurantsComboBox.SelectedItem.ToString();
                    ChoosenRestaurants.Add(restaurantName);
                    CollectionViewSource.GetDefaultView(ChoosenRestaurantsListBox.ItemsSource).Refresh();

                    var remainingRestaurants = new List<Restaurants>();
                    foreach (var restaurant in db.Restaurants.ToList())
                    {
                        if (restaurant.Name != restaurantName) { remainingRestaurants.Add(restaurant); }
                    }
                    restaurantsComboBox.ItemsSource = remainingRestaurants;
                }
            }
        }

        private void AddHotel(object sender, RoutedEventArgs e)
        {
            var result = MessageBoxResult.Yes;
            if (!MainWindow.LogedInUser.Pro)
            {
                result = MessageBox.Show($"Da li ste sigurni da želite da uključite smeštaj {attractionComboBox.SelectedItem}?", "Potvrdite dodavanje smeštaja", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new TravelContext())
                {
                    string hotelName = hotelsComboBox.SelectedItem.ToString();
                    ChoosenHotels.Add(hotelName);
                    CollectionViewSource.GetDefaultView(ChoosenHotelsListBox.ItemsSource).Refresh();

                    var remainingHotels = new List<Hotels>();
                    foreach (var hotel in db.Hotels.ToList())
                    {
                        if (hotel.Name != hotelName) { remainingHotels.Add(hotel); }
                    }
                    hotelsComboBox.ItemsSource = remainingHotels;
                }
            }
        }

        private async void AddAttractionToMap()
        {
            List<List<double>> attractionLocations = new List<List<double>>();
            using(var db = new TravelContext())
            {
                foreach(var attractionName in ChoosenAttractions)
                {
                    var attraction = db.Attractions.Where(el => el.Name == attractionName).FirstOrDefault();
                    
                    mainMap.Children.Clear();

                    if (_fromAddress != "")
                    {
                        AddMainPin(_fromLocation[0], _fromLocation[1]);
                    }
                    if (_toAddress != "")
                    {
                        AddMainPin(_toLocation[0], _toLocation[1]);
                    }

                    var latlng = await GetLatLngFromAddress(attraction.Address);

                    AddAttractionPin(latlng[0], latlng[1]);
                    mainMap.SetView(new Location(latlng[0], latlng[1]), 7);
                    attractionLocations.Add(latlng);
                }

                if (_fromAddress != "" && _toAddress != "")
                {
                    var locationsToConnect = new List<List<double>> { _fromLocation };
                    foreach (var location in attractionLocations)
                    {
                        locationsToConnect.Add(location);
                    }
                    locationsToConnect.Add(_toLocation);
                    DrawRoute(locationsToConnect);
                }
            }
        }

        private void AddMainPin(double lat, double lng)
        {
            Pushpin startEndPin = new Pushpin();
            startEndPin.Location = new Location(lat, lng);
            startEndPin.Background = new SolidColorBrush(Colors.Blue);
            mainMap.Children.Add(startEndPin);
        }

        private void AddAttractionPin(double lat, double lng)
        {
            Pushpin attractionPin = new Pushpin();
            attractionPin.Location = new Location(lat, lng);
            mainMap.Children.Add(attractionPin);
        }
    }
}
