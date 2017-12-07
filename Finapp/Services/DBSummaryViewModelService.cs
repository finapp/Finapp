using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Finapp.Services
{
    public class DBSummaryViewModelService : IDBSummaryViewModelService
    {
        private readonly FinapEntities1 _context;

        public DBSummaryViewModelService(FinapEntities1 context)
        {
            _context = context;
        }

        public DBSummaryViewModel GetViewModel()
        {
            var creditors = _context.Creditor.AsNoTracking()
                             .Count();

            var debtors = _context.Debtor.AsNoTracking()
                           .Count();

            var transactions = _context.Transaction_Out.AsNoTracking()
                                .Count();

            var associations = _context.Associate.AsNoTracking()
                                .Count();

            return new DBSummaryViewModel
            {
                AmountOfCreditors = creditors,
                AmountOfDebtors = debtors,
                AmoountOfTransactions = transactions,
                AmountOfAssociations = associations
            };
        }
    }
}