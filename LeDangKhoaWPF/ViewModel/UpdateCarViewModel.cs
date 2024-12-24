using BusinessObject;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace LeDangKhoaWPF.ViewModel
{
    public interface IUpdateCarViewModel
    {
        public event EventHandler? OnRefreshCarWindow;
        public ICommand UpdateCommand { get; }
        public string? Name { get; set; }
        public string? CarDesciption { get; set; }
        public string? NumberOfDoors { get; set; }
        public string? SeatingCapacity { get; set; }
        public string? FuelType { get; set; }
        public string? Year { get; set; }
        public bool CarStatus { get; set; }
        public string? CarRentingPricePerDay { get; set; }
        public object? Manufacturer { get; set; }
        public object? Supplier { get; set; }
        public IEnumerable<Manufacturer>? Manufacturers { get; set; }
        public IEnumerable<Supplier>? Suppliers { get; set; }
        void SetInformation(CarInformation car);
    }
    public class UpdateCarViewModel : ViewModelBase, IUpdateCarViewModel
    {
        public event EventHandler? OnRefreshCarWindow;

        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICarInformationRepository _carInformationRepository;

        private CarInformation? carInformation;
        private string? name;
        private string? carDesciption;
        private int numberOfDoors;
        private int seatingCapacity;
        private string? fuelType;
        private int? year;
        private byte? carStatus;
        private decimal? carRentingPricePerDay;
        private Manufacturer? manufacturer;
        private Supplier? supplier;
        private IEnumerable<Manufacturer>? manufacturers;
        private IEnumerable<Supplier>? suppliers;

        public string? Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string? CarDesciption
        {
            get => carDesciption;
            set
            {
                carDesciption = value;
                OnPropertyChanged(nameof(CarDesciption));
            }
        }
        public string? NumberOfDoors
        {
            get => numberOfDoors.ToString();
            set
            {
                numberOfDoors = value.IsNullOrEmpty() ? 0 : Int32.Parse(value!);
                OnPropertyChanged(nameof(NumberOfDoors));
            }
        }
        public string? SeatingCapacity
        {
            get => seatingCapacity.ToString();
            set
            {
                seatingCapacity = value.IsNullOrEmpty() ? 0 : Int32.Parse(value!);
                OnPropertyChanged(nameof(SeatingCapacity));
            }
        }
        public string? FuelType
        {
            get => fuelType;
            set
            {
                fuelType = value;
                OnPropertyChanged(nameof(FuelType));
            }
        }
        public string? Year
        {
            get => year.ToString();
            set
            {
                year = value.IsNullOrEmpty() ? null : Int32.Parse(value!);
                OnPropertyChanged(nameof(Year));
            }
        }
        public bool CarStatus
        {
            get
            {
                if (carStatus is 1)
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
                    carStatus = 1;
                }
                else
                {
                    carStatus = 0;
                }
                OnPropertyChanged(nameof(CarStatus));
            }
        }
        public string? CarRentingPricePerDay
        {
            get => carRentingPricePerDay.ToString();
            set
            {
                carRentingPricePerDay = value.IsNullOrEmpty() ? null : decimal.Parse(value!);
                OnPropertyChanged(nameof(CarRentingPricePerDay));
            }
        }
        public object? Manufacturer
        {
            get => manufacturer;
            set
            {
                manufacturer = value as Manufacturer;
                OnPropertyChanged(nameof(Manufacturer));
            }
        }
        public object? Supplier
        {
            get => supplier;
            set
            {
                supplier = value as Supplier;
                OnPropertyChanged(nameof(Supplier));
            }
        }
        public IEnumerable<Manufacturer>? Manufacturers
        {
            get => manufacturers;
            set
            {
                manufacturers = value;
                OnPropertyChanged(nameof(Manufacturers));
            }
        }
        public IEnumerable<Supplier>? Suppliers
        {
            get => suppliers;
            set
            {
                suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }
        public ICommand UpdateCommand { get; }

        public UpdateCarViewModel(ICarInformationRepository carInformationRepository, IManufacturerRepository manufacturerRepository, ISupplierRepository supplierRepository)
        {
            _carInformationRepository = carInformationRepository;
            _manufacturerRepository = manufacturerRepository;
            _supplierRepository = supplierRepository;
            UpdateCommand = new ViewModelCommand(ExecuteCommand, CanExecuteCommand);
        }

        private bool CanExecuteCommand(object? obj)
        {
            bool validate;
            if (manufacturer is not null && supplier is not null && name is not null)
            {
                validate = true;
            }
            else
            {
                validate = false;
            }
            return validate;
        }

        private async void ExecuteCommand(object? obj)
        {
            if(carInformation is not null)
            {
                carInformation.CarName = name!;
                carInformation.CarDescription = carDesciption;
                carInformation.NumberOfDoors = numberOfDoors;
                carInformation.SeatingCapacity = seatingCapacity;
                carInformation.FuelType = fuelType;
                carInformation.Year = year;
                carInformation.CarStatus = carStatus;
                carInformation.CarRentingPricePerDay = carRentingPricePerDay;
                carInformation.ManufacturerId = manufacturer!.ManufacturerId;
                carInformation.SupplierId = supplier!.SupplierId;

                var result = await _carInformationRepository.UpdateCar(carInformation);
                if (result)
                {
                    OnRefreshCarWindow?.Invoke(this, EventArgs.Empty);
                    MessageBox.Show("Update Successfully");
                }
                else
                {
                    MessageBox.Show("Update Fail");
                }
                _carInformationRepository.UnTrackEntity(carInformation);
            }
        }

        public async void SetInformation(CarInformation car)
        {
            var manufacturers = await _manufacturerRepository.GetAll();
            var suppliers = await _supplierRepository.GetAll();

            carInformation = car;
            Name = car.CarName;
            CarDesciption = car.CarDescription;
            NumberOfDoors = car.NumberOfDoors.ToString();
            SeatingCapacity = car.SeatingCapacity.ToString();
            FuelType = car.FuelType;
            Year = car.Year.ToString();
            CarStatus = car.CarStatus == 1 ? true : false;
            CarRentingPricePerDay = car.CarRentingPricePerDay.ToString();
            Manufacturer = car.Manufacturer;
            Supplier = car.Supplier;

            Manufacturers = manufacturers;
            Suppliers = suppliers;
        }
    }
}
