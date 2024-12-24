using LeDangKhoaWPF.ViewModel;
using System.Windows;

namespace LeDangKhoaWPF.View
{
    /// <summary>
    /// Interaction logic for TransactionDetail.xaml
    /// </summary>
    public partial class TransactionDetail : Window
    {
        private ITransactionDetailViewModel _transactionDetailViewModel;
        public TransactionDetail(ITransactionDetailViewModel transactionDetailViewModel)
        {
            InitializeComponent();
            _transactionDetailViewModel = transactionDetailViewModel;
            DataContext = _transactionDetailViewModel;
        }

        public void SetTransactionDetailId(int id)
        {
            _transactionDetailViewModel.SetTransactionDetailId(id);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}
