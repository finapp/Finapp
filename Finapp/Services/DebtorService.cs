using Finapp.Interfaces;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class DebtorService : IDebtor
    {
        FinapEntities context;

        public DebtorService(FinapEntities _context)
        {
            context = _context;
        }

        public Debtor getAvaialbleDebtor()
        {
            try
            {
                return context.Debtor.Where(d => d.Available == true).OrderBy(d => d.Queue_Date).FirstOrDefault();
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}