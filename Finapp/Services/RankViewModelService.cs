using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class RankViewModelService : IRankViewModelService
    {
        private readonly FinapEntities1 _context;

        public RankViewModelService(FinapEntities1 context)
        {
            _context = context;
        }

        public IEnumerable<RankViewModel> GetCreditorsRank()
        {
            var creditors = _context.Creditor
                .ToList();

            var associations = _context.Associate
                .ToList();

            var counter = 0;
            List<RankViewModel> listOfCreditors = new List<RankViewModel>();

            foreach (var creditor in creditors)
            {
                counter = 0;
                foreach (var associate in associations)
                {
                    var transactions = _context.Transaction_Out
                        .Where(t => t.Associate_Id == associate.Associate_Id && t.Creditor_Id == creditor.Creditor_Id)
                        .ToList()
                        .Count();

                    if (transactions > 0)
                        counter++;
                }
                listOfCreditors.Add(new RankViewModel
                {
                    Username = creditor.username,
                    Delta = (int)creditor.Delta_ROI,
                    AssociateCounter = counter
                });
            }

            listOfCreditors = listOfCreditors.OrderBy(c => -c.AssociateCounter).ToList();

            return listOfCreditors;
        }

        public IEnumerable<RankViewModel> GetDebtorsRank()
        {
            var debtors = _context.Debtor
                .ToList();

            var associations = _context.Associate
                .ToList();

            var counter = 0;
            List<RankViewModel> listOfDebtors = new List<RankViewModel>();

            foreach (var debtor in debtors)
            {
                counter = 0;
                foreach (var associate in associations)
                {
                    var transactions = _context.Transaction_Out
                        .Where(t => t.Associate_Id == associate.Associate_Id && t.Debtor_Id == debtor.Debtor_Id)
                        .ToList()
                        .Count();

                    if (transactions > 0)
                        counter++;
                }
                listOfDebtors.Add(new RankViewModel
                {
                    Username = debtor.username,
                    Delta = (int)debtor.Delta_APR,
                    AssociateCounter = counter
                });
            }

            listOfDebtors = listOfDebtors.OrderBy(c => -c.AssociateCounter).ToList();

            return listOfDebtors;
        }
    }
}