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
    }
}
