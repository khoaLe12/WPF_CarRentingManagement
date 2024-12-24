using LeDangKhoaWPF.CustomControls;
using LeDangKhoaWPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
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
    /// Interaction logic for CreateCustomer.xaml
    /// </summary>
    public partial class CreateCustomer : Window
    {
        public CreateCustomer(ICreateCustomerViewModel createCustomerVM)
        {
            InitializeComponent();
            DataContext = createCustomerVM;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            SetBackInput();
        }

        private void SetBackInput()
        {
            foreach (var control in GetAllChildren(this))
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Clear();
                }
                else if (control is CheckBox)
                {
                    ((CheckBox)control).IsChecked = false;
                }
                else if (control is BindablePasswordBox)
                {
                    ((BindablePasswordBox)control).Clear();
                }
            }
        }

        private IEnumerable<Control?> GetAllChildren(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Control)
                    yield return child as Control;

                foreach (var descendant in GetAllChildren(child))
                    yield return descendant;
            }
        }
    }
}
