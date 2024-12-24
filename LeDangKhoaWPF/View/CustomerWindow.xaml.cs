using LeDangKhoaWPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace LeDangKhoaWPF.View
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private readonly IServiceProvider _provider;
        public CustomerWindow(ICustomerViewModel customerViewModel, IServiceProvider provider)
        {
            _provider = provider;
            InitializeComponent();
            DataContext = customerViewModel;
            customerViewModel.ViewDatail += CustomerViewModel_ViewDatail;
        }

        private void CustomerViewModel_ViewDatail(object? sender, CustomerViewModel.ViewDatailArgs e)
        {
            using (var scope = _provider.CreateScope())
            {
                var detailWindow = scope.ServiceProvider.GetService<TransactionDetail>();
                detailWindow!.SetTransactionDetailId(e.transactionId);
                detailWindow!.Show();
            }
        }

        //var name = Thread.CurrentPrincipal?.Identity?.Name;
    }
}
