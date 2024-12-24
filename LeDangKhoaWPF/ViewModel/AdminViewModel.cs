using BusinessObject;
using LeDangKhoaWPF.View;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace LeDangKhoaWPF.ViewModel
{
    public interface IAdminViewModel
    {
        public string? DisplayName { get; set; }
        public IEnumerable<Customer>? Customers { get; set; }
        public IEnumerable<CarInformation>? Cars { get; set; }
        public IEnumerable<RentingTransaction>? RentingTransactions { get; set; }

        public object? SelectedCustomer { get; set; }
        public object? SelectedCar { get; set; }
        public object? SelectedTransaction { get; set; }

        // For Customer
        public ICommand CreateCustomerCommand { get; }
        public ICommand UpdateCustomerCommand { get; }
        public ICommand DeleteCustomerCommand { get; }


        // For Car Information
        public ICommand CreateCarCommand { get; }
        public ICommand UpdateCarCommand { get; }
        public ICommand DeleteCarCommand { get; }


        // For Renting Transaction
        public ICommand CreateTransactionCommand { get; }
        public ICommand CreateOrderCommand { get; }
        public ICommand UpdateTransactionCommand { get; }
        public ICommand DeleteTransactionCommand { get; }
    }

    public class AdminViewModel : ViewModelBase, IAdminViewModel
    {
        private readonly IServiceProvider _provider;
        private readonly IRentingTransactionRepository _transactionRepository;
        private readonly ICarInformationRepository _carInformationRepository;
        private readonly ICustomerRepository _customerRepository;

        private string? displayName;
        private IEnumerable<Customer>? customers;
        private IEnumerable<CarInformation>? cars;
        private IEnumerable<RentingTransaction>? rentingTransactions;
        private Customer? selectedCustomer;
        private CarInformation? selectedCar;
        private RentingTransaction? selectedTransaction;

        public string? DisplayName 
        { 
            get => displayName;
            set
            {
                displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        public IEnumerable<Customer>? Customers 
        { 
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
        public IEnumerable<RentingTransaction>? RentingTransactions 
        { 
            get => rentingTransactions;
            set
            {
                rentingTransactions = value;
                OnPropertyChanged(nameof(RentingTransactions));
            }
        }
        public object? SelectedCustomer 
        { 
            get => selectedCustomer;
            set
            {
                selectedCustomer = value as Customer;
                OnPropertyChanged(nameof(SelectedCustomer));
            } 
        }
        public object? SelectedCar 
        { 
            get => selectedCar;
            set
            {
                selectedCar = value as CarInformation;
                OnPropertyChanged(nameof(SelectedCar));
            }
        }
        public object? SelectedTransaction 
        { 
            get => selectedTransaction;
            set
            {
                selectedTransaction = value as RentingTransaction;
                OnPropertyChanged(nameof(SelectedTransaction));
            }
        }


        // For Customer
        public ICommand CreateCustomerCommand { get; }
        public ICommand UpdateCustomerCommand { get; }
        public ICommand DeleteCustomerCommand { get; }


        // For Car Information
        public ICommand CreateCarCommand { get; }
        public ICommand UpdateCarCommand { get; }
        public ICommand DeleteCarCommand { get; }


        // For Renting Transaction
        public ICommand CreateTransactionCommand { get; }
        public ICommand CreateOrderCommand { get; }
        public ICommand UpdateTransactionCommand { get; }
        public ICommand DeleteTransactionCommand { get; }


        public AdminViewModel(Session session, 
            ICustomerRepository customerRepository, 
            ICarInformationRepository carInformationRepository, 
            IRentingTransactionRepository transactionRepository, 
            IServiceProvider provider,
            ICreateCustomerViewModel createCustomerVM,
            IUpdateCustomerViewModel updateCustomerVM,
            ICreateCarViewModel createCarVM,
            IUpdateCarViewModel updateCarVM,
            ICreateTransactionViewModel createTransactionVM,
            IUpdateTransactionViewModel updateTransactionVM,
            ICreateOrderViewModel createOrderVM,
            IUpdateOrderViewModel updateOrderVM)
        {
            _provider = provider;
            _transactionRepository = transactionRepository;
            _carInformationRepository = carInformationRepository;
            _customerRepository = customerRepository;
            if (session.Name is null)
            {
                DisplayName = "Undefined";
            }
            else
            {
                DisplayName = "Hello " + session.Name;
            }

            LoadInformation();

            #region Event Listeners
            createCustomerVM.OnRefreshCustomerWindow += async (sender, e) =>
            {
                var customers = await _customerRepository.GetAllCustomersAsync();
                Customers = customers;
            };
            updateCustomerVM.OnRefreshCustomerWindow += async (sender, e) =>
            {
                var customers = await _customerRepository.GetAllCustomersAsync();
                Customers = customers;
            };

            createCarVM.OnRefreshCarWindow += async (sender, e) =>
            {
                var cars = await _carInformationRepository.GetAllCarInformations();
                Cars = cars;
            };
            updateCarVM.OnRefreshCarWindow += async (sender, e) =>
            {
                var cars = await _carInformationRepository.GetAllCarInformations();
                Cars = cars;
            };

            createTransactionVM.OnRefreshTransactionWindow += async (sender, e) =>
            {
                var transactions = await _transactionRepository.GetAllRentingTransactions();
                RentingTransactions = transactions;
            };
            createOrderVM.OnRefreshTransactionWindow += async (sender, e) =>
            {
                var transactions = await _transactionRepository.GetAllRentingTransactions();
                RentingTransactions = transactions;
            };
            #endregion

            #region Register Command
            UpdateCustomerCommand = new ViewModelCommand(ExecuteUpdateCustomerCommand, CanExecuteCustomerCommand);
            DeleteCustomerCommand = new ViewModelCommand(ExecuteDeleteCustomerCommand, CanExecuteCustomerCommand);
            CreateCustomerCommand = new ViewModelCommand(ExecuteCreateCustomerCommand);

            CreateCarCommand = new ViewModelCommand(ExecuteCreateCarCommand);
            UpdateCarCommand = new ViewModelCommand(ExecuteUpdateCarCommand, CanExecuteCarCommand);
            DeleteCarCommand = new ViewModelCommand(ExecuteDeleteCarCommand, CanExecuteCarCommand);

            CreateTransactionCommand = new ViewModelCommand(ExecuteCreateTransactionCommand);
            CreateOrderCommand = new ViewModelCommand(ExecuteCreateOrderCommand, CanExecuteTransactionCommand);
            UpdateTransactionCommand = new ViewModelCommand(ExecuteUpdateTransactionCommand, CanExecuteTransactionCommand);
            DeleteTransactionCommand = new ViewModelCommand(ExecuteDeleteTransactionCommand, CanExecuteTransactionCommand);
            #endregion
        }

        #region Transaction Command
        private bool CanExecuteTransactionCommand(object? obj)
        {
            bool validate;
            if (SelectedTransaction is null)
            {
                validate = false;
            }
            else
            {
                validate = true;
            }
            return validate;
        }
        private void ExecuteCreateTransactionCommand(object? obj)
        {
            using (var scope = _provider.CreateScope())
            {
                var createTransactionWindow = scope.ServiceProvider.GetService<CreateTransaction>();
                createTransactionWindow!.Show();
            }
        }
        private void ExecuteCreateOrderCommand(object? obj)
        {
            using (var scope = _provider.CreateScope())
            {
                var createOrderWindow = scope.ServiceProvider.GetService<CreateOrder>();
                createOrderWindow!.LoadInformation(selectedTransaction!.RentingTransationId);
                createOrderWindow!.Show();
            }
        }
        private void ExecuteUpdateTransactionCommand(object? obj)
        {
            throw new NotImplementedException();
        }
        private async void ExecuteDeleteTransactionCommand(object? obj)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var result = await _transactionRepository.DeleteTransaction(selectedTransaction!);
                if (result)
                {
                    var transactions = await _transactionRepository.GetAllRentingTransactions();
                    RentingTransactions = transactions;
                    MessageBox.Show("Delete Successfully");
                }
                else
                {
                    MessageBox.Show("Delete Fail");
                }
            }
        }
        #endregion

        #region Car Command
        private bool CanExecuteCarCommand(object? obj)
        {
            bool validate;
            if (SelectedCar is null)
            {
                validate = false;
            }
            else
            {
                validate = true;
            }
            return validate;
        }
        private void ExecuteCreateCarCommand(object? obj)
        {
            using (var scope = _provider.CreateScope())
            {
                var createCarWindow = scope.ServiceProvider.GetService<CreateCar>();
                createCarWindow!.Show();
            }
        }
        private void ExecuteUpdateCarCommand(object? obj)
        {
            using (var scope = _provider.CreateScope())
            {
                var updateCarWindow = scope.ServiceProvider.GetService<UpdateCar>();
                updateCarWindow!.SetInformation(selectedCar!);
                updateCarWindow!.Show();
            }
        }
        private async void ExecuteDeleteCarCommand(object? obj)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var result = await _carInformationRepository.DeleteCar(selectedCar!);
                if (result)
                {
                    var cars = await _carInformationRepository.GetAllCarInformations();
                    Cars = cars;
                    MessageBox.Show("Delete Successfully");
                }
                else
                {
                    MessageBox.Show("Delete Fail");
                }
            }
        }
        #endregion

        #region CUstomer Command
        private bool CanExecuteCustomerCommand(object? obj)
        {
            bool validate;
            if (SelectedCustomer is null)
            {
                validate = false;
            }
            else
            {
                validate = true;
            }
            return validate;
        }
        private void ExecuteCreateCustomerCommand(object? obj)
        {
            using (var scope = _provider.CreateScope())
            {
                var createCustomerWindow = scope.ServiceProvider.GetService<CreateCustomer>();
                createCustomerWindow!.Show();
            }
        }
        private void ExecuteUpdateCustomerCommand(object? obj)
        {
            using (var scope = _provider.CreateScope())
            {
                var updateCustomerWindow = scope.ServiceProvider.GetService<UpdateCustomer>();
                updateCustomerWindow!.SetInformation(selectedCustomer!);
                updateCustomerWindow!.Show();
            }
        }
        private async void ExecuteDeleteCustomerCommand(object? obj)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var result = await _customerRepository.DeleteCustomer(selectedCustomer!);
                if (result)
                {
                    var customers = await _customerRepository.GetAllCustomersAsync();
                    Customers = customers;
                    MessageBox.Show("Delete Successfully");
                }
                else
                {
                    MessageBox.Show("Delete Fail");
                }
            }
        }
        #endregion

        private async void LoadInformation()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();
            var cars = await _carInformationRepository.GetAllCarInformations();
            var transactions = await _transactionRepository.GetAllRentingTransactions();
            Customers = customers;
            Cars = cars;
            RentingTransactions = transactions;
        }
    }
}
