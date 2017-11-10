using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface IAssociateViewModelService
    {
        IEnumerable<AssociateViewModel> GetAllTransactions();
        IEnumerable<AssociateViewModel> GetTransactionsByDebtorUsername(string username);
        IEnumerable<AssociateViewModel> GetTransactionsByCreditorUsername(string username);
    }
}
