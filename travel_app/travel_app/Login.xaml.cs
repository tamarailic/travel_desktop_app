
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;


namespace travel_app
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            // Add a click event handler to the LoginButton
            LoginButton.Click += LoginButton_Click;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the username and password from the text boxes
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            // Create an object with the username and password
            //var user = new User(username, password);
            Console.WriteLine("user");
            // Put the object in DbContext
            //using (var db = new UserContext())
            //{
            //    db.User.Add(user);
            //    db.SaveChanges();
            //}
        }

    }
}
