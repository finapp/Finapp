using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.Interfaces
{
    public interface ITransactionOut
    {

        bool CreateTransaction(Transaction_Out newTransaction);

        IEnumerable<Transaction_Out> GetTransactionByDebtorId(int debtorId); 
    }
}
