using BusinessObject;
using LeDangKhoaWPF.ViewModel;
using System.Windows;


namespace LeDangKhoaWPF.View
{
    /// <summary>
    /// Interaction logic for UpdateCar.xaml
    /// </summary>
    public partial class UpdateCar : Window
    {
        private readonly IUpdateCarViewModel _updateCarVM;
        public UpdateCar(IUpdateCarViewModel updateCarVM)
        {
            InitializeComponent();
            DataContext = updateCarVM;
            _updateCarVM = updateCarVM;
        }

        public void SetInformation(CarInformation car)
        {
            _updateCarVM.SetInformation(car);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
    }
}
