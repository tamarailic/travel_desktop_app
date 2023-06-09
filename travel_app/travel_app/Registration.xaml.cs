﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using travel_app.MVVM.ViewModel;
using travel_app.Store;
using WPFCustomMessageBox;

namespace travel_app
{
    public partial class Registration : Window
    {
        public string Username { get; set; }

        public Registration()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text.Trim();
            var password = PasswordBox.Password.Trim();
            var repassword = RePasswordBox.Password.Trim();
            ComboBoxItem roleItem = (ComboBoxItem)RoleComboBox.SelectedItem;
            string role = roleItem.Name.ToString();

            var bindingExpressionUsername = UsernameTextBox.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionUsername.UpdateSource();

            var bindingExpressionPassword = PasswordBox.GetBindingExpression(PasswordBox.PasswordCharProperty);
            bindingExpressionUsername.UpdateSource();

            if (Validation.GetHasError(UsernameTextBox))
            {
                var errors = Validation.GetErrors(UsernameTextBox);
                CustomMessageBox.ShowOK(errors[0].ErrorContent.ToString(), "Greška", "U redu");
            }
            else if (((password ?? "").ToString()).Length < 5)
            {
                CustomMessageBox.ShowOK("Lozinka mora da ima bar 5 karaktera.", "Greška", "U redu");
            } else if ((password ?? "").ToString().Count(c => !char.IsLetter(c)) == 0) {
                CustomMessageBox.ShowOK("Lozinka mora da ima bar 1 specijalni karaktera ili broj.", "Greška", "U redu");
            }
            else if (!password.Equals(repassword))
            {
                CustomMessageBox.ShowOK("Lozinke se ne poklapaju.", "Greška", "U redu");
            }
            else {
                using (var db = new TravelContext())
                {
                    if (db.Users.Where(user => user.Email == username).Any())
                    {
                        CustomMessageBox.ShowOK("Podaci nisu uneti u ispravnom obliku", "Neuspela registracija", "U redu");
                        return;
                    }
                    User newUser = new User
                    {
                        Email = username,
                        Password = password,
                        Role = role,
                        Pro = false
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    CustomMessageBox.ShowOK("Uspešna registracija", "Uspešno", "U redu");
                    NavigationStore navigationStore = new NavigationStore();
                    if (newUser.Role.Equals("user"))
                    {
                        UserHomeViewModel userHomeViewModel = new UserHomeViewModel(navigationStore);
                        navigationStore.CurrentViewModel = userHomeViewModel;
                        UserMainWindow userMainWindow = new UserMainWindow()
                        {
                            DataContext = new UserMainViewModel(navigationStore)
                        };
                        UserMainWindow.LogedInUser = newUser;
                        var currentWindow = Application.Current.MainWindow;
                        Application.Current.MainWindow = userMainWindow;
                        userMainWindow.Show();
                        currentWindow.Close();
                    }
                    else
                    {
                        HomeViewModel homeViewModel = new HomeViewModel(navigationStore);
                        navigationStore.CurrentViewModel = homeViewModel;
                        MainWindow mainWindow = new MainWindow()
                        {
                            DataContext = new MainViewModel(navigationStore)
                        };
                        MainWindow.LogedInUser = newUser;
                        var currentWindow = Application.Current.MainWindow;
                        Application.Current.MainWindow = mainWindow;
                        mainWindow.Show();
                        currentWindow.Close();
                    }
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            var currentWindow = Application.Current.MainWindow;
            Application.Current.MainWindow = login;
            login.Show();
            currentWindow.Close();
        }
    }

    

    public class MinimumLengthValidationRule : ValidationRule
    {
        public int MinimumLength { get; set; }
        public string Message { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (((value ?? "").ToString()).Length < MinimumLength)
                return new ValidationResult(false, Message);
            return ValidationResult.ValidResult;
        }
    }

    public class NoSpecialCharactersOrNumbersValidationRule : ValidationRule
    {
        public string Message { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString();
            if (input.Any(c => !char.IsLetter(c)))
                return ValidationResult.ValidResult;
            return new ValidationResult(false, Message);
            
        }
    }
}
