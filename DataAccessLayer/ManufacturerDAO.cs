using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{ 
    public interface IManufacturerDAO : IBaseDAO<Manufacturer,int>
    {
    }

    public class ManufacturerDAO : BaseDAO<Manufacturer, int>, IManufacturerDAO
    {
        public ManufacturerDAO(FucarRentingManagementContext applicationContext) : base(applicationContext)
        {
        }
    }
}
