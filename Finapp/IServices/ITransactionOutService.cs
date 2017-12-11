using Finapp.Models;
using Finapp.ViewModels;
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
        bool AddTransactions(IEnumerable<Transaction_Out> transactions);
        bool AddTransaction(int amount, DateTime date, int creditorAccountId, int debtorAccountId);
        TransactionWithUserViewModel CreateTransactionWithUserViewModel(Transaction_Out transaction);
        IEnumerable<Transaction_Out> GetTransactionsFromAssociate(Associate associate);
        UserTransactionsViewModel GetUserTransactions(int amount, int associateNr, int profit, string username);
    }
}
