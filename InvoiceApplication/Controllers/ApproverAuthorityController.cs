using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.DbModels;
using Microsoft.AspNetCore.Authorization;
using InvoiceApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApplication.Controllers
{
    public class ApproverAuthorityController : Controller
    {
        public TrackingDbContext _context;

        public ApproverAuthorityController(TrackingDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Policy = "Approver")]
        public IActionResult Index(int invoiceId)
        {


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
        [Authorize(Policy = "")]
        public IActionResult Index(InvoiceApprovalModel model)
        {
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
                InvoiceApprove.IsApprovalPending = true;
                InvoiceApprove.ApprovedBy = "";
                InvoiceApprove.ApprovedDate = DateTime.Now.ToString();

            }
            else if (status == "Approved")
            {
                InvoiceApprove.IsApproved = true;
                InvoiceApprove.ApprovedBy = "";
                InvoiceApprove.ApprovedDate = DateTime.Now.ToString();
            }
            else if (status == "Rejected")
            {
                InvoiceApprove.IsWaitingApproval = false;
                InvoiceApprove.ApprovedBy = "";
                InvoiceApprove.ApprovedDate = DateTime.Now.ToString();
            }
            else
            {

            }

            _context.Entry(InvoiceApprove).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}