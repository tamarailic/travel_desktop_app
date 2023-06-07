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
using WPFCustomMessageBox;

namespace travel_app.MVVM.View
{
    /// <summary>
    /// Interaction logic for EditTravelView.xaml
    /// </summary>
    public partial class EditTravelView : UserControl
    {
        public string TravelsName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string DescLong { get; set; }
        public string DescShort { get; set; }
        public string Price { get; set; }
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
                CustomMessageBox.ShowOK(errors[0].ErrorContent.ToString(), "Greška","U redu");
                return false;
            }

            var bindingExpressionStartLocation = StartLocation.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionStartLocation.UpdateSource();
            if (Validation.GetHasError(StartLocation))
            {
                var errors = Validation.GetErrors(StartLocation);
                CustomMessageBox.ShowOK(errors[0].ErrorContent.ToString(), "Greška", "U redu");
                return false;
            }

            var bindingExpressionEndLocation = EndLocation.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionEndLocation.UpdateSource();
            if (Validation.GetHasError(EndLocation))
            {
                var errors = Validation.GetErrors(EndLocation);
                CustomMessageBox.ShowOK(errors[0].ErrorContent.ToString(), "Greška", "U redu");
                return false;
            }

            var bindingExpressionTravelShortDescription = TravelShortDescription.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionTravelShortDescription.UpdateSource();
            if (Validation.GetHasError(TravelShortDescription))
            {
                var errors = Validation.GetErrors(TravelShortDescription);
                CustomMessageBox.ShowOK(errors[0].ErrorContent.ToString(), "Greška", "U redu");
                return false;
            }

            var bindingExpressionTravelLongDescription = TravelDescription.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionTravelLongDescription.UpdateSource();
            if (Validation.GetHasError(TravelDescription))
            {
                var errors = Validation.GetErrors(TravelDescription);
                CustomMessageBox.ShowOK(errors[0].ErrorContent.ToString(), "Greška", "U redu");
                return false;
            }

            var bindingExpressionTravelPrice = TravelPrice.GetBindingExpression(TextBox.TextProperty);
            bindingExpressionTravelPrice.UpdateSource();
            if (Validation.GetHasError(TravelPrice))
            {
                var errors = Validation.GetErrors(TravelPrice);
                CustomMessageBox.ShowOK(errors[0].ErrorContent.ToString(), "Greška", "U redu");
                return false;
            }

            var bindingExpressionTravelDate = TravelDate.GetBindingExpression(DatePicker.SelectedDateProperty);
            bindingExpressionTravelDate.UpdateSource();
            if (Validation.GetHasError(TravelDate))
            {
                var errors = Validation.GetErrors(TravelDate);
                CustomMessageBox.ShowOK(errors[0].ErrorContent.ToString(), "Greška", "U redu");
                return false;
            }

            return true;

        }
    }
}
