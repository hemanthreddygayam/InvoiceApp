using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.DbModels;
using Microsoft.AspNetCore.Authorization;
using InvoiceApplication.Models;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using System.Security.Claims;
using InvoiceApplication.DataAccessLayer;

=======
using System.Security.Claims;

>>>>>>> 768931a30e98d796346d6b52ecc23dd4f52caa05
namespace InvoiceApplication.Controllers
{
    public class ApproverAuthorityController : Controller
    {
        public DbHelper _helper;
        public ApproverAuthorityController()
        {
            _helper = new DbHelper();
        }

        [HttpGet]
        [Authorize(Policy = "Approver")]
        public IActionResult Index(long invoiceId)
        {

<<<<<<< HEAD


            var username = ClaimTypes.NameIdentifier;
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            IDBService dbService = new DBservice(_helper);
            var invoices = dbService.GetInvoice(invoiceId);
=======
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var invoices = from invoice in _context.BtsinvoiceAr
                           where invoice.InvoiceId == invoiceId
                           select invoice;
>>>>>>> 768931a30e98d796346d6b52ecc23dd4f52caa05
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
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "Approver")]
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
                return View();

            }
            catch (Exception ex)
            {

            }
            return View();
        }


        public void UpdateStatusForInvoice(long invoiceId, string status)
        {
            IDBService service = new DBservice(_helper);
            var username = ClaimTypes.NameIdentifier;



            if (status == "Pending")
            {
                service.UpdatePendingStatusForApprover(invoiceId, username);

            }
            else if (status == "Approved")
            {
                service.UpdateApprovedStatusForApprover(invoiceId, username);
            }
            else if (status == "Rejected")
            {
                service.UpdateRejectedStatusForApprover(invoiceId, username);
            }
            else
            {
                throw new Exception("Invalid Status");
            }

<<<<<<< HEAD

=======
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
>>>>>>> 768931a30e98d796346d6b52ecc23dd4f52caa05
        }
    }
}