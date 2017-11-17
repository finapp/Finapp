using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class DebtorRankService : IDebtorRankService
    {
        private readonly FinapEntities1 _context;

        public DebtorRankService(FinapEntities1 context)
        {
            _context = context;
        }

        public bool AddAssociateToDebtor(Debtor_Rank debtorRank)
        {
            if (debtorRank == null)
                return false;

            debtorRank.Associate_Counter++;
            _context.Debtor_Rank.Attach(debtorRank);
            _context.Entry(debtorRank).State = EntityState.Modified;
            _context.SaveChanges();

            return true;
        }
    }
}