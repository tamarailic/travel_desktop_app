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

namespace travel_app.MVVM.View
{
    public partial class CreateNewTravelView : UserControl
    {

        private string _fromAddress = string.Empty;
        private List<double> _fromLocation = new List<double>();
        private string _toAddress = string.Empty;
        private List<double> _toLocation = new List<double>();

        private List<Attractions> ChoosenAttractions = new List<Attractions>();

        public CreateNewTravelView()
        {
            InitializeComponent();

            InitializeOptions();
        }

        private void InitializeOptions()
        {
            using(var db = new TravelContext())
            {
                var attrations = db.Attractions.ToList();
                attractionComboBox.ItemsSource = attrations;
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
            if (imageData.Length > 0 && newTravelName != "" && newTravelShortDesctiption != "" && newTravelLongDescription != "" && int.TryParse(newTravelPriceString, out newTravelPrice))
            {
                using (var db = new TravelContext())
                {
                    Travel newTravel = new Travel()
                    {
                        Name = newTravelName,
                        ShortDescription = newTravelShortDesctiption,
                        Description = newTravelLongDescription,
                        Price = newTravelPrice,
                        Image = imageData

                    };
                    db.Travels.Add(newTravel);
                    db.SaveChanges();
                    Trace.WriteLine("New travel sucessfully added");
                }
            }
            else
            {
                MessageBox.Show("Dodavanje nije uspelo. Proverite da li su svi unosi ispravno popunjeni i pokušajte ponovo");
            }
        }

        private async void UpdateMap(object sender, RoutedEventArgs e)
        {
            if (StartLocation.Text.Trim() != "" && StartLocation.Text.Trim() != _fromAddress)
            {
                if (_fromAddress != "")
                {
                    mainMap.Children.Clear();
                    Pushpin pinOld = new Pushpin();
                    pinOld.Location = new Location(_toLocation[0], _toLocation[1]);
                    mainMap.Children.Add(pinOld);
                }
                _fromAddress = StartLocation.Text.Trim();
                var latlng = await GetLatLngFromAddress(_fromAddress);
                _fromLocation = latlng;
                Pushpin pin = new Pushpin();

                pin.Location = new Location(_fromLocation[0], _fromLocation[1]);
                mainMap.Children.Add(pin);
                mainMap.SetView(new Location(_fromLocation[0], _fromLocation[1]), 7);

                if (_fromAddress != "" && _toAddress!= "")
                {
                    DrawRoute(new List<List<double>> {_fromLocation,_toLocation});
                }
            }
            if (EndLocation.Text.Trim() != "" && EndLocation.Text.Trim() != _toAddress)
            {
                if (_toAddress != "")
                {
                    mainMap.Children.Clear();
                    Pushpin pinOld = new Pushpin();
                    pinOld.Location = new Location(_fromLocation[0], _fromLocation[1]);
                    mainMap.Children.Add(pinOld);
                }
                _toAddress = EndLocation.Text.Trim();
                var latlng = await GetLatLngFromAddress(_toAddress);
                _toLocation = latlng;
                Pushpin pin = new Pushpin();

                pin.Location = new Location(_toLocation[0], _toLocation[1]);
                mainMap.Children.Add(pin);
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
            using (var db = new TravelContext())
            {
                var attractionName = attractionComboBox.SelectedItem;
                ChoosenAttractions.Add(db.Attractions.Where(el => el.Name == attractionName).FirstOrDefault());
            }
        }
    }
}
