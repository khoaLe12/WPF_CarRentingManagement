using BusinessObject;
using DataAccessLayer;
using LeDangKhoaWPF;
using LeDangKhoaWPF.View;
using LeDangKhoaWPF.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows;

namespace WPFApp
{
    public partial class App : Application
    {
        private ServiceProvider provider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            provider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            if(configuration is null)
            {
                throw new ArgumentNullException("Can not find configuration file");
            }

            services.AddDbContext<FucarRentingManagementContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"), b =>
                {
                    b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            });
            //.LogTo(Console.WriteLine).EnableSensitiveDataLogging()
            //DAO
            services.AddScoped<ICustomerDAO, CustomerDAO>();
            services.AddScoped<ICarInformationDAO, CarInformationDAO>();
            services.AddScoped<IManufacturerDAO, ManufacturerDAO>();
            services.AddScoped<IRentingDetailDAO, RentingDetailDAO>();
            services.AddScoped<IRentingTransactionDAO, RentingTransactionDAO>();
            services.AddScoped<ISupplierDAO, SupplierDAO>();

            //REPO
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IRentingTransactionRepository, RentingTransactionRepository>();
            services.AddScoped<ICarInformationRepository, CarInformationRepository>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();

            //VM
            services.AddScoped<ILoginViewModel, LoginViewModel>();
            services.AddScoped<ICustomerViewModel, CustomerViewModel>();
            services.AddScoped<ITransactionDetailViewModel, TransactionDetailViewModel>();
            services.AddScoped<IAdminViewModel, AdminViewModel>();
            services.AddScoped<ICreateCustomerViewModel, CreateCustomerViewModel>();
            services.AddScoped<IUpdateCustomerViewModel, UpdateCustomerViewModel>();
            services.AddScoped<ICreateCarViewModel, CreateCarViewModel>();
            services.AddScoped<IUpdateCarViewModel, UpdateCarViewModel>();
            services.AddScoped<ICreateTransactionViewModel, CreateTransactionViewModel>();
            services.AddScoped<IUpdateTransactionViewModel, UpdateTransactionViewModel>();
            services.AddScoped<ICreateOrderViewModel, CreateOrderViewModel>();
            services.AddScoped<IUpdateOrderViewModel, UpdateOrderViewModel>();

            //UnitOfWork
            services.AddScoped<IUnitOfWork,UnitOfWork>();

            //VIEW
            services.AddSingleton<Login>();
            services.AddSingleton<CustomerWindow>();
            services.AddSingleton<AdminWindow>();
            services.AddSingleton<TransactionDetail>();
            services.AddSingleton<CreateCustomer>();
            services.AddSingleton<UpdateCustomer>();
            services.AddSingleton<CreateCar>();
            services.AddSingleton<UpdateCar>();
            services.AddSingleton<CreateTransaction>();
            services.AddSingleton<UpdateTransaction>();
            services.AddSingleton<CreateOrder>();
            services.AddSingleton<UpdateOrder>();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddScoped<Session>();
        }

        private void OnStart(object sender, StartupEventArgs e)
        {
            var loginWindow = provider.GetService<Login>();
            if (loginWindow is null)
            {
                throw new ArgumentException("Login Window is missing");
            }
            loginWindow.Show();
        }
    }
}
