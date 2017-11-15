﻿using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class SummaryService : ISummaryService
    {
        private readonly FinapEntities1 _context;

        public SummaryService(FinapEntities1 context)
        {
            _context = context;
        }

        public bool CreateSummary(Summary summary)
        {
            try
            {
                _context.Summary.Add(summary);
                _context.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}