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
        IEnumerable<Debtor> GetDebtorsWithDebet();
        IEnumerable<Debtor> GetAvailableDebtors();
        bool ModifyDebtor(Debtor debtor);
        string GetDebtorUsernameById(int id);
        bool AddNewDebtor(Debtor debtor);
    }
}
