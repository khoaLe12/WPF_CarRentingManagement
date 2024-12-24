using BusinessObject;
using LeDangKhoaWPF.CustomControls;
using LeDangKhoaWPF.ViewModel;
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
using System.Windows.Shapes;

namespace LeDangKhoaWPF.View
{
    /// <summary>
    /// Interaction logic for UpdateCustomer.xaml
    /// </summary>
    public partial class UpdateCustomer : Window
    {
        private readonly IUpdateCustomerViewModel _updateCustomerVM;
        public UpdateCustomer(IUpdateCustomerViewModel updateCustomerVM)
        {
            InitializeComponent();
            DataContext = updateCustomerVM;
            _updateCustomerVM = updateCustomerVM;
        }

        public void SetInformation(Customer customer)
        {
            _updateCustomerVM.SetInformation(customer);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}
