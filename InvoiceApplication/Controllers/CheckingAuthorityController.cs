using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.Models;
using InvoiceApplication.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Security.Claims;
using InvoiceApplication.DataAccessLayer;

namespace InvoiceApplication.Controllers
{
    public class CheckingAuthorityController : Controller
    {
        public DbHelper _helper;

        public CheckingAuthorityController()
        {
            _helper = new DbHelper();
        }
        
        [HttpGet]
        [Authorize(Policy = "Checker")]
        public IActionResult Index(long invoiceId)
        {

            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            IDBService dbService = new DBservice(_helper);
            var invoices = dbService.GetInvoice(invoiceId);
            InvoiceViewModel model = new InvoiceViewModel();
            model.invoiceId = invoiceId;
            model.invoiceDate = invoices.InvoiceDate;
            model.invoiceNumber = invoices.InvoiceNo;
            model.totalLocalAmount = invoices.TotalLocalAmt;
            model.exchangeRate = invoices.ExRate;
            model.CustomerName = invoices.CustomerName;
            model.customerId = invoices.CustomerId;
            model.DelivaryDate = invoices.DeliveryDate;
            model.currencyCode = invoices.CurrencyCode;
            model.Amount = invoices.TotalAmt;
            if(invoices.InvoiceStatus <= 4)
            {
                model.showbuttons = true;
            }
            
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy ="Checker")]
        public IActionResult Index(InvoiceApprovalModel model)
        {
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            try
            {
                UpdateStatusForInvoice(model.InvoiceId, model.Status);
                ViewBag.UpdateMessage = String.Format("Successfully updated status for Invoice:{0}", model.InvoiceId);
                return new JsonResult("Updated Status sucess fully");

            }
            catch (Exception ex)
            {
                new JsonResult("Error in updating status");
            }
            return new JsonResult("Error in updating status");
        }


        public void UpdateStatusForInvoice(long invoiceId, string status)
        {
            IDBService service = new DBservice(_helper);

            var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string email = service.GetEmailForUser(username);

            var invoice = service.GetInvoice(invoiceId);
            var invoiveViewModel = new InvoiceViewModel
            {
                AccountDate = invoice.AccountDate,
                CustomerName = invoice.CustomerName,
                DelivaryDate = invoice.DeliveryDate,
                totalLocalAmount = invoice.TotalLocalAmt,
                DueDate = invoice.DueDate,
                invoiceNumber = invoice.InvoiceNo

            };

            if (status == "Pending")
            {
                service.UpdatePendingStatusForChecker(invoiceId,username);

            }
            else if (status == "Approved")
            {
                service.UpdateCheckedStatusForChecker(invoiceId, username);
                ConstructEmail construct = new ConstructEmail();
                construct.SendEmail(invoiveViewModel, email);
            }
            else if (status == "Rejected")
            {
                service.UpdateRejectedStatusForChecker(invoiceId, username);
            }
            else
            {
                throw new Exception("Invalid Status");
            }
        }

        [Authorize]
        public IActionResult ViewInvoices()
        {
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            IDBService service = new DBservice(_helper);
            var results = service.GetAllInvoices(DateTime.MinValue, DateTime.MinValue,"pending", 10);
            List<InvoiceViewModel> list = new List<InvoiceViewModel>();
            foreach (var invoice in results)
            {
                InvoiceViewModel model = new InvoiceViewModel();
                model.invoiceId = invoice.InvoiceId;
                model.invoiceNumber = invoice.InvoiceNo;
                model.invoiceDate = invoice.AccountDate;
                model.exchangeRate = invoice.ExRate;
                model.totalLocalAmount = invoice.TotalLocalAmt;
                list.Add(model);
            }
            return View(list.AsEnumerable());
        }

        [Authorize]
        [HttpPost]
        public JsonResult GetInvoices([FromBody]InvoiceSearchViewModel model)
        {
            /*
            var invoices = _context.BtsinvoiceAr.Where(i => DateTime.Compare(i.InvoiceDate, model.From) >= 0);
            if(model.Status == "pending")
            {
                //invoices = invoices.Where(i => i.IsApprovalPending == true);
            }
            if (model.Status == "checked")
            {
                //invoices = invoices.Where(i => i.IsChecked == true);
            }
            if (model.Status == "Approved")
            {
                //invoices = invoices.Where(i => i.IsApproved == true);
            }
            //invoices = invoices.Skip(model.Page * model.Results).Take(model.Results);
            List<InvoiceResults> results = new List<InvoiceResults>();
            foreach(var invoice in invoices)
            {
                InvoiceResults result = new InvoiceResults();
                result.invoiceId = invoice.InvoiceId;
                result.invoiceNo = invoice.InvoiceNo;
                result.invoiceDate = invoice.InvoiceDate;
                result.dueDate = invoice.DueDate;
                result.customerName = invoice.CustomerName;
                result.totalAmt = invoice.TotalAmt;
                result.currencyCode = invoice.CurrencyCode;
                results.Add(result);
            }*/
            List<InvoiceResults> results = new List<InvoiceResults>();

            InvoiceResults result = new InvoiceResults();
            result.invoiceId = 123;
            result.invoiceNo = "N123";
            result.invoiceDate = DateTime.Now.ToShortDateString();
            result.dueDate = DateTime.Now.ToShortDateString();
            result.customerName = "Praveen";
            result.totalAmt = 10090;
            result.currencyCode = "USD";
            results.Add(result);

            return Json(results);
        }
    }
}