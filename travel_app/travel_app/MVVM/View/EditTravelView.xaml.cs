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

namespace travel_app.MVVM.View
{
    /// <summary>
    /// Interaction logic for EditTravelView.xaml
    /// </summary>
    public partial class EditTravelView : UserControl
    {
        public EditTravelView()
        {
            InitializeComponent();
        }

        private bool ValidateData()
        {
            var bindingExpressionTravelName = TravelName.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionTravelName.UpdateSource();
            if (Validation.GetHasError(TravelName))
            {
                var errors = Validation.GetErrors(TravelName);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionStartLocation = StartLocation.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionStartLocation.UpdateSource();
            if (Validation.GetHasError(StartLocation))
            {
                var errors = Validation.GetErrors(StartLocation);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionEndLocation = EndLocation.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionEndLocation.UpdateSource();
            if (Validation.GetHasError(EndLocation))
            {
                var errors = Validation.GetErrors(EndLocation);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionTravelShortDescription = TravelShortDescription.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionTravelShortDescription.UpdateSource();
            if (Validation.GetHasError(TravelShortDescription))
            {
                var errors = Validation.GetErrors(TravelShortDescription);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionTravelLongDescription = TravelDescription.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionTravelLongDescription.UpdateSource();
            if (Validation.GetHasError(TravelDescription))
            {
                var errors = Validation.GetErrors(TravelDescription);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionTravelPrice = TravelPrice.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionTravelPrice.UpdateSource();
            if (Validation.GetHasError(TravelPrice))
            {
                var errors = Validation.GetErrors(TravelPrice);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var bindingExpressionTravelDate = TravelDate.GetBindingExpression(DatePicker.SelectedDateProperty);
            bindingExpressionTravelDate.UpdateSource();
            if (Validation.GetHasError(TravelDate))
            {
                var errors = Validation.GetErrors(TravelDate);
                MessageBox.Show(errors[0].ErrorContent.ToString(), "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;

        }
    }
}
