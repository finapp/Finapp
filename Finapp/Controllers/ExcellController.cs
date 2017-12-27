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

            var rowNumber = 5;

            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Transactions");
                worksheet.TabColor = Color.Blue;
                worksheet.DefaultRowHeight = 12;
                worksheet.HeaderFooter.FirstFooter.LeftAlignedText = string.Format("Generated: {0}", DateTime.Now.ToShortDateString());

                worksheet.Cells[3, 1].Value = "Amount";
                worksheet.Cells[3, 2].Value = "Days";
                worksheet.Cells[3, 3].Value = "Debtor";
                worksheet.Cells[3, 4].Value = "Deb Finapp";
                worksheet.Cells[3, 5].Value = "Delta APR %";
                worksheet.Cells[3, 6].Value = "Savings";
                worksheet.Cells[3, 7].Value = "Investor";
                worksheet.Cells[3, 8].Value = "Inv Finapp";
                worksheet.Cells[3, 9].Value = "Delta ROI %";
                worksheet.Cells[3, 10].Value = "Profits";
                worksheet.Cells[3, 11].Value = "Actual profits";

                foreach (var transaction in info.ListOfTransactions)
                {
                    worksheet.Cells[rowNumber, 6].Value = "Association " + transaction.Number;
                    rowNumber++;
                    foreach (var tr in transaction.List)
                    {
                        worksheet.Cells[rowNumber, 1].Value = tr.Amount;
                        worksheet.Cells[rowNumber, 2].Value = tr.DayAccessToFunds;
                        worksheet.Cells[rowNumber, 3].Value = tr.DebtorUsername;
                        worksheet.Cells[rowNumber, 4].Value = tr.DebtorAccountFinappAmount;
                        worksheet.Cells[rowNumber, 5].Value = tr.APR;
                        worksheet.Cells[rowNumber, 6].Value = tr.RealDebtorBenefits;
                        worksheet.Cells[rowNumber, 7].Value = tr.CreditorUsername;
                        worksheet.Cells[rowNumber, 8].Value = tr.CreditorAccountFinappAmount;
                        worksheet.Cells[rowNumber, 9].Value = tr.ROI;
                        worksheet.Cells[rowNumber, 10].Value = tr.RealCreditorBenefits;
                        worksheet.Cells[rowNumber, 11].Value = tr.ActualCreditorProfits;
                        rowNumber++;
                    }
                    rowNumber += 2;
                }
                for (int i = 1; i < 12; i++)
                {
                    worksheet.Column(i).AutoFit();
                }

                ExcelWorksheet worksheetCred = package.Workbook.Worksheets.Add("Creditor Rank");
                worksheetCred.TabColor = Color.Blue;
                worksheetCred.DefaultRowHeight = 12;
                worksheetCred.HeaderFooter.FirstFooter.LeftAlignedText = string.Format("Generated: {0}", DateTime.Now.ToShortDateString());

                worksheetCred.Cells[3, 1].Value = "Username";
                worksheetCred.Cells[3, 2].Value = "ROI";
                worksheetCred.Cells[3, 3].Value = "EROI";
                worksheetCred.Cells[3, 4].Value = "Trials";
                worksheetCred.Cells[3, 5].Value = "Associations";
                worksheetCred.Cells[3, 6].Value = "Profits";
                worksheetCred.Cells[3, 7].Value = "Associate nr";
                worksheetCred.Cells[3, 8].Value = "Transactions";
                worksheetCred.Cells[3, 9].Value = "Balance";
                worksheetCred.Cells[3, 10].Value = "Transactions sum";

                var counterForAssociations = 0;
                rowNumber = 4;
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
                    worksheetCred.Cells[3, i].Value = "Associate nr";
                    worksheetCred.Cells[3, i+1].Value = "Transactions";
                    worksheetCred.Cells[3, i+2].Value = "Balance";
                    worksheetCred.Cells[3, i+3].Value = "Transaction sum";
                }

                for (int i = 1; i < count * 10; i++)
                {
                    worksheetCred.Column(i).AutoFit();
                }


                ExcelWorksheet worksheetDebt = package.Workbook.Worksheets.Add("Debtor Rank");
                worksheetDebt.TabColor = Color.Blue;
                worksheetDebt.DefaultRowHeight = 12;
                worksheetDebt.HeaderFooter.FirstFooter.LeftAlignedText = string.Format("Generated: {0}", DateTime.Now.ToShortDateString());

                worksheetDebt.Cells[3, 1].Value = "Username";
                worksheetDebt.Cells[3, 2].Value = "APR";
                worksheetDebt.Cells[3, 3].Value = "EAPR";
                worksheetDebt.Cells[3, 4].Value = "Trials";
                worksheetDebt.Cells[3, 5].Value = "Associations";
                worksheetDebt.Cells[3, 6].Value = "Savings";
                worksheetDebt.Cells[3, 7].Value = "Days with app";
                worksheetDebt.Cells[3, 8].Value = "Days with money";
                worksheetDebt.Cells[3, 9].Value = "Associate nr";
                worksheetDebt.Cells[3, 10].Value = "Transactions";
                worksheetDebt.Cells[3, 11].Value = "Debit";
                worksheetDebt.Cells[3, 12].Value = "Transactions sum";

                counterForAssociations = 0;
                rowNumber = 4;
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

                for (int i = 14; i < 14 + counterForAssociations * 4 - 4; i += 4)
                {
                    worksheetCred.Cells[3, i].Value = "Associate nr";
                    worksheetCred.Cells[3, i + 1].Value = "Transactions";
                    worksheetCred.Cells[3, i + 2].Value = "Debit";
                    worksheetCred.Cells[3, i + 3].Value = "Transaction sum";
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