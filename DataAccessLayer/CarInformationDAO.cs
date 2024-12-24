using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface ICarInformationDAO : IBaseDAO<CarInformation,int>
    {
    }

    public class CarInformationDAO : BaseDAO<CarInformation,int>, ICarInformationDAO
    {
        public CarInformationDAO(FucarRentingManagementContext dbContext) : base(dbContext)
        {
        }
    }
}
