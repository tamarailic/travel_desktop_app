using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using travel_app.MVVM.Model;

namespace travel_app.MVVM.View
{
    /// <summary>
    /// Interaction logic for DetailsView.xaml
    /// </summary>
    public partial class DetailsView : UserControl
    {
        public DetailsView()
        {
            InitializeComponent();
            drawRoute();
        }

        private async void drawRoute()
        {
            await Task.Delay(1000);
            String _fromAddress = StartLocation.Text.Trim();
            var latlngStart = await GetLatLngFromAddress(_fromAddress);
            String _toAddress = EndLocation.Text.Trim();
            var latlngEnd = await GetLatLngFromAddress(_toAddress);
            if (_fromAddress != "")
            {
                AddMainPin(latlngStart[0], latlngStart[1]);
                mainMap.SetView(new Location(latlngStart[0], latlngStart[1]), 7);
            }
            if (_toAddress != "")
            {
                AddMainPin(latlngEnd[0], latlngEnd[1]);
            }

            Travel currentTravel;

            using (var db = new TravelContext())
            {
                currentTravel = db.Travels.Include("Attractions").Single(item => item.Name == TravelName.Text);
            }

            List<List<double>> attractionLocations = new List<List<double>>();

            foreach (var att in currentTravel.Attractions)
            {
                var latlngAttraction = await GetLatLngFromAddress(att.Address);
                attractionLocations.Add(latlngAttraction);
                AddAttractionPin(latlngAttraction[0], latlngAttraction[1]);
            }

            if (_fromAddress != "" && _toAddress != "")
            {
                var locationsToConnect = new List<List<double>> { latlngStart };
                foreach (var location in attractionLocations)
                {
                    locationsToConnect.Add(location);
                }
                locationsToConnect.Add(latlngEnd);
                DrawRoute(locationsToConnect);
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
            foreach (var location in locations)
            {
                polyline.Locations.Add(new Location(location[0], location[1]));
            }

            mainMap.Children.Add(polyline);
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
