using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class TransactionOutService : ITransactionOutService
    {
        private readonly FinapEntities1 _context;
        private readonly ICreditorService _creditorService;
        private readonly IDebtorService _debtorService;

        public TransactionOutService(FinapEntities1 context, ICreditorService creditorService, IDebtorService debtorService)
        {
            _context = context;
            _creditorService = creditorService;
            _debtorService = debtorService;
        }

        public bool AddTransaction(Transaction_Out transaction)
        {
            try
            {
                _context.Transaction_Out.Add(transaction);
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool AddTransaction(int amount, DateTime date, int creditorAccountId, int debtorAccountId)
        {
            try
            {
                _context.Transaction_Out.Add(new Transaction_Out
                {
                    Ammount = amount,
                    Date_Of_Transaction = date,
                    Creditor_Account_Id = creditorAccountId,
                    Debtor_Account_Id = debtorAccountId
                });

                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public TransactionWithUserViewModel CreateTransactionWithUserViewModel(Transaction_Out transaction)
        {
            var days = transaction.Day_Access_To_Funds;
            var partOfYear = (float)((float)days / 365);

            var debtor = _debtorService.GetDebtorById(transaction.Debtor_Id??0);
            var realDebtorBenefits = ((int)((float)partOfYear * transaction.Debtor_Benefits_Per_Annum ?? 0));

            var creditor = _creditorService.GetCreditorById(transaction.Creditor_Id??0);
            var realCreditorBenefits = ((int)((float)partOfYear * transaction.Creditor_Benefits_Per_Annum ?? 0));

            return new TransactionWithUserViewModel
            {
                Amount = transaction.Ammount,
                DebtorAccountFinappAmount = transaction.Finapp_Debetor ?? 0,
                DebtorUsername = debtor.username,
                Date = transaction.Date_Of_Transaction ?? DateTime.Now,
                ROI = (float)transaction.ROI,
                APR = debtor.Delta_APR,
                CreditorUsername = creditor.username,
                CreditorAccountFinappAmount = transaction.Finapp_Creditor ?? 100,
                CreditorBenefits = transaction.Creditor_Benefits_Per_Annum ?? 0,
                DebtorBenefits = transaction.Debtor_Benefits_Per_Annum ?? 0,
                RealCreditorBenefits = realCreditorBenefits,
                RealDebtorBenefits = realDebtorBenefits,
                DayAccessToFunds = transaction.Day_Access_To_Funds
            };
        }

        public IEnumerable<Transaction_Out> GetTransactionsFromAssociate(Associate associate)
        {
            return _context.Transaction_Out
                .Where(t => t.Associate_Id == associate.Associate_Id)
                .ToList();
        }
    }
}