using BusinessObject;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> LoginUser(string email, string password);
        Task<Customer?> FindUserById(int id);
        Task<Customer?> FindUserWithTransactionsById(int id);
        Task<bool> UpdateUserAsync(Customer updatedUser);
        Task<IEnumerable<Customer>?> GetAllCustomersAsync();
        Task<Customer?> GetUserByEmail(string email);
        Task<bool> AddNewCustomer(Customer customer);
        void UnTrackEntity(Customer customer);
        Task<bool> DeleteCustomer(Customer deletedCustomer);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Customer?> LoginUser(string email, string password)
        {
            var existedUser = await _unitOfWork.Customers.Get(c => c.Email == email).FirstOrDefaultAsync();
            if(existedUser is not null)
            {
                if(existedUser.Password is null || !existedUser.Password.Equals(password))
                {
                    return null;
                }
                return existedUser;
            }
            return null;
        }

        public async Task<Customer?> FindUserById(int id)
        {
            return await _unitOfWork.Customers.FindByIdAsync(id);
        }

        public async Task<Customer?> FindUserWithTransactionsById(int id)
        {
            Expression<Func<Customer, object>>[] includes = new Expression<Func<Customer, object>>[1]
            {
                c => c.RentingTransactions!
            };
            return await _unitOfWork.Customers.Get(c => c.CustomerId == id, includes).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateUserAsync(Customer updatedUser)
        {
            _unitOfWork.Customers.Update(updatedUser);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>?> GetAllCustomersAsync()
        {
            return await _unitOfWork.Customers.FindAll().ToArrayAsync();
        }

        public async Task<Customer?> GetUserByEmail(string email)
        {
            return await _unitOfWork.Customers.Get(c => c.Email.Equals(email)).FirstOrDefaultAsync();
        }

        public async Task<bool> AddNewCustomer(Customer customer)
        {
            await _unitOfWork.Customers.AddAsync(customer);
            return await _unitOfWork.SaveChangesAsync();
        }

        public void UnTrackEntity(Customer customer)
        {
            _unitOfWork.Customers.UnTrack(customer);
        }

        public async Task<bool> DeleteCustomer(Customer deletedCustomer)
        {
            _unitOfWork.Customers.Delete(deletedCustomer);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
