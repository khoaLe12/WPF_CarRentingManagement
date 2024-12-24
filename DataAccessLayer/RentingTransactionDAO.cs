using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IRentingTransactionDAO : IBaseDAO<RentingTransaction,int>
    {
    }
    public class RentingTransactionDAO : BaseDAO<RentingTransaction, int>, IRentingTransactionDAO
    {
        public RentingTransactionDAO(FucarRentingManagementContext applicationContext) : base(applicationContext)
        {
        }
    }
}
