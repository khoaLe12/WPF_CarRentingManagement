using BusinessObject;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Windows;
using System.Windows.Input;

namespace LeDangKhoaWPF.ViewModel
{
    public interface ICreateCustomerViewModel
    {
        public event EventHandler? OnRefreshCustomerWindow;
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? BirthDate { get; set; }
        public string? Password { get; set; }
        public bool Status { get; set; }
        public ICommand CreateCommand { get; }
    }

    public class CreateCustomerViewModel : ViewModelBase, ICreateCustomerViewModel
    {
        public event EventHandler? OnRefreshCustomerWindow;

        private readonly IConfiguration _configuration;
        private readonly ICustomerRepository _customerRepository;

        private string? name;
        private string? phone;
        private string? email;
        private DateTime birthDate;
        private string? password;
        private byte? status;

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
        public bool Status 
        {
            get
            {
                if(status is 1)
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
                    status = 1;
                }
                else
                {
                    status = 0;
                }
                OnPropertyChanged(nameof(Status));
            }
        }
        public ICommand CreateCommand { get; }

        public CreateCustomerViewModel(ICustomerRepository customerRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _customerRepository = customerRepository;
            CreateCommand = new ViewModelCommand(ExecuteCreateCommand, CanExecuteCreateCommand);
        }

        private bool CanExecuteCreateCommand(object? obj)
        {
            if(email.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private async void ExecuteCreateCommand(object? obj)
        {
            var existedUser = await _customerRepository.GetUserByEmail(email!);
            var admin = _configuration.GetSection("Admin");
            if (email!.Equals(admin["email"]))
            {
                MessageBox.Show("Email already existed");
            }
            else if (existedUser is not null)
            {
                MessageBox.Show("Email already existed");
            }
            else
            {
                var customer = new Customer()
                {
                    CustomerName = name,
                    Email = email!,
                    Telephone = phone,
                    CustomerBirthday = birthDate,
                    Password = password,
                    CustomerStatus = status
                };
                var result = await _customerRepository.AddNewCustomer(customer);
                if (result)
                {
                    OnRefreshCustomerWindow?.Invoke(this, EventArgs.Empty);
                    MessageBox.Show("Create Successfully");
                }
                else
                {
                    MessageBox.Show("Create Fail");
                }
            }
        }
    }
}
