using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.Interfaces
{
    public interface ICreditorAccount
    {
        Creditor_Account getCreditorAccountByCreditorId(int id);

    }
}
