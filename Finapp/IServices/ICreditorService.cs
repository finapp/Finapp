using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface ICreditorService
    {
        IEnumerable<Creditor> GetAllCreditors();
        Creditor GetCreditorById(int id);
        IEnumerable<Creditor> GetCreditorsWithBalance();
        IEnumerable<Creditor> GetAvailableCreditors(float eapr);
        bool ModifyCreditor(Creditor creditor);
        string GetCreditorUsernameById(int id);
        bool AddNewCreditor(Creditor creditor)
    }
}
