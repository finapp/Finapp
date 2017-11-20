using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.ICreateDatabase
{
    public interface ICreator
    {
        void CreateDB(int amountOfDebtors, int amountOfCreditors);
        void ClearDB();
        void UpdateDB();
    }
}
