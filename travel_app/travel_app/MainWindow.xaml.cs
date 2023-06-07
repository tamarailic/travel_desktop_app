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

namespace travel_app
{
    public partial class MainWindow : Window
    {

        public static User? LogedInUser { get; set; }   

        public MainWindow()
        {
            InitializeComponent();
        }

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
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }

        public void doThings(string param)
        {
            //btnOK.Background = new SolidColorBrush(Color.FromRgb(32, 64, 128));
            Title = param;
        }

    }
}
