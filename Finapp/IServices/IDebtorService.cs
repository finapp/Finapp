using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface IDebtorService
    {
        IEnumerable<Debtor> GetAllDebtors();
        Debtor GetDebtorById(int id);
        Debtor GetDebtorByUsername(string username);
        IEnumerable<Debtor> GetDebtorsWithDebet();
        IEnumerable<Debtor> GetAvailableDebtors();
        bool ModifyDebtor(Debtor debtor);
        bool ModifyDebtors(IEnumerable<Debtor> debtors);
        string GetDebtorUsernameById(int id);
        bool AddNewDebtor(Debtor debtor);
        bool AddNewDebtors(IEnumerable<Debtor> debtors);
        bool AddAssociate(Associate associate, Debtor debtor);
        IEnumerable<Debtor> GetDebtorsFromAssociate(Associate associate);
        DateTime GetTheOldestQueueDate();
    }
}
