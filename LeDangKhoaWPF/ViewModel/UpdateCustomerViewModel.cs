using BusinessObject;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LeDangKhoaWPF.ViewModel
{
    public interface IUpdateCustomerViewModel
    {
        void SetInformation(Customer customer);
        public event EventHandler? OnRefreshCustomerWindow;
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? BirthDate { get; set; }
        public string? Password { get; set; }
        public bool Status { get; set; }
        public ICommand UpdateCommand { get; }
    }

    public class UpdateCustomerViewModel : ViewModelBase, IUpdateCustomerViewModel
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomerRepository _customerRepository;

        private Customer? customer;
        private string? name;
        private string? phone;
        private string? email;
        private DateTime birthDate;
        private string? password;
        private byte? status;

        public event EventHandler? OnRefreshCustomerWindow;
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
                if (status is 1)
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
            }
        }
        public ICommand UpdateCommand { get; }

        public UpdateCustomerViewModel(ICustomerRepository customerRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _customerRepository = customerRepository;
            UpdateCommand = new ViewModelCommand(ExecuteCommand);
        }

        private async void ExecuteCommand(object? obj)
        {
            Customer? existedUser = null;
            if (!email!.Equals(customer!.Email))
            {
                existedUser = await _customerRepository.GetUserByEmail(email!);
            }
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
                if(customer is not null)
                {
                    customer.CustomerName = name;
                    customer.Telephone = phone;
                    customer.Email = email;
                    customer.CustomerBirthday = birthDate;
                    customer.CustomerStatus = status;
                    customer.Password = password ?? customer.Password;

                    var result = await _customerRepository.UpdateUserAsync(customer);
                    if (result)
                    {
                        OnRefreshCustomerWindow?.Invoke(this, EventArgs.Empty);
                        MessageBox.Show("Update Successfully");
                    }
                    else
                    {
                        MessageBox.Show("Update Fail");
                    }
                    _customerRepository.UnTrackEntity(customer);
                }
                else
                {
                    MessageBox.Show("Update Fail");
                }
            }
        }

        public void SetInformation(Customer customer)
        {
            this.customer = customer;
            Name = customer.CustomerName;
            Phone = customer.Telephone;
            Email = customer.Email;
            BirthDate = customer.CustomerBirthday.ToString();
            Status = customer.CustomerStatus == 1 ? true : false;
        }
    }
}
