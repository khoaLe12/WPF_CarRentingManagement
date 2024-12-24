using BusinessObject;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using static LeDangKhoaWPF.ViewModel.CustomerViewModel;

namespace LeDangKhoaWPF.ViewModel
{
    public interface ICustomerViewModel
    {
        public event EventHandler<ViewDatailArgs>? ViewDatail;
        public string? DisplayName { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? BirthDate { get; set; }
        public string? Password { get; set; }
        public IEnumerable<RentingTransaction>? RentingTransactions { get; set; }
        public ICommand ViewDetailCommand { get; }
        public ICommand SaveCommand { get; }
    }

    public class CustomerViewModel : ViewModelBase, ICustomerViewModel
    {
        // Event for viewing transaction detail
        public event EventHandler<ViewDatailArgs>? ViewDatail;
        public class ViewDatailArgs : EventArgs
        {
            internal int transactionId;
        }
        //

        private readonly ICustomerRepository _customerRepository;

        private readonly IRentingTransactionRepository _transactionRepository;

        private Session session;

        private string? displayName;
        private string? name;
        private string? phone;
        private string? email;
        private DateTime birthDate;
        private string? password;
        private IEnumerable<RentingTransaction>? rentingTransactions;
        private RentingTransaction? selectedTransaction = null;

        public string? DisplayName
        {
            get => displayName;
            set
            {
                if(value is null)
                {
                    displayName = "Undefined";
                }
                else
                {
                    displayName = "Hello " + value;
                }
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public string? Name 
        { 
            get => name;
            set 
            { 
                name = value ?? "Undefined";
                OnPropertyChanged(nameof(Name));
            }
        }

        public string? Phone 
        { 
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        public string? Email 
        { 
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string? BirthDate 
        { 
            get => birthDate.ToString();
            set
            {
                if (value.IsNullOrEmpty())
                {
                    birthDate = new DateTime();
                }
                else
                {
                    birthDate = DateTime.Parse(value!);
                }
                OnPropertyChanged(nameof(BirthDate));
            }
        }

        public string? Password 
        { 
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
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

        public object? SelectedTransaction 
        {
            get => selectedTransaction;
            set
            {
                selectedTransaction = value as RentingTransaction;
                OnPropertyChanged(nameof(SelectedTransaction));
            }
        } 

        public ICommand ViewDetailCommand { get; }

        public ICommand SaveCommand { get; }

        public CustomerViewModel(ICustomerRepository customerRepository, IRentingTransactionRepository rentingTransactionRepository, Session session)
        {
            this.session = session;
            _transactionRepository = rentingTransactionRepository;
            _customerRepository = customerRepository;
            LoadInformation(session.Id);
            ViewDetailCommand = new ViewModelCommand(ExecuteViewCommand, CanExecuteViewCommand);
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand);
        }

        private async void ExecuteSaveCommand(object? obj)
        {
            var user = await _customerRepository.FindUserById(session.Id);
            if(user is null)
            {
                MessageBox.Show("User not found");
            }
            else if (email is null)
            {
                MessageBox.Show("Invalid Data");
            }
            else
            {
                user.CustomerName = name;
                user.Email = email;
                user.Telephone = phone;
                user.CustomerBirthday = birthDate;
                if(password is not null)
                {
                    user.Password = password;
                    Password = null;
                }
                if(await _customerRepository.UpdateUserAsync(user))
                {
                    MessageBox.Show("Update Successfully");
                    DisplayName = user.CustomerName;
                }
                else
                {
                    MessageBox.Show("Update Fail");
                }
            }
        }

        private bool CanExecuteViewCommand(object? obj)
        {
            bool validate;
            if(SelectedTransaction is null)
            {
                validate = false;
            }
            else
            {
                validate = true;
            }
            return validate;
        }

        private void ExecuteViewCommand(object? obj)
        {
            var transaction = selectedTransaction!;
            ViewDatail?.Invoke(this, new ViewDatailArgs { transactionId = transaction.RentingTransationId });
    }

        private async void LoadInformation(int id)
        {
            var existedUser = await _customerRepository.FindUserWithTransactionsById(id);
            if (existedUser is not null)
            {
                DisplayName = existedUser.CustomerName;
                Name = existedUser.CustomerName;
                Phone = existedUser.Telephone;
                Email = existedUser.Email;
                BirthDate = existedUser.CustomerBirthday.ToString();
                RentingTransactions = existedUser.RentingTransactions;
            }
        }
    }
}
