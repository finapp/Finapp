using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface IAssociateService
    {
        bool AddNewAssociate(Associate associate);
        bool AddCreditor(Associate associate, Creditor creditor);
        bool AddDebtor(Associate associate, Debtor debtor);
        IEnumerable<Associate> GetAllAssociations();
    }
}
