using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IRentingDetailDAO : IBaseDAO<RentingDetail, int>
    {
    }

    public class RentingDetailDAO : BaseDAO<RentingDetail, int>, IRentingDetailDAO
    {
        public RentingDetailDAO(FucarRentingManagementContext applicationContext) : base(applicationContext)
        {
        }
    }
}
