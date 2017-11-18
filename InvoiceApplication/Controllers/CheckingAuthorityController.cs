using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InvoiceApplication.Models;
using InvoiceApplication.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using InvoiceApplication.DataAccessLayer;

namespace InvoiceApplication.Controllers
{
    public class CheckingAuthorityController : Controller
    {
        public DbHelper _helper;

        public CheckingAuthorityController(TrackingDbContext context)
        {
            _helper = new DbHelper();
        }
        
        [HttpGet]
        [Authorize(Policy = "Checker")]
        public IActionResult Index(long invoiceId)
        {

            var username = ClaimTypes.NameIdentifier;
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
                service.UpdatePendingStatusForChecker(invoiceId,username);

            }
            else if (status == "Approved")
            {
                service.UpdateCheckedStatusForChecker(invoiceId, username);
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
    }
}