using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.Models;
using InvoiceApplication.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace InvoiceApplication.Controllers
{
    public class CheckingAuthorityController : Controller
    {
        public TrackingDbContext _context;

        public CheckingAuthorityController(TrackingDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Authorize(Policy = "")]
        public IActionResult Index(int invoiceId)
        {

            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var invoices = from invoice in _context.BtsinvoiceAr
                           where invoice.InvoiceId == invoiceId
                           select invoice;
            InvoiceViewModel model = new InvoiceViewModel();
            var invoiceList = invoices.First();
            model.invoiceId = invoiceList.InvoiceId;
            model.invoiceDate = invoiceList.InvoiceDate;
            model.invoiceNumber = invoiceList.InvoiceNo;
            model.totalLocalAmount = invoiceList.TotalLocalAmt;
            model.exchangeRate = invoiceList.ExRate;
            model.CustomerName = invoiceList.CustomerName;
            model.customerId = invoiceList.CustomerId;
            model.DelivaryDate = invoiceList.DeliveryDate;
            model.currencyCode = invoiceList.CurrencyCode;
            model.Amount = invoiceList.TotalAmt;
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy ="")]
        public IActionResult Index(InvoiceApprovalModel model)
        {
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            UpdateStatusForInvoice(model.InvoiceId, model.status);
            return View();
        }


        public void UpdateStatusForInvoice(long invoiceId, string status)
        {
            var modifyinvoice = from invoice in _context.BtsinvoiceAr
                                where invoice.InvoiceId == invoiceId
                                select invoice;
            var InvoiceApprove = modifyinvoice.First();
            if (status == "Pending")
            {
                InvoiceApprove.IsCheckedPending = true;
                InvoiceApprove.CheckedBy = "";
                InvoiceApprove.CheckedDate = DateTime.Now.ToString();

            }
            else if (status == "Approved")
            {
                InvoiceApprove.IsChecked = true;
                InvoiceApprove.CheckedBy = "";
                InvoiceApprove.CheckedDate = DateTime.Now.ToString();
            }
            else if (status == "Rejected")
            {
                InvoiceApprove.IsWaitingApproval = false;
                InvoiceApprove.CheckedBy = "";
                InvoiceApprove.CheckedDate = DateTime.Now.ToString();
            }
            else
            {

            }

            _context.Entry(InvoiceApprove).State = EntityState.Modified;
            _context.SaveChanges();
        }

        [Authorize]
        public IActionResult ViewInvoices()
        {
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View();
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