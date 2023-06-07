using System;
using System.Collections.Generic;
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
    public partial class SettingsView : UserControl
    {
        public bool Pro { get; set; }

        public SettingsView()
        {
            InitializeComponent();
            this.DataContext = MainWindow.LogedInUser;
            Pro = MainWindow.LogedInUser.Pro;
        }

        private void Pro_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;

            if ((bool)checkBox.IsChecked) {
                var result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da pređete u profesionalan režim?", "Potvrda profesionalnog režima","Da", "Ne");
                // Check the result value
                if (result == MessageBoxResult.Yes)
                {
                    using (var db = new TravelContext())
                    {
                        var user = db.Users.FirstOrDefault(u => u.Id == MainWindow.LogedInUser.Id);
                        if (user != null)
                        {
                            user.Pro = true;
                            MainWindow.LogedInUser.Pro = true;
                            db.SaveChanges();
                        }
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    checkBox.IsChecked = false;
                }
            }
            else
            {
                using (var db = new TravelContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.Id == MainWindow.LogedInUser.Id);
                    if (user != null)
                    {
                        user.Pro = false;
                        MainWindow.LogedInUser.Pro = false;
                        db.SaveChanges();
                    }
                }
            }
            
        }
    }
}
