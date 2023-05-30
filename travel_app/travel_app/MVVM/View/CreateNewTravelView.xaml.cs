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
using System.Globalization;
using System.Collections.ObjectModel;
using travel_app.MVVM.ViewModel;

namespace travel_app.MVVM.View
{
    public partial class CreateNewTravelView : UserControl
    {
        public string check { get; set; }
        private string _fromAddress = string.Empty;
        private List<double> _fromLocation = new List<double>();
        private string _toAddress = string.Empty;
        private List<double> _toLocation = new List<double>();

        public new ObservableCollection<string> ChoosenAttractions { get; set; } = new ObservableCollection<string>() {};
        public ObservableCollection<string> ChoosenRestaurants { get; set; } = new ObservableCollection<string>() {};
        public ObservableCollection<string> ChoosenHotels { get; set; } = new ObservableCollection<string>() {};

        public CreateNewTravelView()
        {
            InitializeComponent();
            DataContext = this;

            InitializeOptions();
            Style itemContainerStyle1 = new Style(typeof(ListBoxItem));
            itemContainerStyle1.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));
            itemContainerStyle1.Setters.Add(new EventSetter(ListBoxItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(s_PreviewMouseLeftButtonDown)));
            itemContainerStyle1.Setters.Add(new EventSetter(ListBoxItem.DropEvent, new DragEventHandler(listboxAttractions_Drop)));
            ChoosenAttractionsListBox.ItemContainerStyle = itemContainerStyle1;

            Style itemContainerStyle2 = new Style(typeof(ListBoxItem));
            itemContainerStyle2.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));
            itemContainerStyle2.Setters.Add(new EventSetter(ListBoxItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(s_PreviewMouseLeftButtonDown)));
            itemContainerStyle2.Setters.Add(new EventSetter(ListBoxItem.DropEvent, new DragEventHandler(listboxRestaurants_Drop)));
            ChoosenRestaurantsListBox.ItemContainerStyle = itemContainerStyle2;

            Style itemContainerStyle3 = new Style(typeof(ListBoxItem));
            itemContainerStyle3.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));
            itemContainerStyle3.Setters.Add(new EventSetter(ListBoxItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(s_PreviewMouseLeftButtonDown)));
            itemContainerStyle3.Setters.Add(new EventSetter(ListBoxItem.DropEvent, new DragEventHandler(listboxHotels_Drop)));
            ChoosenHotelsListBox.ItemContainerStyle = itemContainerStyle3;
        }

        void s_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem)
            {
                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        void listboxAttractions_Drop(object sender, DragEventArgs e)
        {
            string droppedData = e.Data.GetData(typeof(string)) as string;
            string target = ((ListBoxItem)(sender)).DataContext as string;

            int removedIdx = ChoosenAttractionsListBox.Items.IndexOf(droppedData);
            int targetIdx = ChoosenAttractionsListBox.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                ChoosenAttractions.Insert(targetIdx + 1, droppedData);
                ChoosenAttractions.RemoveAt(removedIdx);
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (ChoosenAttractions.Count + 1 > remIdx)
                {
                    ChoosenAttractions.Insert(targetIdx, droppedData);
                    ChoosenAttractions.RemoveAt(remIdx);
                }
            }
        }

        void listboxRestaurants_Drop(object sender, DragEventArgs e)
        {
            string droppedData = e.Data.GetData(typeof(string)) as string;
            string target = ((ListBoxItem)(sender)).DataContext as string;

            int removedIdx = ChoosenRestaurantsListBox.Items.IndexOf(droppedData);
            int targetIdx = ChoosenRestaurantsListBox.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                ChoosenRestaurants.Insert(targetIdx + 1, droppedData);
                ChoosenRestaurants.RemoveAt(removedIdx);
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (((UserDetailsViewModel)DataContext).Attractions.Count + 1 > remIdx)
                {
                    ChoosenRestaurants.Insert(targetIdx, droppedData);
                    ChoosenRestaurants.RemoveAt(remIdx);
                }
            }
        }

        void listboxHotels_Drop(object sender, DragEventArgs e)
        {
            string droppedData = e.Data.GetData(typeof(string)) as string;
            string target = ((ListBoxItem)(sender)).DataContext as string;

            int removedIdx = ChoosenHotelsListBox.Items.IndexOf(droppedData);
            int targetIdx = ChoosenHotelsListBox.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                ChoosenHotels.Insert(targetIdx + 1, droppedData);
                ChoosenHotels.RemoveAt(removedIdx);
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (ChoosenHotels.Count + 1 > remIdx)
                {
                    ChoosenHotels.Insert(targetIdx, droppedData);
                    ChoosenHotels.RemoveAt(remIdx);
                }
            }
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
                Photo.Source = new BitmapImage(new Uri(op.FileName));
                imageData = File.ReadAllBytes(op.FileName);
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {            
            if (ValidateData()) {
                var newTravelName = TravelName.Text.Trim();
                var newTravelShortDesctiption = TravelShortDescription.Text.Trim();
                var newTravelLongDescription = TravelLongDescription.Text.Trim();
                var newTravelPriceString = TravelPrice.Text.Trim();
                string travelDate = TravelDate.SelectedDate.Value.ToString("s");
                int newTravelPrice;
                int.TryParse(newTravelPriceString, out newTravelPrice);
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
                        End = _toAddress,
                        Date = travelDate
                    };
                    if (ChoosenAttractions.Count > 0)
                    {
                        newTravel.Attractions.AddRange(db.Attractions.Where(el => ChoosenAttractions.Contains(el.Name)));
                    }
                    if (ChoosenHotels.Count > 0)
                    {
                        newTravel.Hotels.AddRange(db.Hotels.Where(el => ChoosenHotels.Contains(el.Name)));
                    }
                    if (ChoosenRestaurants.Count > 0)
                    {
                        newTravel.Restaurants.AddRange(db.Restaurants.Where(el => ChoosenRestaurants.Contains(el.Name)));
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
                        TravelDate.SelectedDate = null;
                        Photo.Source = null;
                        ChoosenAttractions.Clear();
                        ChoosenHotels.Clear();
                        ChoosenRestaurants.Clear();
                        CollectionViewSource.GetDefaultView(ChoosenAttractionsListBox.ItemsSource).Refresh();
                        CollectionViewSource.GetDefaultView(ChoosenHotelsListBox.ItemsSource).Refresh();
                        CollectionViewSource.GetDefaultView(ChoosenRestaurantsListBox.ItemsSource).Refresh();
                        InitializeOptions();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Podaci putovanja nisu u ispravnom obliku. Proverite sva polja i pokušajte ponovo.", "Neuspelo kreiranje", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private bool ValidateData() {
            var bindingExpressionTravelName = TravelName.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionTravelName.UpdateSource();
            if (Validation.GetHasError(TravelName))
            {
                var errors = Validation.GetErrors(TravelName);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionStartLocation = StartLocation.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionStartLocation.UpdateSource();
            if (Validation.GetHasError(StartLocation))
            {
                var errors = Validation.GetErrors(StartLocation);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionEndLocation = EndLocation.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionEndLocation.UpdateSource();
            if (Validation.GetHasError(EndLocation))
            {
                var errors = Validation.GetErrors(EndLocation);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionTravelShortDescription = TravelShortDescription.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionTravelShortDescription.UpdateSource();
            if (Validation.GetHasError(TravelShortDescription))
            {
                var errors = Validation.GetErrors(TravelShortDescription);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionTravelLongDescription = TravelLongDescription.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionTravelLongDescription.UpdateSource();
            if (Validation.GetHasError(TravelLongDescription))
            {
                var errors = Validation.GetErrors(TravelLongDescription);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionTravelPrice = TravelPrice.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionTravelPrice.UpdateSource();
            if (Validation.GetHasError(TravelPrice))
            {
                var errors = Validation.GetErrors(TravelPrice);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionTravelDate = TravelDate.GetBindingExpression(DatePicker.SelectedDateProperty);
            bindingExpressionTravelDate.UpdateSource();
            if (Validation.GetHasError(TravelDate))
            {
                var errors = Validation.GetErrors(TravelDate);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;

        }

        private async void UpdateMap(object sender, RoutedEventArgs e)
        {
            if (StartLocation.Text.Trim() != "" && StartLocation.Text.Trim() != _fromAddress)
            {
                if (_fromAddress != "")
                {
                    mainMap.Children.Clear();
                    if(_toAddress != "")
                    {
                        AddMainPin(_toLocation[0], _toLocation[1]);
                    }
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
                if (_toAddress != "")
                {
                    mainMap.Children.Clear();
                    if(_fromAddress != "")
                    {
                        AddMainPin(_fromLocation[0], _fromLocation[1]);
                    }
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

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
