using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface ICreditorAccountService
    {
        Creditor_Account GetAccountByCreditorId(int id);
    }
}
