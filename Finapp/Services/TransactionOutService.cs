using Finapp.IServices;
using Finapp.Models;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Finapp.Services
{
    public class TransactionOutService : ITransactionOutService
    {
        private FinapEntities1 _context;
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

        public bool AddTransactions(IEnumerable<Transaction_Out> transactions)
        {
            int counter = 1;

            try
            {
                foreach (var item in transactions)
                {
                    counter++;
                    _context.Entry(item).State = EntityState.Added;

                    if (counter % 100 == 0)
                    {
                        _context.SaveChanges();
                        _context.Dispose();
                        _context = new FinapEntities1();
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                _context.Dispose();
            }

            return true;
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

            var debtor = (from d in _context.Debtor
                          where d.Debtor_Id == transaction.Debtor_Id
                          select new { d.username, d.Delta_APR })
                          .FirstOrDefault();

            var realDebtorBenefits = ((int)((float)partOfYear * transaction.Debtor_Benefits_Per_Annum ?? 0));

            var creditor = (from c in _context.Creditor
                            where c.Creditor_Id == transaction.Creditor_Id
                            select c.username)
                            .FirstOrDefault();

            var realCreditorBenefits = ((int)((float)partOfYear * transaction.Creditor_Benefits_Per_Annum ?? 0));

            return new TransactionWithUserViewModel
            {
                Amount = transaction.Ammount,
                DebtorAccountFinappAmount = transaction.Finapp_Debetor ?? 0,
                DebtorUsername = debtor.username,
                Date = transaction.Date_Of_Transaction ?? DateTime.Now,
                ROI = (float)transaction.ROI,
                APR = debtor.Delta_APR??0,
                CreditorUsername = creditor,
                CreditorAccountFinappAmount = transaction.Finapp_Creditor ?? 100,
                CreditorBenefits = transaction.CreditorBenefits ?? 0,
                DebtorBenefits = transaction.Debtor_Benefits_Per_Annum ?? 0,
                RealCreditorBenefits = transaction.CreditorBenefits ?? 0,
                RealDebtorBenefits = realDebtorBenefits,
                DayAccessToFunds = transaction.Day_Access_To_Funds,
                ActualCreditorProfits = transaction.ActualCreditorBenefits ?? 0
            };
        }

        public IEnumerable<Transaction_Out> GetTransactionsFromAssociate(Associate associate)
        {
            return _context.Transaction_Out
                .Where(t => t.Associate_Id == associate.Associate_Id)
                .ToList();
        }

        public UserTransactionsViewModel GetUserTransactions(int amount, int associateNr, int profit, string username, int transactions)
        {
            return new UserTransactionsViewModel
            {
                Amount = amount,
                AssociateNr = associateNr,
                Username = username,
                Profits = profit,
                Transactions = transactions
            };
        }
    }
}