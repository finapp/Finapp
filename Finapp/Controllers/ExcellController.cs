using Finapp.IServices;
using Ionic.Zip;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Finapp.Controllers
{
    public class ExcellController : Controller
    {
        private readonly ISummaryViewModelService _summaryService;
        private readonly IRankViewModelService _rankService;

        public ExcellController(ISummaryViewModelService summaryService,
            IRankViewModelService rankService)
        {
            _summaryService = summaryService;
            _rankService = rankService;
        }

        public ActionResult Index()
        {
            MemoryStream file = new MemoryStream();

            var info = _summaryService.GetAllInformations();
            var credRank = _rankService.GetCreditorsRank();
            var debRank = _rankService.GetDebtorsRank();

            var rowNumber = 2;

            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Transactions");
                worksheet.TabColor = Color.Blue;
                worksheet.DefaultRowHeight = 12;
                worksheet.HeaderFooter.FirstFooter.LeftAlignedText = string.Format("Generated: {0}", DateTime.Now.ToShortDateString());

                worksheet.Cells[1, 1].Value = "Association";
                worksheet.Cells[1, 2].Value = "Amount TX";
                worksheet.Cells[1, 3].Value = "Days";
                worksheet.Cells[1, 4].Value = "Debtor";
                worksheet.Cells[1, 5].Value = "Deb Finapp";
                worksheet.Cells[1, 6].Value = "Delta APR %";
                worksheet.Cells[1, 7].Value = "Savings";
                worksheet.Cells[1, 8].Value = "Investor";
                worksheet.Cells[1, 9].Value = "Inv Finapp";
                worksheet.Cells[1, 10].Value = "Delta ROI %";
                worksheet.Cells[1, 11].Value = "Profits";
                worksheet.Cells[1, 12].Value = "Actual profits";

                foreach (var transaction in info.ListOfTransactions)
                {
                    worksheet.Cells[rowNumber, 1].Value = "Association " + transaction.Number;
                    
                    foreach (var tr in transaction.List)
                    {
                        worksheet.Cells[rowNumber, 2].Value = tr.Amount;
                        worksheet.Cells[rowNumber, 3].Value = tr.DayAccessToFunds;
                        worksheet.Cells[rowNumber, 4].Value = tr.DebtorUsername;
                        worksheet.Cells[rowNumber, 5].Value = tr.DebtorAccountFinappAmount;
                        worksheet.Cells[rowNumber, 6].Value = tr.APR;
                        worksheet.Cells[rowNumber, 7].Value = tr.RealDebtorBenefits;
                        worksheet.Cells[rowNumber, 8].Value = tr.CreditorUsername;
                        worksheet.Cells[rowNumber, 9].Value = tr.CreditorAccountFinappAmount;
                        worksheet.Cells[rowNumber, 10].Value = tr.ROI;
                        worksheet.Cells[rowNumber, 11].Value = tr.RealCreditorBenefits;
                        worksheet.Cells[rowNumber, 12].Value = tr.ActualCreditorProfits;
                        rowNumber++;
                    }
                }
                for (int i = 1; i < 12; i++)
                {
                    worksheet.Column(i).AutoFit();
                }

                ExcelWorksheet worksheetCred = package.Workbook.Worksheets.Add("Creditor Rank");
                worksheetCred.TabColor = Color.Blue;
                worksheetCred.DefaultRowHeight = 12;
                worksheetCred.HeaderFooter.FirstFooter.LeftAlignedText = string.Format("Generated: {0}", DateTime.Now.ToShortDateString());

                worksheetCred.Cells[1, 1].Value = "Username";
                worksheetCred.Cells[1, 2].Value = "ROI";
                worksheetCred.Cells[1, 3].Value = "EROI";
                worksheetCred.Cells[1, 4].Value = "Trials";
                worksheetCred.Cells[1, 5].Value = "Associations";
                worksheetCred.Cells[1, 6].Value = "Profits";
                worksheetCred.Cells[1, 7].Value = "Associate nr";
                worksheetCred.Cells[1, 8].Value = "Transactions";
                worksheetCred.Cells[1, 9].Value = "Balance";
                worksheetCred.Cells[1, 10].Value = "Transactions sum";

                var counterForAssociations = 0;
                rowNumber = 2;
                var count = 7;
                foreach (var cred in credRank)
                {
                    worksheetCred.Cells[rowNumber, 1].Value = cred.Username;
                    worksheetCred.Cells[rowNumber, 2].Value = cred.ROI;
                    worksheetCred.Cells[rowNumber, 3].Value = cred.EROI;
                    worksheetCred.Cells[rowNumber, 4].Value = cred.Trials;
                    worksheetCred.Cells[rowNumber, 5].Value = cred.AssociateCounter;
                    worksheetCred.Cells[rowNumber, 6].Value = cred.Money;
                    count = 7;
                    var userAssociationsCounter = 0;

                    foreach (var item in cred.Associations)
                    {
                        userAssociationsCounter++;
                        worksheetCred.Cells[rowNumber, count].Value = item.AssociateNr;
                        count++;
                        worksheetCred.Cells[rowNumber, count].Value = item.TransactionCounter;
                        count++;
                        worksheetCred.Cells[rowNumber, count].Value = item.ActualDebet;
                        count++;
                        worksheetCred.Cells[rowNumber, count].Value = item.MoneyInTransactions;
                        count++;

                        if (userAssociationsCounter > counterForAssociations)
                            counterForAssociations = userAssociationsCounter;
                    }

                    rowNumber++;
                }

                for (int i = 11; i < 11+counterForAssociations*4-4; i+=4)
                {
                    worksheetCred.Cells[1, i].Value = "Associate nr";
                    worksheetCred.Cells[1, i+1].Value = "Transactions";
                    worksheetCred.Cells[1, i+2].Value = "Balance";
                    worksheetCred.Cells[1, i+3].Value = "Transaction sum";
                }

                for (int i = 1; i < count * 10; i++)
                {
                    worksheetCred.Column(i).AutoFit();
                }


                ExcelWorksheet worksheetDebt = package.Workbook.Worksheets.Add("Debtor Rank");
                worksheetDebt.TabColor = Color.Blue;
                worksheetDebt.DefaultRowHeight = 12;
                worksheetDebt.HeaderFooter.FirstFooter.LeftAlignedText = string.Format("Generated: {0}", DateTime.Now.ToShortDateString());

                worksheetDebt.Cells[1, 1].Value = "Username";
                worksheetDebt.Cells[1, 2].Value = "APR";
                worksheetDebt.Cells[1, 3].Value = "EAPR";
                worksheetDebt.Cells[1, 4].Value = "Trials";
                worksheetDebt.Cells[1, 5].Value = "Associations";
                worksheetDebt.Cells[1, 6].Value = "Savings";
                worksheetDebt.Cells[1, 7].Value = "Days with app";
                worksheetDebt.Cells[1, 8].Value = "Days with money";
                worksheetDebt.Cells[1, 9].Value = "Associate nr";
                worksheetDebt.Cells[1, 10].Value = "Transactions";
                worksheetDebt.Cells[1, 11].Value = "Debit";
                worksheetDebt.Cells[1, 12].Value = "Transactions sum";

                counterForAssociations = 0;
                rowNumber = 2;
                count = 8;
                foreach (var cred in debRank)
                {
                    worksheetDebt.Cells[rowNumber, 1].Value = cred.Username;
                    worksheetDebt.Cells[rowNumber, 2].Value = cred.APR;
                    worksheetDebt.Cells[rowNumber, 3].Value = cred.EAPR;
                    worksheetDebt.Cells[rowNumber, 4].Value = cred.Trials;
                    worksheetDebt.Cells[rowNumber, 5].Value = cred.AssociateCounter;
                    worksheetDebt.Cells[rowNumber, 6].Value = cred.Money;
                    worksheetDebt.Cells[rowNumber, 7].Value = cred.Days;
                    worksheetDebt.Cells[rowNumber, 8].Value = cred.DaysWithMoney;
                    var userAssociationsCounter = 0;
                    count = 9;
                    foreach (var item in cred.Associations)
                    {
                        userAssociationsCounter++;
                        worksheetDebt.Cells[rowNumber, count].Value = item.AssociateNr;
                        count++;
                        worksheetDebt.Cells[rowNumber, count].Value = item.TransactionCounter;
                        count++;
                        worksheetDebt.Cells[rowNumber, count].Value = item.ActualDebet;
                        count++;
                        worksheetDebt.Cells[rowNumber, count].Value = item.MoneyInTransactions;
                        count++;

                        if (userAssociationsCounter > counterForAssociations)
                            counterForAssociations = userAssociationsCounter;
                    }

                    rowNumber++;
                }

                for (int i = 13; i < 13 + counterForAssociations * 4 - 4; i += 4)
                {
                    worksheetDebt.Cells[1, i].Value = "Associate nr";
                    worksheetDebt.Cells[1, i + 1].Value = "Transactions";
                    worksheetDebt.Cells[1, i + 2].Value = "Debit";
                    worksheetDebt.Cells[1, i + 3].Value = "Transaction sum";
                }
                for (int i = 1; i < count * 10; i++)
                {
                    worksheetDebt.Column(i).AutoFit();
                }

                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=Arkusz.xlsx");
                    package.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();

                }

            }

            return RedirectToAction("Index", "Transaction");
        }
    }
}