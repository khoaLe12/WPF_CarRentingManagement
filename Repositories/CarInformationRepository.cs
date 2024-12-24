using BusinessObject;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ICarInformationRepository
    {
        Task<IEnumerable<CarInformation>?> GetAllCarInformations();
        Task<bool> AddNewCar(CarInformation addedCar);
        Task<bool> UpdateCar(CarInformation updatedCar);
        void UnTrackEntity(CarInformation car);
        Task<bool> DeleteCar(CarInformation deletedCar);
    }
    public class CarInformationRepository : ICarInformationRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public CarInformationRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CarInformation>?> GetAllCarInformations()
        {
            return await _unitOfWork.CarInformations.Get(c => true, new Expression<Func<CarInformation, object>>[2] {c => c.Manufacturer, c => c.Supplier}).ToArrayAsync();
        }
        public async Task<bool> AddNewCar(CarInformation addedCar)
        {
            await _unitOfWork.CarInformations.AddAsync(addedCar);
            return await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> UpdateCar(CarInformation updatedCar)
        {
            _unitOfWork.CarInformations.Update(updatedCar);
            return await _unitOfWork.SaveChangesAsync();
        }
        public void UnTrackEntity(CarInformation car)
        {
            _unitOfWork.CarInformations.UnTrack(car);
        }
        public async Task<bool> DeleteCar(CarInformation deletedCar)

        {
            var check = await _unitOfWork.RentingDetails.Get(r => r.CarId == deletedCar.CarId).ToArrayAsync();
            if(check.Count() > 0)
            {
                if(deletedCar.CarStatus == 0)
                {
                    return false;
                }

                deletedCar.CarStatus = 0;
                _unitOfWork.CarInformations.Update(deletedCar);
            }
            else
            {
                _unitOfWork.CarInformations.Delete(deletedCar);
            }
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
