using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace travel_app.MVVM.View
{
    /// <summary>
    /// Interaction logic for CreateNewTravelView.xaml
    /// </summary>
    public partial class CreateNewTravelView : Window
    {
        public CreateNewTravelView()
        {
            InitializeComponent();
        }

        private byte[] imageData; // A variable to store the image data

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of OpenFileDialog
            var openFileDialog = new OpenFileDialog();
            // Set the filter and title for the dialog
            openFileDialog.Filter = "Image files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";
            openFileDialog.Title = "Select an image";
            // Show the dialog and get the result
            var result = openFileDialog.ShowDialog();
            // If the result is true, the user has selected a file
            if (result == true)
            {
                // Get the file name and path
                var fileName = openFileDialog.FileName;
                // Load the image file into a BitmapImage object
                var bitmapImage = new BitmapImage(new Uri(fileName));
                // Set the source of the Image control to the BitmapImage object
                ImageControl.Source = bitmapImage;
                // Read the image file into a byte array
                imageData = File.ReadAllBytes(fileName);
            }
        }

        /*
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of your entity data model
            var entity = new MyEntityDataModel();
            // Create an instance of your table class
            var table = new MyTable();
            // Set the properties of the table class with the form data
            table.Name = NameTextBox.Text;
            table.Description = DescriptionTextBox.Text;
            table.Price = PriceTextBox.Text;
            table.Image = imageData; // Set the image data from the variable
                                     // Add the table class to the entity data model
            entity.MyTables.Add(table);
            // Save the changes to the database
            entity.SaveChanges();
        }
        */
    }
}
