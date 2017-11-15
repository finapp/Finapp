﻿using Finapp.IServices;
using Finapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class AssociateService : IAssociateService
    {
        private readonly FinapEntities1 _context;

        public AssociateService(FinapEntities1 context)
        {
            _context = context;
        }

        public bool AddNewAssociate(Associate associate)
        {
            try
            {
                _context.Associate.Add(associate);
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