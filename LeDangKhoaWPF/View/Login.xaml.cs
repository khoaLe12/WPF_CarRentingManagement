using LeDangKhoaWPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly IServiceProvider _provider;
        public Login(ILoginViewModel loginVM, IServiceProvider provider)
        {
            _provider = provider;
            InitializeComponent();
            DataContext = loginVM;
            loginVM.OnCloseWindow += LoginVM_OnCloseWindow;
        }

        private void LoginVM_OnCloseWindow(object? sender, LoginViewModel.OnCloseWindowArgs e)
        {
            if (e.IsAdmin)
            {
                var adminWindow = _provider.GetService<AdminWindow>();
                adminWindow!.Show();
                Close();
            }
            else
            {
                var customerWindow = _provider.GetService<CustomerWindow>();
                customerWindow!.Show();
                Close();
            }
        }
    }
}
