using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    interface ICreditorService
    {
        IEnumerable<Creditor> GetAllCreditors();
        Creditor GetCreditorById(int id);
        IEnumerable<Creditor> GetDebtorsWithDebet();
    }
}
