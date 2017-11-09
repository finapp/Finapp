using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface ITransactionOutService
    {
        bool AddTransaction(Transaction_Out transaction);
        bool AddTransaction(int amount, DateTime date, int creditorAccountId, int debtorAccountId);
        void GetTransactionsByDebtorId(int id);
    }
}
