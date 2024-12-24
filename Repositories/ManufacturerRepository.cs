using BusinessObject;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public interface IManufacturerRepository
    {
        Task<IEnumerable<Manufacturer>?> GetAll();
    }
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public ManufacturerRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Manufacturer>?> GetAll()
        {
            return await _unitOfWork.Manufacturers.FindAll().ToArrayAsync();
        }
    }
}
