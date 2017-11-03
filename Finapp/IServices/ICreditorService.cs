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
        IEnumerable<Creditor> GetAvailableCreditors();
        bool ModifyCreditor(Creditor creditor);
    }
}
