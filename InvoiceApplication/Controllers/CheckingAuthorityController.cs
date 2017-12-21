using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.Models;
using Microsoft.AspNetCore.Authorization;

using System.Collections.Generic;
using System.Security.Claims;
using InvoiceApplication.DataAccessLayer;
using System.Globalization;
using static InvoiceApplication.Utilies;

namespace InvoiceApplication.Controllers
{
    public class CheckingAuthorityController : Controller
    {
        public DbHelper _helper;

        public CheckingAuthorityController()
        {
            _helper = new DbHelper();
        }
        [Route("CheckingAuthority/Index")]
        [Authorize(Policy = "Checker")]
        public IActionResult Index(long invoiceId = 1)
        {

            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            IDBService dbService = new DBservice(_helper);
            var invoices = dbService.GetInvoice(invoiceId);
            var numberOfDocs = dbService.FetchDocumentsForInvoice(invoiceId);
            InvoiceViewModel model = new InvoiceViewModel();
            InvoiceStatus invStatus = (InvoiceStatus)invoices.InvoiceStatus;
            model.InvoiceId = invoiceId;
            model.AccountDate = invoices.AccountDate.ToString("dd / M / yyyy", CultureInfo.InvariantCulture);
            model.InvoiceNumber = invoices.InvoiceNo;
            model.TotalLocalAmount = invoices.TotalLocalAmt.ToString("N",
                                                CultureInfo.CreateSpecificCulture("en-IN")); 
            model.ExchangeRate = invoices.ExRate;
            model.CustomerName = invoices.CustomerName;
            model.DelivaryDate = invoices.DeliveryDate.ToString("dd / M / yyyy", CultureInfo.InvariantCulture);
            model.CurrencyCode = invoices.CurrencyCode;
            model.VesselName = invoices.VesselName;
            model.Amount = invoices.TotalAmt.ToString("N",
                                                CultureInfo.CreateSpecificCulture("en-IN"));
            model.InvoiceStatus = invStatus.ToString();
            model.NoOfDocuments = numberOfDocs != null ?numberOfDocs.Count : 0;
            model.Remarks = invoices.Remarks;
            if(invoices.InvoiceStatus <= 4)
            {
                model.Showbuttons = true;
            }
            
            return View(model);
        }

        [HttpGet]
        public IActionResult ViewInvoices(InvoiceSearchViewModel model1)
        {
            DateTime from = DateTime.MinValue;
            DateTime to = DateTime.MinValue;
            if (model1.Status == null || model1.Status == string.Empty)
            {
                model1.Status = "pending";
            }
            if(model1.From != null)
            {
                if (model1.From != "" || model1.From != string.Empty )
                {
                    from = DateTime.Parse(model1.From, new CultureInfo("zh-SG"));
                }
            }
            if(model1.To != null)
            {
                if (model1.To != "" || model1.To != string.Empty)
                {
                    to = DateTime.Parse(model1.To, new CultureInfo("zh-SG"));
                }
            }
            
            
            var userRole = "";
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if(role == "1")
            {
                userRole = "CheckingAuthority";
            }
            else if(role == "2")
            {
                userRole = "ApproverAuthority";
            }
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            IDBService service = new DBservice(_helper);
            var fetchedResults = service.GetAllInvoices(from, to, model1.Status,userRole);
            var results = fetchedResults.Skip(model1.Results*5).Take(5).OrderBy(e => e.DueDate);
            InvoicePagingResults pagingResults = new InvoicePagingResults();
            List<InvoiceResults> list = new List<InvoiceResults>();
            foreach (var invoice in results)
            {
                InvoiceResults model = new InvoiceResults();
                model.InvoiceId = invoice.InvoiceId;
                model.InvoiceNo = invoice.InvoiceNo;
                model.AccountDate = invoice.AccountDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                model.DueDate = invoice.DueDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                model.CurrencyCode = invoice.CurrencyCode;
                model.CustomerName = invoice.CustomerName;
                model.VesselName = invoice.VesselName;
                model.TotalAmt = invoice.TotalLocalAmt.ToString("N",
                                                CultureInfo.CreateSpecificCulture("en-IN"));
                list.Add(model);
            }
            pagingResults.currentPageNumber = model1.Results;
            int records = fetchedResults.Count();
            int remainder = records % 5;
            pagingResults.totalNumberOfRecords = remainder > 0 ? (records / 5) + 1 : (records / 5);
            pagingResults.invoiceResults = list;
            pagingResults.modelInvoice = model1;
            return View(pagingResults);
        }

        [Authorize(Policy = "Checker")]
        [HttpPost]
        public IActionResult ViewInvoices(InvoicePostSearch model1)
        {
            DateTime from = DateTime.MinValue;
            DateTime to = DateTime.MinValue;
            if (model1.From != null)
            {
                if (model1.From != "" || model1.From != string.Empty)
                {
                    from = DateTime.Parse(model1.From, new CultureInfo("zh-SG"));
                }
            }
            if (model1.To != null)
            {
                if (model1.To != "" || model1.To != string.Empty)
                {
                    to = DateTime.Parse(model1.To, new CultureInfo("zh-SG"));
                }
            }
            var userRole = "";
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if (role == "1")
            {
                userRole = "CheckingAuthority";
            }
            else if (role == "2")
            {
                userRole = "ApproverAuthority";
            }
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            IDBService service = new DBservice(_helper);
            var fetchedResults = service.GetAllInvoices(from, to, model1.Status,userRole);
            var results = fetchedResults.Take(5);
            results = results.OrderBy(e => e.DueDate);
            InvoicePagingResults pagingResults = new InvoicePagingResults();
            List<InvoiceResults> list = new List<InvoiceResults>();
            foreach (var invoice in results)
            {
                InvoiceResults model = new InvoiceResults();
                model.InvoiceId = invoice.InvoiceId;
                model.InvoiceNo = invoice.InvoiceNo;
                model.AccountDate = invoice.AccountDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                model.DueDate = invoice.DueDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                model.CurrencyCode = invoice.CurrencyCode;
                model.CustomerName = invoice.CustomerName;
                model.VesselName = invoice.VesselName;
                model.TotalAmt = invoice.TotalLocalAmt.ToString("N",
                                                CultureInfo.CreateSpecificCulture("en-IN"));
                list.Add(model);
            }
            pagingResults.currentPageNumber = 0;
            int records = fetchedResults.Count();
            int remainder = records % 5;
            pagingResults.totalNumberOfRecords = remainder > 0 ? (records / 5) + 1 : (records / 5);
            pagingResults.invoiceResults = list;
            pagingResults.modelInvoice = new InvoiceSearchViewModel()
            {
                 From = model1.From,
                 To = model1.To,
                  Page = 0,
                  Results = 0,
                   Status = model1.Status
            };
            return View(pagingResults);
        }
    }
}