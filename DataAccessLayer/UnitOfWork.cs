using BusinessObject;

namespace DataAccessLayer
{
    public interface IUnitOfWork
    {
        ICarInformationDAO CarInformations { get; }
        ICustomerDAO Customers { get; }
        IManufacturerDAO Manufacturers { get; }
        IRentingDetailDAO RentingDetails { get; }
        IRentingTransactionDAO RentingTransactions { get; }
        ISupplierDAO Superpliers { get; }
        Task<bool> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FucarRentingManagementContext _dbContext;

        public UnitOfWork(FucarRentingManagementContext dbContext, 
            ICarInformationDAO carInformations, 
            ICustomerDAO customers, 
            IManufacturerDAO manufacturers, 
            IRentingDetailDAO rentingDetails, 
            IRentingTransactionDAO rentingTransactions,
            ISupplierDAO superpliers)
        {
            _dbContext = dbContext;
            CarInformations = carInformations;
            Customers = customers;
            Manufacturers = manufacturers;
            RentingDetails = rentingDetails;
            RentingTransactions = rentingTransactions;
            Superpliers = superpliers;
        }

        public ICarInformationDAO CarInformations { get; private set;  }
        public ICustomerDAO Customers { get; private set; }
        public IManufacturerDAO Manufacturers { get; private set; }
        public IRentingDetailDAO RentingDetails { get; private set; }
        public IRentingTransactionDAO RentingTransactions { get; private set; }
        public ISupplierDAO Superpliers { get; private set; }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
