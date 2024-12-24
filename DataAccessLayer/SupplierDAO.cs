using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface ISupplierDAO : IBaseDAO<Supplier, int>
    {
    }

    public class SupplierDAO : BaseDAO<Supplier, int>, ISupplierDAO
    {
        public SupplierDAO(FucarRentingManagementContext applicationContext) : base(applicationContext)
        {
        }
    }
}
