using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface ITransactionViewModelService
    {
        TransactionOutListViewModel GetAllTransactions();
        TransactionOutListViewModel GetTransactionByDeptorId(int id);
        TransactionOutListViewModel GetTransactionByCreditorId(int id);
        TransactionOutListViewModel GetTransactionByCreditorUsername(string username);
        TransactionOutListViewModel GetTransactionByDebtorUsername(string username);
    }
}
