using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.Interfaces
{
    public interface IAlgorithm
    {


        bool MergeDebtorWithCreditors();

        IEnumerable<Creditor> SelectCreditorsToMerge(Debtor debtor, IEnumerable<Creditor> availablesCreditors);

        bool CreateTransactionOut(Debtor debtor, Creditor creditor, int amount);

    }
}
