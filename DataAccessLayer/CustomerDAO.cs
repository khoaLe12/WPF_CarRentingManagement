using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface ICustomerDAO : IBaseDAO<Customer, int>
    {

    }

    public class CustomerDAO : BaseDAO<Customer, int>, ICustomerDAO
    {
        public CustomerDAO(FucarRentingManagementContext applicationContext) : base(applicationContext)
        {
        }
    }
}
