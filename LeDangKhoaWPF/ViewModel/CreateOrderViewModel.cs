using BusinessObject;
using Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LeDangKhoaWPF.ViewModel
{ 
    public interface ICreateOrderViewModel
    {
        public event EventHandler? OnRefreshTransactionWindow;
        public object? Car { get; set; }
        public IEnumerable<CarInformation>? Cars { get; set; }
        public ICommand CreateCommand { get; }
        void LoadInformation(int transactionId);
    }
    public class CreateOrderViewModel : ViewModelBase, ICreateOrderViewModel
    {
        public event EventHandler? OnRefreshTransactionWindow;

        private readonly ICarInformationRepository _carInformationRepository;
        private readonly IRentingTransactionRepository _transactionRepository;

        private int transactionId;
        private DateTime startDate;
        private DateTime endDate;
        private CarInformation? car;
        private IEnumerable<CarInformation>? cars;

        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        public object? Car
        {
            get => car;
            set
            {
                car = value as CarInformation;
                OnPropertyChanged(nameof(Car));
            }
        }
        public IEnumerable<CarInformation>? Cars
        {
            get => cars;
            set
            {
                cars = value;
                OnPropertyChanged(nameof(Cars));
            }
        }
        public ICommand CreateCommand { get; }

        public CreateOrderViewModel(ICarInformationRepository carInformationRepository, IRentingTransactionRepository transactionRepository)
        {
            _carInformationRepository = carInformationRepository;
            _transactionRepository = transactionRepository;
            CreateCommand = new ViewModelCommand(ExecuteCommand, CanExecuteCommand);
        }

        private bool CanExecuteCommand(object? obj)
        {
            if(car is null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private async void ExecuteCommand(object? obj)
        {
            RentingDetail order = new RentingDetail
            {
                RentingTransactionId = transactionId,
                CarId = car!.CarId,
                StartDate = startDate,
                EndDate = endDate,
                Price = car!.CarRentingPricePerDay
            };
            var price = (int)(endDate - startDate).TotalDays * car!.CarRentingPricePerDay;
            var result = await _transactionRepository.AddNewOrder(order, price);
            if (result)
            {
                OnRefreshTransactionWindow?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Add Successfully");
            }
            else
            {
                MessageBox.Show("Add Fail");
            }
        }

        public async void LoadInformation(int transactionId)
        {
            this.transactionId = transactionId;
            var cars = await _carInformationRepository.GetAllCarInformations();
            Cars = cars;
        }
    }
}
