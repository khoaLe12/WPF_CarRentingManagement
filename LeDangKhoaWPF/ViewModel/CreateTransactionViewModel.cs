using BusinessObject;
using Repositories;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace LeDangKhoaWPF.ViewModel
{
    public interface ICreateTransactionViewModel
    {
        public event EventHandler? OnRefreshTransactionWindow;
        public bool RentingStatus { get; set; }
        public object? Customer { get; set; }
        public object? Car { get; set; }
        public IEnumerable<Customer>? Customers { get; set; }
        public IEnumerable<CarInformation>? Cars { get; set; }
        public ICommand CreateCommand { get; }
        void LoadInformation();
    }
    public class CreateTransactionViewModel : ViewModelBase, ICreateTransactionViewModel
    {
        public event EventHandler? OnRefreshTransactionWindow;
        
        private readonly ICarInformationRepository _carInformationRepository;
        private readonly IRentingTransactionRepository _transactionRepository;
        private readonly ICustomerRepository _customerRepository;

        private DateTime startDate;
        private DateTime endDate;
        private byte? rentingStatus;
        private Customer? customer; 
        private CarInformation? car;
        private IEnumerable<Customer>? customers;
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
        public bool RentingStatus 
        {
            get
            {
                if(rentingStatus == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    rentingStatus = 1;
                }
                else
                {
                    rentingStatus = 0;
                }
                OnPropertyChanged(nameof(RentingStatus));
            }
        }
        public object? Customer 
        { 
            get => customer;
            set
            {
                customer = value as Customer;
                OnPropertyChanged(nameof(Customer));
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
        public IEnumerable<Customer>? Customers {
            get => customers;
            set
            {
                customers = value;
                OnPropertyChanged(nameof(Customers));
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

        public CreateTransactionViewModel(IRentingTransactionRepository rentingTransactionRepository, ICustomerRepository customerRepository, ICarInformationRepository carInformationRepository)
        {
            _customerRepository = customerRepository;
            _transactionRepository = rentingTransactionRepository;
            CreateCommand = new ViewModelCommand(ExecuteCommand, CanExecuteCommand);
            _carInformationRepository = carInformationRepository;
        }

        private bool CanExecuteCommand(object? obj)
        {
            if(customer is null || car is null)
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
            RentingTransaction transaction = new RentingTransaction
            {
                RentingDate = DateTime.UtcNow,
                TotalPrice = (int)(endDate - startDate).TotalDays * car!.CarRentingPricePerDay,
                RentingStatus = rentingStatus,
                CustomerId = customer!.CustomerId
            };
            RentingDetail order = new RentingDetail()
            {
                CarId = car!.CarId,
                StartDate = startDate,
                EndDate = endDate,
                Price = car!.CarRentingPricePerDay,
                RentingTransaction = transaction
            };
            var result = await _transactionRepository.AddNewTransaction(transaction, order);
            if (result)
            {
                OnRefreshTransactionWindow?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Add Successfully");
            }
            else
            {
                MessageBox.Show("Add Fail");
            }
            _transactionRepository.UnTrackEntity(transaction);
        }

        public async void LoadInformation()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();
            var cars = await _carInformationRepository.GetAllCarInformations();
            Customers = customers;
            Cars = cars;
        }
    }
}
