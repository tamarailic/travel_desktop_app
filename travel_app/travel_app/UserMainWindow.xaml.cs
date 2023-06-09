﻿using System;
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
using System.Windows.Shapes;
using travel_app.MVVM.Model;
using WPFCustomMessageBox;

namespace travel_app
{
    /// <summary>
    /// Interaction logic for UserMainWindow.xaml
    /// </summary>
    public partial class UserMainWindow : Window
    {
        public UserMainWindow()
        {
            InitializeComponent();
        }

        public static User? LogedInUser { get; set; }


        private void LogOut(object sender, RoutedEventArgs e)
        {
            var result = MessageBoxResult.Yes;
            if (!LogedInUser.Pro)
            {
                result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da se odjavite sa naloga?", "Potvrdite odjavu", "Da", "Ne");
            }
            if (result == MessageBoxResult.Yes)
            {
                LogedInUser = null;
                Login login = new Login();
                var currentWindow = Application.Current.MainWindow;
                Application.Current.MainWindow = login;
                login.Show();
                currentWindow.Close();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
