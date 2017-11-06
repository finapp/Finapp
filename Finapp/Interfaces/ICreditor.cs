using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.Interfaces
{
    public interface ICreditor
    {

        IEnumerable<Creditor> GetAvailableCreditors(Debtor debtor);

        bool ModifyCreditor(Creditor creditor);

        IEnumerable<Creditor> GetAllCreditors();

        Creditor GetCreditorById(int id);

    }
}
