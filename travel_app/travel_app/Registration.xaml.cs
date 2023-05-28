using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using travel_app.MVVM.Model;

namespace travel_app
{
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text.Trim();
            var password = PasswordBox.Password.Trim();
            var repassword = RePasswordBox.Password.Trim();

            if(username != "" && password != "" && password == repassword)
            {
                using (var db = new TravelContext())
                {
                    if(db.Users.Where(user => user.Email == username).Any())
                    {
                        Trace.WriteLine("User already exists");
                        return;
                    }
                    User newUser = new User
                    {
                        Email = username,
                        Password = password,
                        Role = "agent",
                        Pro = false
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    Trace.WriteLine($"Sucessfull registration {newUser.Email}");
                }
            }
        }
    }
}
