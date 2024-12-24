using LeDangKhoaWPF.View;
using Microsoft.Extensions.Configuration;
using Repositories;
using System;
using System.Security.Principal;
using System.Threading;
using System.Windows.Input;
using static LeDangKhoaWPF.ViewModel.LoginViewModel;

namespace LeDangKhoaWPF.ViewModel
{
    public interface ILoginViewModel
    {
        public event EventHandler<OnCloseWindowArgs>? OnCloseWindow;
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsViewVisible { get; set; }
        public ICommand LoginCommand { get; }
    }
    public class LoginViewModel : ViewModelBase, ILoginViewModel
    {
        private Session session;

        // Event for closing the login window
        public event EventHandler<OnCloseWindowArgs>? OnCloseWindow;
        public class OnCloseWindowArgs : EventArgs
        {
            internal bool IsAdmin { get; set; } = false;
        }
        //

        private readonly ICustomerRepository _customerRepository;
        private readonly IConfiguration _configuration;

        private string? email;
        private string? password;
        private string? errorMessage;
        private bool isViewVisible = true;

        public string? Email 
        { 
            get => email; 
            set 
            {
                email = value;
                //OnPropertyChanged(nameof(email));
            }
        }

        public string? Password 
        { 
            get => password;
            set
            {
                password = value;
                //OnPropertyChanged(nameof(Password));
            }
        }

        public string? ErrorMessage 
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsViewVisible 
        { 
            get => isViewVisible;
            set
            {
                isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(ICustomerRepository customerRepository, IConfiguration configuration, Session session)
        {
            this.session = session;
            _customerRepository = customerRepository;
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            _configuration = configuration;
        }

        private bool CanExecuteLoginCommand(object? obj)
        {
            bool validate;
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
            {
                validate = false;
            }
            else{
                validate = true;
            }
            return validate;
        }

        private async void ExecuteLoginCommand(object? obj)
        {
            var result = await _customerRepository.LoginUser(email!,password!);
            if(result is not null)
            {
                IsViewVisible = false;
                session.Id = result.CustomerId;
                //Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(result.CustomerName ?? "Undefined"), null);
                OnCloseWindow?.Invoke(this, new OnCloseWindowArgs { IsAdmin = false });
            }
            else
            {
                var admin = _configuration.GetSection("Admin");
                if (email!.Equals(admin["email"]) && password!.Equals(admin["password"]))
                {
                    IsViewVisible = false;
                    session.Name = "Admin";
                    //Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("Admin"), null);
                    OnCloseWindow?.Invoke(this, new OnCloseWindowArgs {IsAdmin = true });
                }
                else
                {
                    ErrorMessage = "Invalid email or password";
                }
            }
        }
    }
}
