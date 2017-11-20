using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.DbModels;
using Microsoft.AspNetCore.Authorization;
using InvoiceApplication.Models;

using System.Security.Claims;
using InvoiceApplication.DataAccessLayer;


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

            var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.Name = username;
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
            model.AccountDate = invoices.AccountDate;
            if (invoices.InvoiceStatus <= 4)
            {
                model.showbuttons = true;
            }

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

        }

        [Authorize]
        public IActionResult ViewInvoices()
        {
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            IDBService service = new DBservice(_helper);
            var results = service.GetAllInvoices(DateTime.MinValue, DateTime.MinValue, "pending", 10);
            List<InvoiceViewModel> list = new List<InvoiceViewModel>();
            foreach(var invoice in results)
            {
                InvoiceViewModel model = new InvoiceViewModel();
                model.invoiceId = invoice.InvoiceId;
                model.invoiceNumber = invoice.InvoiceNo;
                model.invoiceDate = invoice.AccountDate;
                model.exchangeRate = invoice.ExRate;
                model.totalLocalAmount = invoice.TotalLocalAmt;

                list.Add(model);
                
            }
            IEnumerable<InvoiceViewModel> lenum = list.AsEnumerable();
            return View(lenum);
        }

        [Authorize]
        [HttpPost]
        public IActionResult GetInvoices([FromBody]InvoiceSearchViewModel model1)
        {
            ViewBag.Name = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            IDBService service = new DBservice(_helper);
            var results = service.GetAllInvoices(model1.From, model1.To, model1.Status, model1.Results);
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
            IEnumerable<InvoiceViewModel> lenum = list.AsEnumerable();
            return View(lenum);
        }
    }
}