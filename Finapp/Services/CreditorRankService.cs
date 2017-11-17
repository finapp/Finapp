using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class CreditorRankService : ICreditorRankService
    {
        private readonly FinapEntities1 _context;

        public CreditorRankService(FinapEntities1 context)
        {
            _context = context;
        }

        public bool AddAssociateToCreditor(Creditor_Rank creditorRank)
        {
            if (creditorRank == null)
                return false;

            creditorRank.Associate_Counter++;
            _context.Creditor_Rank.Attach(creditorRank);
            _context.Entry(creditorRank).State = EntityState.Modified;
            _context.SaveChanges();

            return true;
        }
    }
}