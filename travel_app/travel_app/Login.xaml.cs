
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using travel_app.MVVM.Model;

namespace travel_app
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

            //InitializeAttractionTypes();
            //InitializeRestaurentTypes();
            //ListAll();
        }

        private void InitializeAttractionTypes()
        {
            using(var db = new TravelContext())
            {
                var attractionTypes = new List<AttractionTypes>() {
                                                            new AttractionTypes(){ Name = "Spomenik" },
                                                            new AttractionTypes(){ Name = "Muzej" },
                                                            new AttractionTypes(){ Name = "Prirodno dobro" },
                                                            new AttractionTypes(){ Name = "Ostalo" },
                                                            };
                //db.Database.ExecuteSqlCommand("DELETE FROM [AttractionTypes]");
                db.AttractionTypes.AddRange(attractionTypes);
                db.SaveChanges();
            }
        }

        private void InitializeRestaurentTypes()
        {
            using(var db = new TravelContext())
            {
                var restaurentTypes = new List<RestaurantTypes>() {
                                                            new RestaurantTypes(){ Name = "Standardni" },
                                                            new RestaurantTypes(){ Name = "Meksički" },
                                                            new RestaurantTypes(){ Name = "Veganski" },
                                                            };
                //db.Database.ExecuteSqlCommand("DELETE FROM [RestaurantTypes]");
                db.RestaurantTypes.AddRange(restaurentTypes);
                db.SaveChanges();
            }
        }

        private void ListAll()
        {

            Trace.WriteLine("*****");

            using (var db = new TravelContext())
            {

                var elements = db.AttractionTypes.ToList();
                foreach(var element in elements)
                {
                    Trace.WriteLine(element);
                }
            }

            Trace.WriteLine("*****");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            
            using (var db = new TravelContext())
            {
                User? user = db.Users.Where(user => user.Email == username).SingleOrDefault();
                if(user != null && user.Password == password)
                {
                    MainWindow mainWindow = new MainWindow();
                    MainWindow.LogedInUser = user;
                    var currentWindow = Application.Current.MainWindow;
                    Application.Current.MainWindow = mainWindow;
                    mainWindow.Show();
                    currentWindow.Close();
                }
                else
                {
                    MessageBox.Show("Korisničko ime ili lozinka nisu ispravi. Pokušajte ponovo");
                }
            }
        }
    }
}
