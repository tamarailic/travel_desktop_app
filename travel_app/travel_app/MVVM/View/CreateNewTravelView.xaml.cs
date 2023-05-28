using Microsoft.Win32;
using System;
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
    public partial class CreateNewTravelView : UserControl
    {
        public CreateNewTravelView()
        {
            InitializeComponent();
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
    }
}
