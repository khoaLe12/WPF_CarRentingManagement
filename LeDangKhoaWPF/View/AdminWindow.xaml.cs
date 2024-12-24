using BusinessObject;
using LeDangKhoaWPF.ViewModel;
using System;
using System.Linq;
using System.Windows;

namespace LeDangKhoaWPF.View
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow(IAdminViewModel adminViewModel)
        {
            InitializeComponent();
            DataContext = adminViewModel;
        }

        private void CUstomer_Filter(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CustomerList.Items.Filter = CustomerFilterMethod;
        }

        private bool CustomerFilterMethod(object obj)
        {
            var customer = obj as Customer;

            return customer!.CustomerName!.Contains(FilterTextBox.Text, StringComparison.OrdinalIgnoreCase) 
                || customer!.Email!.Contains(FilterTextBox.Text, StringComparison.OrdinalIgnoreCase);
        }

        private void Car_Filter(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CarList.Items.Filter = CarFilterMethod;
        }

        private bool CarFilterMethod(object obj)
        {
            var car = obj as CarInformation;

            return car!.CarName.Contains(CarFilterTextBox.Text, StringComparison.OrdinalIgnoreCase)
                || car!.CarId.ToString().Contains(CarFilterTextBox.Text, StringComparison.OrdinalIgnoreCase);
        }

        private void Transaction_Filter(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TransactionList.Items.Filter = TransactionFilterMethod;
        }

        private bool TransactionFilterMethod(object obj)
        {
            var transaction = obj as RentingTransaction;

            return transaction!.RentingTransationId.ToString().Contains(TransactionFilterTextBox.Text, StringComparison.OrdinalIgnoreCase);
        }
    }
}
