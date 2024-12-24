using BusinessObject;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRentingTransactionRepository
    {
        Task<IEnumerable<RentingTransaction>> GetAllRentingTransactionsOfACustomer(int customerId);
        Task<RentingTransaction?> GetTransactionById(int id);
        Task<IEnumerable<RentingDetail>?> GetTransactionDetailById(int id);
        Task<IEnumerable<RentingTransaction>> GetAllRentingTransactions();
        Task<bool> AddNewTransaction(RentingTransaction rentingTransaction, RentingDetail order);
        Task<bool> AddNewOrder(RentingDetail order, decimal? price);
        Task<bool> DeleteTransaction(RentingTransaction rentingTransaction);
        void UnTrackEntity(RentingTransaction rentingTransaction);
    }

    public class RentingTransactionRepository : IRentingTransactionRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public RentingTransactionRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RentingTransaction>> GetAllRentingTransactionsOfACustomer(int customerId)
        {
            return await _unitOfWork.RentingTransactions.Get(r => r.CustomerId == customerId).ToArrayAsync();
        }

        public async Task<RentingTransaction?> GetTransactionById(int id)
        {
            return await _unitOfWork.RentingTransactions.Get(r => r.RentingTransationId == id, new Expression<Func<RentingTransaction, object>>[2] {r => r.Customer!, r => r.RentingDetails}).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RentingDetail>?> GetTransactionDetailById(int id)
        {
            return await _unitOfWork.RentingDetails.Get(r => r.RentingTransactionId == id, new Expression<Func<RentingDetail, object>>[1] {r => r.Car}).ToArrayAsync();
        }

        public async Task<IEnumerable<RentingTransaction>> GetAllRentingTransactions()
        {
            return await _unitOfWork.RentingTransactions.FindAll().ToArrayAsync();
        }

        public async Task<bool> AddNewTransaction(RentingTransaction rentingTransaction, RentingDetail order)
        {
            var largest = _unitOfWork.RentingTransactions.FindAll().Max(r => r.RentingTransationId);
            rentingTransaction.RentingTransationId = largest + 1;
            await _unitOfWork.RentingTransactions.AddAsync(rentingTransaction);
            await _unitOfWork.RentingDetails.AddAsync(order);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> AddNewOrder(RentingDetail order, decimal? price)
        {
            var existedTransaction = await _unitOfWork.RentingTransactions.FindByIdAsync(order.RentingTransactionId);
            if(existedTransaction is not null)
            {
                existedTransaction.TotalPrice += price;
                await _unitOfWork.RentingDetails.AddAsync(order);
                return await _unitOfWork.SaveChangesAsync();
            }
            return false;
        }

        public async Task<bool> DeleteTransaction(RentingTransaction rentingTransaction)
        {
            _unitOfWork.RentingTransactions.Delete(rentingTransaction);
            return await _unitOfWork.SaveChangesAsync();
        }

        public void UnTrackEntity(RentingTransaction rentingTransaction)
        {
            _unitOfWork.RentingTransactions.UnTrack(rentingTransaction);
        }
    }
}