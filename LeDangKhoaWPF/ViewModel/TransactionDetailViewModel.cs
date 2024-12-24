using LeDangKhoaWPF.View;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Collections.Generic;

namespace LeDangKhoaWPF.ViewModel
{
    public interface ITransactionDetailViewModel
    {
        public IEnumerable<RetingDetail>? TransactionDetails { get; set; }
        public void SetTransactionDetailId(int id);
    }
    public class TransactionDetailViewModel : ViewModelBase, ITransactionDetailViewModel
    {
        private readonly IRentingTransactionRepository _transactionRepository;

        private int transactionId;

        private IEnumerable<RetingDetail>? transactionDetails;

        public IEnumerable<RetingDetail>? TransactionDetails 
        { 
            get => transactionDetails;
            set
            {
                transactionDetails = value;
                OnPropertyChanged(nameof(TransactionDetails));
            }
        }

        public TransactionDetailViewModel(IRentingTransactionRepository rentingTransactionRepository)
        {
            _transactionRepository = rentingTransactionRepository;
        }

        private async void LoadInformation()
        {
            var result = await _transactionRepository.GetTransactionDetailById(transactionId);
            if (!result.IsNullOrEmpty())
            {
                var list = new List<RetingDetail>();
                foreach (var item in result!)
                {
                    list.Add(new RetingDetail
                    {
                        StartDate = item.StartDate.ToString(),
                        EndDate = item.EndDate.ToString(),
                        Price = item.Price.ToString(),
                        CarId = item.CarId.ToString(),
                        CarName = item.Car.CarName,
                        RentingPricePerDay = item.Car.CarRentingPricePerDay.ToString()
                    });
                }
                TransactionDetails = list;
            }
        }

        public void SetTransactionDetailId(int id)
        {
            this.transactionId = id;
            LoadInformation();
        }
    }

    public class RetingDetail : ViewModelBase
    {
        private string startDate = "";
        public string StartDate 
        { 
            get => startDate;
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private string endDate = "";
        public string EndDate 
        { 
            get => endDate;
            set
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            } 
        }

        private string? price;
        public string? Price 
        { 
            get => price;
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        private string carId = "Undefined";
        public string CarId 
        { 
            get => carId;
            set
            {
                carId = value;
                OnPropertyChanged(nameof(CarId));
            }
        }

        private string carName = "Undefined";
        public string CarName 
        { 
            get => carName;
            set
            {
                carName = value;
                OnPropertyChanged(nameof(CarName));
            }
        }

        private string? rentingPricePerDay;
        public string? RentingPricePerDay 
        { 
            get => rentingPricePerDay;
            set
            {
                rentingPricePerDay = value;
                OnPropertyChanged(nameof(RentingPricePerDay));
            }
        }
    }
}
