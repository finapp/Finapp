using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface IReturnTransactionService
    {
        bool AddReturnTransaction(Return_Transaction returnTransaction);
        bool AddReturnTransaction(Transaction_Out transactionOut);
    }
}
