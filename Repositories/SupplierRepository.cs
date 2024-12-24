using BusinessObject;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>?> GetAll();
    }
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SupplierRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Supplier>?> GetAll()
        {
            return await _unitOfWork.Superpliers.FindAll().ToArrayAsync();
        }
    }
}
